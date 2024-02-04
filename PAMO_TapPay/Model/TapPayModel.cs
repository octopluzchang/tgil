using System;

namespace PAMO_TapPay
{
    public class TravelProReceiveModel
    {
        public string PersonName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Prime { get; set; }
        public int Groups { get; set; }
		public int Plan { get; set; }
		public int Price { get; set; }

	}

	public class TapPaySnedModel
    {
        public string prime { get; set; }
        public string partner_key { get; set; }
        public string merchant_id { get; set; }
        public string details { get; set; }
        public int amount { get; set; }
        public Cardholder cardholder { get; set; }
        public bool remember { get; set; }
    }

    public class Cardholder
    {
        public string phone_number { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string zip_code { get; set; }
        public string address { get; set; }
        public string national_id { get; set; }
    }

}
