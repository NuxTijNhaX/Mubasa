using Mubasa.DataAccess.Data;
using Mubasa.DataAccess.Repository.IRepository;
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
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public IAuthorRepository Author { get; private set; }
        public ICategoryRepository Category { get; private set; }
        public ICoverTypeRepository CoverType { get; private set; }
        public IProductRepository Product { get; private set; }
        public IPublisherRepository Publisher { get; private set; }
        public IShoppingCartRepository ShoppingCart { get; private set; }
        public ISupplierRepository Supplier { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            ApplicationUser = new ApplicationUserRepository(db);
            Author = new AuthorRepository(db);
            Category = new CategoryRepository(db);
            CoverType = new CoverTypeRepository(db);
            Product = new ProductRepository(db);
            Publisher = new PublisherRepository(db);
            ShoppingCart = new ShoppingCartRepository(db);
            Supplier = new SupplierRepository(db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
