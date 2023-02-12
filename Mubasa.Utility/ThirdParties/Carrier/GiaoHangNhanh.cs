using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mubasa.Utility.ThirdParties.Carrier
{
    public class GiaoHangNhanh
    {
        public string EndPoint { get; set; }
        public string Token { get; set; }
        public string ShopId { get; set; }
        public string DistrictId { get; set; }
        public string WardId { get; set; }

        public async Task<IEnumerable<dynamic>?> GetService(string toDistrict)
        {
            using HttpClient httpClient = new HttpClient();
            using HttpRequestMessage requestMsg = new HttpRequestMessage();
            requestMsg.Method = HttpMethod.Get;
            requestMsg.RequestUri = new Uri($"{EndPoint}/v2/shipping-order/available-services?from_district={DistrictId}&to_district={toDistrict}&shop_id={ShopId}");
            requestMsg.Headers.Add("Accept", "application/json");
            requestMsg.Headers.Add("Token", Token);

            HttpResponseMessage response = await httpClient.SendAsync(requestMsg);

            string serviceJSON = await response.Content.ReadAsStringAsync();

            JObject serviceObj = JObject.Parse(serviceJSON);
            IEnumerable<dynamic>? serviceData = serviceObj["data"]?.Children().ToList().Cast<dynamic>();

            return serviceData;
        }

        public async Task<string> GetLeadTime(string toDistrict, string toWard, string serviceId)
        {
            using HttpClient httpClient = new HttpClient();
            using HttpRequestMessage requestMsg = new HttpRequestMessage();
            requestMsg.Method = HttpMethod.Get;
            requestMsg.RequestUri = new Uri($"{EndPoint}/v2/shipping-order/leadtime?from_district_id={DistrictId}&from_ward_code={WardId}&to_district_id={toDistrict}&to_ward_code={toWard}&service_id={serviceId}");
            requestMsg.Headers.Add("Accept", "application/json");
            requestMsg.Headers.Add("ShopId", ShopId);
            requestMsg.Headers.Add("Token", Token);

            HttpResponseMessage response = await httpClient.SendAsync(requestMsg);

            string responseJSON = await response.Content.ReadAsStringAsync();

            JObject responseObj = JObject.Parse(responseJSON);
            var responseData = responseObj["data"]["leadtime"];

            string result = String.Empty;
            if(responseData != null)
            {
                DateTime leadTime = new(1970, 1, 1, 0, 0, 0, 0);
                leadTime = leadTime.AddSeconds(Double.Parse(responseData.ToString()));
                result = leadTime.ToString(format: "dd/MM/yyyy");
            }

            return result;
        }

        public async Task<string> GetFee(string toDistrict, string toWard, string serviceId, string serviceType)
        {
            using HttpClient httpClient = new HttpClient();
            using HttpRequestMessage requestMsg = new HttpRequestMessage();
            requestMsg.Method = HttpMethod.Get;
            requestMsg.RequestUri = new Uri($"{EndPoint}/v2/shipping-order/fee?from_district={DistrictId}&service_id={serviceId}&service_type_id={serviceType}&to_district_id={toDistrict}&to_ward_code={toWard}&weight=200");
            requestMsg.Headers.Add("Accept", "application/json");
            requestMsg.Headers.Add("ShopId", ShopId);
            requestMsg.Headers.Add("Token", Token);

            HttpResponseMessage response = await httpClient.SendAsync(requestMsg);

            string responseJSON = await response.Content.ReadAsStringAsync();

            JObject responseObj = JObject.Parse(responseJSON);
            var responseData = responseObj["data"]["total"];

            return responseData?.ToString() ?? String.Empty;
        }
    }
}
