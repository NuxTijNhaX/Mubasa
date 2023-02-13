using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mubasa.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IAddressRepository Address { get; }
        IProvinceRepository Province { get; }
        IDistrictRepository District { get; }
        IWardRepository Ward { get; }

        IApplicationUserRepository ApplicationUser { get; }
        IAuthorRepository Author { get; }
        ICategoryRepository Category { get; }
        ICoverTypeRepository CoverType { get; }
        IOrderHeaderRepository OrderHeader { get; }
        IOrderDetailRepository OrderDetail { get; }
        IProductRepository Product { get; }
        IPublisherRepository Publisher { get; }
        IShoppingItemRepository ShoppingItem { get; }
        ISupplierRepository Supplier { get; }

        IPaymentMethodRepository PaymentMethod { get; }

        void Save();
    }
}
