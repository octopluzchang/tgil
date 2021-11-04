using System;

namespace PAMO_TapPay
{
    public class PamoSendModel
    {
        public string iss { get; set; }
        public string sub { get; set; }
        public UserData user_data { get; set; }
    }

    public class UserData
    {
        public string last_name { get; set; }
        public string first_name { get; set; }
        public string plate_number { get; set; }
        public string national_id { get; set; }
        public string phone_number { get; set; }
        public string birth { get; set; }
        public string email { get; set; }
    }
}
