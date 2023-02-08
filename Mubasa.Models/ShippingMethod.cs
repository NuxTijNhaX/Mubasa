using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mubasa.Models
{
    public class ShippingMethod
    {
        public string ServiceId { get; set; }
        public string ServiceTypeId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string ExpectedDay{ get; set; }
    }
}
