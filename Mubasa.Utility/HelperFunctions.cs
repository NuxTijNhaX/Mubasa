using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Mubasa.Utility
{
    public class HelperFunctions
    {
        public static string ComputeHmacSHA256(string key = "", string message = "")
        {
            byte[] keyByte = Encoding.UTF8.GetBytes(key);
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            byte[] hashMessage = new HMACSHA256(keyByte).ComputeHash(messageBytes);

            return BitConverter.ToString(hashMessage).Replace("-", "").ToLower();
        }

        public static string EncodeBase64(string value)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value);

            return Convert.ToBase64String(bytes);
        }

        private static async Task<T> PostAsync<T>(string uri, FormUrlEncodedContent content)
        {
            HttpClient httpClient = new HttpClient();
            var response = await httpClient.PostAsync(uri, content);
            var responseString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseString);
        }

        private static Task<T> PostFormAsync<T>(string uri, Dictionary<string, string> data)
        {
            return PostAsync<T>(uri, new FormUrlEncodedContent(data));
        }

        public static Task<Dictionary<string, string>> PostFormAsync(string uri, Dictionary<string, string> data)
        {
            return PostFormAsync<Dictionary<string, string>>(uri, data);
        }
    }
}
