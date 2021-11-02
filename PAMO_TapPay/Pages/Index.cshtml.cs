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
using System.Threading.Tasks;

namespace PAMO_TapPay.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _config;


        public IndexModel(ILogger<IndexModel> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public void OnGet()
        {
        }


        public IActionResult OnGetSnedData(TapPayReceiveModel receiveModel)
        {
            string tapPayHost = _config.GetValue<string>("TapPayInfo:TapPayHost");
            string tapPayUrl = _config.GetValue<string>("TapPayInfo:TapPayUrl");
            string appID = _config.GetValue<string>("TapPayInfo:AppID");
            string appKey = _config.GetValue<string>("TapPayInfo:AppKey");
            string partnerKey = _config.GetValue<string>("TapPayInfo:PartnerKey");
            string merchant_id = _config.GetValue<string>("TapPayInfo:Merchant_id");

            TapPaySnedModel sendModel = new TapPaySnedModel
            {
                prime = receiveModel.Prime,
                merchant_id = merchant_id,
                partner_key = partnerKey,
                details = "",
                amount = 1, //TODO test
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

            string sendJson = JsonConvert.SerializeObject(sendModel);
            var headers = new Dictionary<string, string> { { "x-api-key", partnerKey } };
            string tapPayResult = new Biz_Client().Client(tapPayHost, tapPayUrl, sendJson, headers);

            dynamic tapPayObject = JsonConvert.DeserializeObject<dynamic>(tapPayResult);
            if (tapPayObject["status"] != "0")
                return Content(tapPayResult);


            //TODO PAMO Create Account API

            //TODO Twilio SMS API

            return Content(tapPayResult);
        }

    }
}
