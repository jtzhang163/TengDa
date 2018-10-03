using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace TengDa.Web
{
    public static class HttpHelper
    {
        public static string HttpGet(string url)
        {
            using (var client = new HttpClient())
            {
                var responseString = client.GetStringAsync(url);
            }
            return "";
        }

        public static async Task<string> HttpPostAsync(string url, List<KeyValuePair<string, string>> values)
        {
            var responseString = string.Empty;
            using (var client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(values);
                var response = await client.PostAsync(url, content);
                responseString = await response.Content.ReadAsStringAsync();
            }
            return responseString;
        }
    }
}
