using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mubasa.Models.ViewModels
{
    public class ShoppingCartVM
    {
        public IEnumerable<ShoppingItem> ShoppingCarts { get; set; }
        public double GrandTotal { get; set; } = 0;
    }
}
