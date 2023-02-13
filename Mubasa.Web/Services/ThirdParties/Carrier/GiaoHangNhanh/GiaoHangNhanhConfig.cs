using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mubasa.Web.Services.ThirdParties.Carrier.GiaoHangNhanh
{
    public class GiaoHangNhanhConfig
    {
        public string EndPoint { get; set; }
        public string Token { get; set; }
        public string ShopId { get; set; }
        public string DistrictId { get; set; }
        public string WardId { get; set; }
    }
}
