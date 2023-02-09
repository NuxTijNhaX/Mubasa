using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mubasa.Models.ViewModels
{
    public class CheckoutVM
    {
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public OrderHeader OrderHeader { get; set; }
        public IEnumerable<ShippingMethod> ShippingMethods { get; set; }
        public IEnumerable<PaymentMethod> PaymentMethods { get; set; }
    }
}
