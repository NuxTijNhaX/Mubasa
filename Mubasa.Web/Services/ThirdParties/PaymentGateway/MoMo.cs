using Microsoft.Extensions.Options;
using Mubasa.Models.ConfigModels;
using Mubasa.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Mubasa.Web.Services.ThirdParties.PaymentGateway
{
    public class MoMo
    {
        public MoMo(IOptions<MoMoConfig> momo)
        {
            PartnerCode = momo.Value.PartnerCode;
            PartnerName = momo.Value.PartnerName;
            StoreId = momo.Value.StoreId;
            AccessKey = momo.Value.AccessKey;
            SecretKey = momo.Value.SecretKey;
            EndPoint = momo.Value.EndPoint;
        }

        private string PartnerCode { get; set; }
        private string PartnerName { get; set; }
        private string StoreId { get; set; }
        private string AccessKey { get; set; }
        private string SecretKey { get; set; }
        private string EndPoint { get; set; }

        public async Task<Dictionary<string, string>> CreateOrder(
            int orderId,
            string paymentName,
            string userId,
            double amount)
        {
            int amountInt = int.Parse(amount.ToString());
            string requestId = Guid.NewGuid().ToString();
            string redirectUrl = $"https://localhost:7153/Customer/ShoppingCart/PaymentResult?orderId={orderId}&paymentName={paymentName}&userId={userId}";
            string ipnUrl = "https://youtube.com";
            string requestType = "captureWallet";
            string lang = "vi";
            string signature;
            string extraData = "";
            string orderInfo = "Thanh toán đơn hàng tại Mubasa.Com";

            signature = "accessKey=" + AccessKey +
                "&amount=" + amountInt +
                "&extraData=" + extraData +
                "&ipnUrl=" + ipnUrl +
                "&orderId=" + orderId.ToString() +
                "&orderInfo=" + orderInfo +
                "&partnerCode=" + PartnerCode +
                "&redirectUrl=" + redirectUrl +
                "&requestId=" + requestId +
                "&requestType=" + requestType
                ;

            signature = HelperFunctions.ComputeHmacSHA256(SecretKey, signature);

            JObject parameters = new JObject()
            {
                { "partnerCode", PartnerCode },
                { "partnerName", PartnerName },
                { "storeId", StoreId },
                { "requestId", requestId },
                { "amount", amountInt },
                { "orderId", orderId.ToString() },
                { "orderInfo", orderInfo },
                { "redirectUrl", redirectUrl },
                { "ipnUrl", ipnUrl },
                { "requestType", requestType },
                { "extraData", extraData },
                { "lang", lang },
                { "signature", signature },
            };

            HttpClient httpClient = new HttpClient();
            var payload = new StringContent(JsonConvert.SerializeObject(parameters), Encoding.UTF8, "application/json");
            var httpResponse = await httpClient.PostAsync(
                new Uri($"{EndPoint}/v2/gateway/api/create"),
                payload);
            var responseString = await httpResponse.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Dictionary<string, string>>(responseString);
        }
    }
}
