using Mubasa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mubasa.DataAccess.Repository.IRepository
{
    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    {
        int IncreQuantity(ShoppingCart shoppingCart, int quantity);
        int DecreQuantity(ShoppingCart shoppingCart, int quantity);
    }
}
