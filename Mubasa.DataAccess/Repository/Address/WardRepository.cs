﻿using Mubasa.DataAccess.Data;
using Mubasa.DataAccess.Repository.IRepository;
using Mubasa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mubasa.DataAccess.Repository
{
    public class WardRepository : Repository<Ward>, IWardRepository
    {
        private readonly ApplicationDbContext _db;
        public WardRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
