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
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public IAddressRepository Address { get; private set; }
        public IProvinceRepository Province { get; private set; }
        public IDistrictRepository District { get; private set; }
        public IWardRepository Ward { get; private set; }

        public IApplicationUserRepository ApplicationUser { get; private set; }
        public IAuthorRepository Author { get; private set; }
        public ICategoryRepository Category { get; private set; }
        public ICoverTypeRepository CoverType { get; private set; }
        public IProductRepository Product { get; private set; }
        public IPublisherRepository Publisher { get; private set; }
        public IShoppingItemRepository ShoppingItem { get; private set; }
        public ISupplierRepository Supplier { get; private set; }
        public IOrderHeaderRepository OrderHeader { get; private set; }
        public IOrderDetailRepository OrderDetail { get; private set; }

        public IPaymentMethodRepository PaymentMethod { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;

            Address = new AddressRepository(db);
            Province = new ProvinceRepository(db);
            District = new DistrictRepository(db);
            Ward = new WardRepository(db);

            ApplicationUser = new ApplicationUserRepository(db);
            Author = new AuthorRepository(db);
            Category = new CategoryRepository(db);
            CoverType = new CoverTypeRepository(db);
            OrderHeader = new OrderHeaderRepository(db);
            OrderDetail = new OrderDetailRepository(db);
            Product = new ProductRepository(db);
            Publisher = new PublisherRepository(db);
            ShoppingItem = new ShoppingItemRepository(db);
            Supplier = new SupplierRepository(db);

            PaymentMethod = new PaymentMethodRepository(db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
