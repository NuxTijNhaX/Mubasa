using Mubasa.Models;
using Mubasa.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Mubasa.Web.Services.ThirdParties.PaymentGateway.ZaloPay
{
    public class ZaloPayConfig
    {
        public int AppId { get; set; }
        public string Api_Key_1 { get; set; }
        public string Api_Key_2 { get; set; }
        public string EndPoint { get; set; }
    }
}
