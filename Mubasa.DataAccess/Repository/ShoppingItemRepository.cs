using Mubasa.DataAccess.Data;
using Mubasa.DataAccess.Repository.IRepository;
using Mubasa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mubasa.DataAccess.Repository
{
    public class ShoppingItemRepository : Repository<ShoppingItem>, IShoppingItemRepository
    {
        private readonly ApplicationDbContext _db;
        public ShoppingItemRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public int DecreQuantity(ShoppingItem shoppingItem, int quantity)
        {
            shoppingItem.Count -= quantity;
            return shoppingItem.Count;
        }

        public int IncreQuantity(ShoppingItem shoppingItem, int quantity)
        {
            shoppingItem.Count += quantity;
            return shoppingItem.Count;
        }
    }
}
