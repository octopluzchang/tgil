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
    public class RentalcarModel : PageModel
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IConfiguration _config;
        public RentalcarModel(IConfiguration config)
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
                var PamoPrice = _config.GetValue<string>("PamoInfo:PamoPrice2");
                var AccountSid = _config.GetValue<string>("TwilioInfo:Account_Sid");
                var AuthToken = _config.GetValue<string>("TwilioInfo:Auth_Token");
                var PhoneNoFrom = _config.GetValue<string>("TwilioInfo:PhoneNoFrom");
                #endregion

                #region TapPayAPI
                var fullName = IsChinese(receiveModel.LastName) ? $"{ receiveModel.LastName }{ receiveModel.FirstName }" : $"{ receiveModel.FirstName } { receiveModel.LastName }";
                TapPaySnedModel tapPaySendModel = new TapPaySnedModel
                {
                    prime = receiveModel.Prime,
                    merchant_id = Merchant_id,
                    partner_key = PartnerKey,
                    details = "PAMO的負擔保紙本租賃合約",
                    amount = int.Parse(PamoPrice),
                    remember = false,
                    cardholder = new Cardholder
                    {
                        phone_number = receiveModel.PhoneNumber,
                        name = fullName,
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

                #region TwilioSMSAPI
                TwilioClient.Init(AccountSid, AuthToken);

                var message = MessageResource.Create(
                    body: $"{fullName}您好，感謝您選擇PAMO。我們會盡快與您聯繫確認發票資訊。您也可以用LINE跟我們聯絡 https://lin.ee/zWVQXJU",
                    from: new Twilio.Types.PhoneNumber(PhoneNoFrom),
                    to: new Twilio.Types.PhoneNumber($"+886{receiveModel.PhoneNumber.Remove(0, 1)}")
                );

                if (message.ErrorCode == null)
                {
                    _logger.Trace("_Done:" + JsonConvert.SerializeObject(message));
                    result = "{\"status\":\"0\",\"msg\":\"付款成功！簡訊已寄出，請留意您手機訊息。\"}";
                }
                else
                {
                    _logger.Trace("_SMS_Error:" + JsonConvert.SerializeObject(message));
                    result = "{\"status\":\"-3\",\"msg\":\"簡訊寄送失敗，請聯絡客服人員。\"}";
                }
                #endregion

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

        public static bool IsChinese(string s)
        {
            Boolean flag = true;
            int dstringmax = Convert.ToInt32("9fff", 16);
            int dstringmin = Convert.ToInt32("4e00", 16);
            for (int i = 0; i < s.Length; i++)
            {
                int dRange = Convert.ToInt32(Convert.ToChar(s.Substring(i, 1)));
                if (dRange >= dstringmin && dRange < dstringmax)
                {
                    flag = true;
                }
                else
                {
                    flag = false;
                    break;
                }
            }
            return flag;
        }

    }
}
