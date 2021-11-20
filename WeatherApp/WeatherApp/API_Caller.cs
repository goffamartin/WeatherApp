using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WeatherDesktopApp
{
    public class API_Caller
    {
        public static async Task<API_Response> Get(string url, string authID = null)
        {
            using (var client = new HttpClient())
            {
                if (!string.IsNullOrWhiteSpace(authID))
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", authID);

                var request = await client.GetAsync(url);
                if (request.IsSuccessStatusCode)
                {
                    return new API_Response { Response = await request.Content.ReadAsStringAsync() };
                }
                else
                    return new API_Response { ErrorMessage = request.ReasonPhrase };
            }
        }
    }

    public class API_Response
    {
        public bool Successuful => ErrorMessage == null;
        public string ErrorMessage { get; set; }
        public string Response { get; set; }
    }
}