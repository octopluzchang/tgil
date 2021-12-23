using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace PAMO_TapPay.Biz
{

    public class Biz_Client
    {

        private static bool ValidateServerCertificate(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public string Post(string clientHost, string clientUrl, string postString, Dictionary<string, string> headers = null)
        {
            try
            {
                //設定 HTTPS 連線時，不要理會憑證的有效性問題
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);

                //建立 HttpClient
                HttpClient client = new HttpClient();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                //將轉為 string 的 json 依編碼並指定 content type 存為 httpcontent
                if (headers != null)
                {
                    foreach (var header in headers)
                    { client.DefaultRequestHeaders.Add(header.Key, header.Value); };
                }
                HttpContent contentPost = new StringContent(postString, Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync(clientHost + clientUrl, contentPost).GetAwaiter().GetResult();

                //將回應結果內容取出並轉為 string 再透過 linqpad 輸出
                return response.Content.ReadAsStringAsync().GetAwaiter().GetResult().ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string Get(string clientHost, string clientUrl, string getString)
        {
            try
            {
                //設定 HTTPS 連線時，不要理會憑證的有效性問題
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);

                //建立 HttpClient
                HttpClient client = new HttpClient();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var url = clientHost + clientUrl + getString;
                HttpResponseMessage response = client.GetAsync(url).GetAwaiter().GetResult();

                //將回應結果內容取出並轉為 string 再透過 linqpad 輸出
                return response.Content.ReadAsStringAsync().GetAwaiter().GetResult().ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
