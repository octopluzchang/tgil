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
                        name = IsASCIIForeigner(receiveModel.LastName) ? $"{ receiveModel.FirstName } { receiveModel.LastName }" : $"{ receiveModel.LastName }{receiveModel.FirstName }",
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
                    return Content(tapPayResult);
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
                    body: "PAMO sell Asshole X Link:" + PamoUrl,
                    from: new Twilio.Types.PhoneNumber(PhoneNoFrom),
                    to: new Twilio.Types.PhoneNumber($"+886{receiveModel.PhoneNumber}")
                );
                _logger.Trace("_Done:" + message);
                #endregion

                result = tapPayResult;
            }
            catch (Exception ex)
            {
                _logger.Trace("_Catch:" + ex.ToString());
                return Content("{\"status\": -1,\"msg\":\"系統發生異常，請聯絡客服人員。\"}");
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
