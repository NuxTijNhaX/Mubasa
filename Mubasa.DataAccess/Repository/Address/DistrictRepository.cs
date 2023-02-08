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
    public class DistrictRepository : Repository<District>, IDistrictRepository
    {
        private readonly ApplicationDbContext _db;
        public DistrictRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
