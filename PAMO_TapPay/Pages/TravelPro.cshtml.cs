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
using System.Net.Mail;

namespace PAMO_TapPay.Pages
{
    public class TravelProModel : PageModel
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IConfiguration _config;
        public TravelProModel(IConfiguration config)
        {
            _config = config;
            Clinet = new Biz_Client();
        }

        public string Env { get; set; }
        public string AppID { get; set; }
        public string AppKey { get; set; }
        public string Price { get; set; }
        private readonly Biz_Client Clinet;

        public void OnGet()
        {
            Env = _config.GetValue<string>("TapPayInfo:Env");
            AppID = _config.GetValue<string>("TapPayInfo:AppID");
            AppKey = _config.GetValue<string>("TapPayInfo:AppKey");
            Price = _config.GetValue<string>("PamoInfo:PamoPrice3");
        }


        public IActionResult OnPostSnedData(TravelProReceiveModel receiveModel)
        {
            _logger.Trace("_Info:" + JsonConvert.SerializeObject(receiveModel));

            bool isPayAlready = false;
            bool isSendSMS = false;
            string result;

            try
            {
                #region Get Config
                var TapPayHost = _config.GetValue<string>("TapPayInfo:TapPayHost");
                var TapPayUrl = _config.GetValue<string>("TapPayInfo:TapPayUrl");
                var PartnerKey = _config.GetValue<string>("TapPayInfo:PartnerKey");
                var Merchant_id = _config.GetValue<string>("TapPayInfo:Merchant_id");
                var PamoPrice = _config.GetValue<string>("PamoInfo:PamoPrice3");
                var AccountSid = _config.GetValue<string>("TwilioInfo:Account_Sid");
                var AuthToken = _config.GetValue<string>("TwilioInfo:Auth_Token");
                var PhoneNoFrom = _config.GetValue<string>("TwilioInfo:PhoneNoFrom");

                var Port = _config.GetValue<string>("Gmail:Port");
                var Host = _config.GetValue<string>("Gmail:Host");
                var GmailId = _config.GetValue<string>("Gmail:Id");
                var GmailPassword = _config.GetValue<string>("Gmail:Password");
                var FromEmail = _config.GetValue<string>("Gmail:FromEmail");
                var FromName = _config.GetValue<string>("Gmail:FromName");
                var ToEmail = _config.GetValue<string>("Gmail:ToEmail");
                #endregion

                #region info
                var sY = receiveModel.StartDate.Year - 1911;
                var sM = receiveModel.StartDate.Month;
                var sD = receiveModel.StartDate.Day;
                var eY = receiveModel.StartDate.AddDays(receiveModel.Days).Year - 1911;
                var eM = receiveModel.StartDate.AddDays(receiveModel.Days).Month;
                var eD = receiveModel.StartDate.AddDays(receiveModel.Days).Day;
                var dateRange = $"{sY}年{sM}月{sD}日至{eY}年{eM}月{eD}日";
                var totalAmount = int.Parse(PamoPrice) * receiveModel.Days;
                #endregion

                #region TapPayAPI

                var tapPaySendModel = new TapPaySnedModel
                {
                    prime = receiveModel.Prime,
                    merchant_id = Merchant_id,
                    partner_key = PartnerKey,
                    details = $"PAMO旅行法保-{receiveModel.CompanyName},{dateRange}",
                    amount = totalAmount,
                    remember = false,
                    cardholder = new Cardholder
                    {
                        phone_number = receiveModel.PhoneNumber,
                        name = receiveModel.PersonName,
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
                    body: $"{receiveModel.PersonName}您好，您已取得{dateRange}旅行法保服務，請您點擊下方連結便於聯繫PAMO。\n https://lin.ee/SbFLz7Y",
                    from: new Twilio.Types.PhoneNumber(PhoneNoFrom),
                    to: new Twilio.Types.PhoneNumber($"+886{receiveModel.PhoneNumber.Remove(0, 1)}")
                );

                if (message.ErrorCode != null)
                {
                    _logger.Trace("_SMS_Error:" + JsonConvert.SerializeObject(message));
                    result = "{\"status\":\"-3\",\"msg\":\"簡訊寄送失敗，請聯絡客服人員。\"}";
                }
                else
                    isSendSMS = true;


                #endregion

                #region Gmail
                //var email = new MailMessage();
                //email.From = new MailAddress(FromEmail, FromName, Encoding.UTF8);
                //email.To.Add(ToEmail);
                //email.Subject = $"PAMO的旅行法保-{receiveModel.PersonName}";
                //email.Body = $"<h4>姓名:</h4><p>{receiveModel.PersonName}</p>" +
                //             $"<h4>聯絡手機:</h4><p>{receiveModel.PhoneNumber}</p>" +
                //             $"<h4>租車車行:</h4><p>{receiveModel.CompanyName}</p>" +
                //             $"<h4>租車期間:</h4><p>{dateRange}</p>" +
                //             $"<h4>旅行法保費用:</h4><p>{totalAmount}</p>";
                //email.IsBodyHtml = true;
                //var smtp = new SmtpClient();
                //smtp.Host = Host;
                //smtp.Port = int.Parse(Port);
                //smtp.Credentials = new NetworkCredential(GmailId, GmailPassword);
                //smtp.EnableSsl = true;
                //smtp.Send(email);
                //smtp.Dispose();
                #endregion

                _logger.Trace("_Done:" + JsonConvert.SerializeObject(message));
                result = "{\"status\":\"0\",\"msg\":\"付款成功！簡訊已寄出，請留意您手機訊息。\"}";

            }
            catch (Exception ex)
            {
                _logger.Trace("_Catch:" + ex.ToString());
                if (isPayAlready && !isSendSMS)
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
