using Microsoft.Extensions.Options;
using Mubasa.Models.ConfigModels;
using Newtonsoft.Json.Linq;

namespace Mubasa.Web.Services.ThirdParties.Carrier
{
    public class GiaoHangNhanh
    {
        public GiaoHangNhanh(IOptions<GiaoHangNhanhConfig> ghn)
        {
            EndPoint = ghn.Value.EndPoint;
            Token = ghn.Value.Token;
            ShopId = ghn.Value.ShopId;
            DistrictId = ghn.Value.DistrictId;
            WardId = ghn.Value.WardId;
        }
        private string EndPoint { get; set; }
        private string Token { get; set; }
        private string ShopId { get; set; }
        private string DistrictId { get; set; }
        private string WardId { get; set; }

        public async Task<IEnumerable<dynamic>?> GetAddress(string path)
        {
            using HttpClient httpClient = new HttpClient();
            using HttpRequestMessage requestMsg = new HttpRequestMessage();
            requestMsg.Method = HttpMethod.Get;
            requestMsg.RequestUri = new Uri($"{EndPoint}/master-data/{path}");
            requestMsg.Headers.Add("Accept", "application/json");
            requestMsg.Headers.Add("Token", Token);

            HttpResponseMessage response = await httpClient.SendAsync(requestMsg);

            string provinceJSON = await response.Content.ReadAsStringAsync();

            JObject provinceObj = JObject.Parse(provinceJSON);
            IEnumerable<dynamic>? provinceData = provinceObj["data"]?.Children().ToList().Cast<dynamic>();

            return provinceData;
        }

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

            string result = string.Empty;
            if (responseData != null)
            {
                DateTime leadTime = new(1970, 1, 1, 0, 0, 0, 0);
                leadTime = leadTime.AddSeconds(double.Parse(responseData.ToString()));
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

            return responseData?.ToString() ?? string.Empty;
        }
    }
}
