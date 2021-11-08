using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PAMO_TapPay.Biz;
using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace PAMO_TapPay.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _config;
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        public IndexModel(IConfiguration config)
        {
            _config = config;
        }

        public string Env { get; set; }
        public string AppID { get; set; }
        public string AppKey { get; set; }

        public void OnGet()
        {

            Env = _config.GetValue<string>("TapPayInfo:Env");
            AppID = _config.GetValue<string>("TapPayInfo:AppID");
            AppKey = _config.GetValue<string>("TapPayInfo:AppKey");
        }

        public IActionResult OnGetSnedData(TapPayReceiveModel receiveModel)
        {
            string result = string.Empty;
            bool isPayAlready = false;
            _logger.Trace("_Info:" + JsonConvert.SerializeObject(receiveModel));
            try
            {
                var TapPayHost = _config.GetValue<string>("TapPayInfo:TapPayHost");
                var TapPayUrl = _config.GetValue<string>("TapPayInfo:TapPayUrl");
                var PartnerKey = _config.GetValue<string>("TapPayInfo:PartnerKey");
                var Merchant_id = _config.GetValue<string>("TapPayInfo:Merchant_id");
                var PamoUrl = _config.GetValue<string>("PamoInfo:PamoUrl");
                var PamoAmount = _config.GetValue<string>("PamoInfo:PamoAmount");
                var AccountSid = _config.GetValue<string>("TwilioInfo:Account_Sid");
                var AuthToken = _config.GetValue<string>("TwilioInfo:Auth_Token");
                var PhoneNoFrom = _config.GetValue<string>("TwilioInfo:PhoneNoFrom");

                #region TapPayAPI
                TapPaySnedModel tapPaySendModel = new TapPaySnedModel
                {
                    prime = receiveModel.Prime,
                    merchant_id = Merchant_id,
                    partner_key = PartnerKey,
                    details = "",
                    amount = int.Parse(PamoAmount),
                    remember = false,
                    cardholder = new Cardholder
                    {
                        phone_number = receiveModel.PhoneNumber,
                        name = IsASCIIForeigner(receiveModel.LastName) ? $"{ receiveModel.FirstName } { receiveModel.LastName }" : $"{ receiveModel.LastName }{ receiveModel.FirstName }",
                        email = receiveModel.Email,
                        zip_code = "",
                        address = "",
                        national_id = ""
                    }
                };

                string sendJson = JsonConvert.SerializeObject(tapPaySendModel);
                var headers = new Dictionary<string, string> { { "x-api-key", PartnerKey } };
                string tapPayResult = new Biz_Client().Client(TapPayHost, TapPayUrl, sendJson, headers);
                dynamic tapPayObject = JsonConvert.DeserializeObject<dynamic>(tapPayResult);
                if (tapPayObject["status"] != "0")
                {
                    _logger.Trace("_TapPay_Error:" + JsonConvert.SerializeObject(tapPayObject));
                    return Content("{\"status\":\"-2\",\"msg\":\"付款失敗，請檢查卡號相關資料是否正確。\"}");
                }
                isPayAlready = true;
                #endregion

                #region PamoUrl
                PamoSendModel pamoSendModel = new PamoSendModel
                {
                    iss = "iss",
                    sub = "sub",
                    user_data = new UserData
                    {
                        last_name = "",
                        first_name = "",
                        plate_number = "",
                        national_id = "",
                        phone_number = "",
                        birth = "",
                        email = ""
                    }

                };
                #endregion

                #region TwilioSMSAPI
                TwilioClient.Init(AccountSid, AuthToken);

                var message = MessageResource.Create(
                    body: $"Hi, {receiveModel.FirstName}\n歡迎使用PAMO會員系統！\nLink: {PamoUrl}",
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
            if (s.Any(c => c < 'a' || c > 'z'))
                return true; //that is foreigner when owner anyone english word

            return false;
        }

    }
}
