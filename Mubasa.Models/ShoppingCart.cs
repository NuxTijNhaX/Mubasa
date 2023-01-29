﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mubasa.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        
        public int Count { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
