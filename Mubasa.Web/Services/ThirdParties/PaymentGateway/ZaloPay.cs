using Microsoft.Extensions.Options;
using Mubasa.Models;
using Mubasa.Models.ConfigModels;
using Mubasa.Utility;
using Newtonsoft.Json;

namespace Mubasa.Web.Services.ThirdParties.PaymentGateway
{
    public class ZaloPay
    {
        public ZaloPay(IOptions<ZaloPayConfig> zalo)
        {
            AppId = zalo.Value.AppId;
            Api_Key_1 = zalo.Value.Api_Key_1;
            Api_Key_2 = zalo.Value.Api_Key_2;
            EndPoint = zalo.Value.EndPoint;
        }

        private int AppId { get; set; }
        private string Api_Key_1 { get; set; }
        private string Api_Key_2 { get; set; }
        private string EndPoint { get; set; }

        public Task<Dictionary<string, string>> CreateOrder(IEnumerable<ShoppingItem> shoppingItems, int orderheaderId, double amount)
        {
            var appid = AppId.ToString();
            var appuser = shoppingItems.FirstOrDefault().ApplicationUserId;
            var apptransid = $"{DateTime.Now.ToString("yyMMdd")}_{Guid.NewGuid()}";
            var embeddata = new
            {
                redirecturl = "https://localhost:7153/Customer/ShoppingCart/GetZaloPayCallback",
                orderheaderid = orderheaderId,
                paymentmethod = SD.PayMethod_Zalo,
            };

            var param = new Dictionary<string, string>
            {
                { "appid", appid },
                { "appuser", appuser },
                { "apptime", DateTimeOffset.Now.ToUnixTimeSeconds().ToString() },
                { "amount", amount.ToString() },
                { "apptransid", apptransid },
                { "embeddata", JsonConvert.SerializeObject(embeddata) },
                { "item", JsonConvert.SerializeObject(shoppingItems) },
                { "description", "Thanh toán đơn hàng tại Mubasa.Com" },
                { "bankcode", "zalopayapp" }
            };

            var data = appid + "|" + param["apptransid"] + "|" + param["appuser"] + "|" + param["amount"] + "|"
                + param["apptime"] + "|" + param["embeddata"] + "|" + param["item"];
            param.Add("mac", HelperFunctions.ComputeHmacSHA256(Api_Key_1, data));

            var response = HelperFunctions.PostFormAsync($"{EndPoint}/createorder", param);

            return response;
        }
    }
}
