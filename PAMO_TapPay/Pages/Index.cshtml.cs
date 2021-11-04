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
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _config;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public IndexModel(ILogger<IndexModel> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public void OnGet()
        { }

        public IActionResult OnGetSnedData(TapPayReceiveModel receiveModel)
        {
            string result = string.Empty;

            try
            {
                string tapPayHost = _config.GetValue<string>("TapPayInfo:TapPayHost");
                string tapPayUrl = _config.GetValue<string>("TapPayInfo:TapPayUrl");
                string appID = _config.GetValue<string>("TapPayInfo:AppID");
                string appKey = _config.GetValue<string>("TapPayInfo:AppKey");
                string partnerKey = _config.GetValue<string>("TapPayInfo:PartnerKey");
                string merchant_id = _config.GetValue<string>("TapPayInfo:Merchant_id");

                string pamoUrl = _config.GetValue<string>("PamoInfo:PamoUrl");
                string pamoAmount = _config.GetValue<string>("PamoInfo:pamoAmount");

                #region TapPayAPI
                TapPaySnedModel tapPaySendModel = new TapPaySnedModel
                {
                    prime = receiveModel.Prime,
                    merchant_id = merchant_id,
                    partner_key = partnerKey,
                    details = "",
                    amount = int.Parse(pamoAmount), 
                    remember = false,
                    cardholder = new Cardholder
                    {
                        phone_number = receiveModel.PhoneNumber,
                        name = receiveModel.LastName + receiveModel.FristName,
                        email = receiveModel.Email,
                        zip_code = "",
                        address = "",
                        national_id = ""
                    }
                };

                string sendJson = JsonConvert.SerializeObject(tapPaySendModel);
                var headers = new Dictionary<string, string> { { "x-api-key", partnerKey } };
                string tapPayResult = new Biz_Client().Client(tapPayHost, tapPayUrl, sendJson, headers);
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
                string accountSid = _config.GetValue<string>("TapPayInfo:TapPayHost");
                string authToken = _config.GetValue<string>("TapPayInfo:TapPayUrl");
                TwilioClient.Init(accountSid, authToken);

                var message = MessageResource.Create(
                    body: "PAMO sell Asshole X Link:" + pamoUrl,
                    from: new Twilio.Types.PhoneNumber("+15017122661"),
                    to: new Twilio.Types.PhoneNumber("+15558675310")
                );
                Console.WriteLine(message);
                #endregion

                result = tapPayResult;
            }
            catch(Exception ex)
            {
                _logger.LogInformation("_Catch:" + ex.ToString());
                logger.Debug("Catch:" + ex.ToString());
                return Content("{\"status\": -1,\"msg\":\"系統發生異常，請聯絡客服人員。\"}");
            }

            return Content(result);
        }

    }
}
