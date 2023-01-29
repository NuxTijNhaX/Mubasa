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
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        private readonly ApplicationDbContext _db;
        public ShoppingCartRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public int DecreQuantity(ShoppingCart shoppingCart, int quantity)
        {
            shoppingCart.Count -= quantity;
            return shoppingCart.Count;
        }

        public int IncreQuantity(ShoppingCart shoppingCart, int quantity)
        {
            shoppingCart.Count += quantity;
            return shoppingCart.Count;
        }
    }
}
