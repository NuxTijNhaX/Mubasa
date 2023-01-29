using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mubasa.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IApplicationUserRepository ApplicationUser { get; }
        IAuthorRepository Author { get; }
        ICategoryRepository Category { get; }
        ICoverTypeRepository CoverType { get; }
        IProductRepository Product { get; }
        IPublisherRepository Publisher { get; }
        IShoppingCartRepository ShoppingCart { get; }
        ISupplierRepository Supplier { get; }

        void Save();
    }
}
