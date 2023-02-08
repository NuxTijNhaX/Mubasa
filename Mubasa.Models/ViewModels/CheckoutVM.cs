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
        public IEnumerable<ShoppingItem> ShoppingCarts { get; set; }
        public Address Address { get; set; }
        public OrderHeader OrderHeader { get; set; }
        public IEnumerable<ShippingMethod> ShippingMethods { get; set; }
        public IEnumerable<PaymentMethod> PaymentMethods { get; set; }
        public double SubTotal { get; set; } = 0;
    }
}
