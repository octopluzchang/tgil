using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PAMO_TapPay.Biz;
using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using System.Text;
using System.Web;
using System.Net;
using System.IO;

namespace PAMO_TapPay.Pages
{
    public class IndexModel : PageModel
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IConfiguration _config;
        public IndexModel(IConfiguration config)
        {
            _config = config;
            Clinet = new Biz_Client();
        }

        public string Env { get; set; }
        public string AppID { get; set; }
        public string AppKey { get; set; }
        private readonly Biz_Client Clinet;

        public void OnGet()
        {
            Env = _config.GetValue<string>("TapPayInfo:Env");
            AppID = _config.GetValue<string>("TapPayInfo:AppID");
            AppKey = _config.GetValue<string>("TapPayInfo:AppKey");
        }


        public IActionResult OnPostSnedData(TapPayReceiveModel receiveModel)
        {
            _logger.Trace("_Info:" + JsonConvert.SerializeObject(receiveModel));

            bool isPayAlready = false;
            string result;

            try
            {
                #region Get Config
                var TapPayHost = _config.GetValue<string>("TapPayInfo:TapPayHost");
                var TapPayUrl = _config.GetValue<string>("TapPayInfo:TapPayUrl");
                var PartnerKey = _config.GetValue<string>("TapPayInfo:PartnerKey");
                var Merchant_id = _config.GetValue<string>("TapPayInfo:Merchant_id");
                var PamoIss = _config.GetValue<string>("PamoInfo:PamoIss");
                var PamoSub = _config.GetValue<string>("PamoInfo:PamoSub");
                var PamoUrl = _config.GetValue<string>("PamoInfo:PamoUrl");
                var PamoPrice = _config.GetValue<string>("PamoInfo:PamoPrice");
                var PamoSecret = _config.GetValue<string>("PamoInfo:PamoSecret");
                var BitlyUrl = _config.GetValue<string>("Bitly:BitlyUrl");
                var BitlyApiKey = _config.GetValue<string>("Bitly:ApiKey");
                var AccountSid = _config.GetValue<string>("TwilioInfo:Account_Sid");
                var AuthToken = _config.GetValue<string>("TwilioInfo:Auth_Token");
                var PhoneNoFrom = _config.GetValue<string>("TwilioInfo:PhoneNoFrom");
                #endregion

                #region TapPayAPI
                TapPaySnedModel tapPaySendModel = new TapPaySnedModel
                {
                    prime = receiveModel.Prime,
                    merchant_id = Merchant_id,
                    partner_key = PartnerKey,
                    details = "PAMO安心方案",
                    amount = int.Parse(PamoPrice),
                    remember = false,
                    cardholder = new Cardholder
                    {
                        phone_number = receiveModel.PhoneNumber,
                        name = IsASCIIForeigner(receiveModel.LastName) ? $"{ receiveModel.FirstName } { receiveModel.LastName }" : $"{ receiveModel.LastName }{ receiveModel.FirstName }",
                        email = receiveModel.Email ?? "",
                        zip_code = "",
                        address = "",
                        national_id = ""
                    }
                };  

                string sendJson = JsonConvert.SerializeObject(tapPaySendModel);
                var headers = new Dictionary<string, string> { { "x-api-key", PartnerKey } };
                string tapPayResult = Clinet.Post(TapPayHost, TapPayUrl, sendJson, headers);
                dynamic tapPayObject = JsonConvert.DeserializeObject<dynamic>(tapPayResult);
                if (tapPayObject["status"] != "0")
                {
                    _logger.Trace("_TapPay_Error:" + JsonConvert.SerializeObject(tapPayObject));
                    return Content("{\"status\":\"-2\",\"msg\":\"付款失敗，請檢查卡號相關資料是否正確。\"}");
                }
                isPayAlready = true;
                #endregion

                #region PamoAuth
                //  Use Chilkat.Jwt
                Chilkat.Jwt jwt = new Chilkat.Jwt();
                //  Build the JOSE header
                Chilkat.JsonObject jose = new Chilkat.JsonObject();
                //  Use HS256.  Pass the string "HS384" or "HS512" to use a different algorithm.
                jose.AppendString("alg", "HS256");
                jose.AppendString("typ", "JWT");
                //  Now build the JWT claims (also known as the payload)
                Chilkat.JsonObject claims = new Chilkat.JsonObject();
                claims.AppendString("iss", PamoIss);
                claims.AppendString("sub", PamoSub);
                var userDataJsonObject = claims.AppendObject("user_data");
                userDataJsonObject.AppendString("last_name", receiveModel.LastName);
                userDataJsonObject.AppendString("first_name", receiveModel.FirstName);
                userDataJsonObject.AppendString("plate_number", "");
                userDataJsonObject.AppendString("national_id", "");
                userDataJsonObject.AppendString("phone_number", receiveModel.PhoneNumber);
                userDataJsonObject.AppendString("birth", "");
                userDataJsonObject.AppendString("email", receiveModel.Email ?? "");

                jwt.AutoCompact = true;
                string strJwt = jwt.CreateJwt(jose.Emit(), claims.Emit(), PamoSecret);
                #endregion

                #region Make Short Url
                var longUrl = $"{ PamoUrl }?auth={ strJwt }";
                var bitlydata = $"?access_token={ BitlyApiKey }&longUrl={HttpUtility.UrlEncode(longUrl)}";
                var bitlyResponse = Clinet.Get(BitlyUrl, "", bitlydata);
                var bitlyResponseJObject = JObject.Parse(bitlyResponse);
                int statusCode = bitlyResponseJObject["status_code"].Value<int>();
                string tempPamoUrl = longUrl;
                if (statusCode == (int)HttpStatusCode.OK)
                    tempPamoUrl = bitlyResponseJObject["data"]["url"].Value<string>();
                #endregion

                #region TwilioSMSAPI
                TwilioClient.Init(AccountSid, AuthToken);

                var message = MessageResource.Create(
                    body: $"Hi, {receiveModel.FirstName}\n歡迎使用PAMO會員系統！\nLink: {tempPamoUrl}",
                    from: new Twilio.Types.PhoneNumber(PhoneNoFrom),
                    to: new Twilio.Types.PhoneNumber($"+886{receiveModel.PhoneNumber.Remove(0, 1)}")
                );

                if (message.ErrorCode == null)
                {
                    result = "{\"status\":\"0\",\"msg\":\"付款成功！簡訊已寄出，請留意您手機訊息。\"}";
                }
                else
                {
                    _logger.Trace("_SMS_Error:" + JsonConvert.SerializeObject(message));
                    result = "{\"status\":\"-3\",\"msg\":\"簡訊寄送失敗，請聯絡客服人員。\"}";
                }
                #endregion

                _logger.Trace("_Done:" + JsonConvert.SerializeObject(message));
            }
            catch (Exception ex)
            {
                _logger.Trace("_Catch:" + ex.ToString());
                if (isPayAlready)
                {   //status = -1 系統異常、-2 TapPay付款異常、-3 已付款成功，但簡訊寄送異常
                    return Content("{\"status\":\"-3\",\"msg\":\"簡訊系統異常，請聯絡客服人員。\"}");
                }
                else
                {
                    return Content("{\"status\":\"-1\",\"msg\":\"系統發生異常，請聯絡客服人員。\"}");
                }
            }

            return Content(result);
        }

        public static bool IsASCIIForeigner(string s)
        {
            if (!s.Any(c => c < 'a' || c > 'z'))
                return true; //that is foreigner when name have any one word of eng

            return false;
        }

    }
}
