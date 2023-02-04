using Mubasa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mubasa.DataAccess.Repository.IRepository
{
    public interface IShoppingItemRepository : IRepository<ShoppingItem>
    {
        int IncreQuantity(ShoppingItem shoppingItem, int quantity);
        int DecreQuantity(ShoppingItem shoppingItem, int quantity);
    }
}
