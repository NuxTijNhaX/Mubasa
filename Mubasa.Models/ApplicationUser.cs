﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mubasa.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }

        [AllowNull]
        public int? AddressId { get; set; }
        [ForeignKey("AddressId")]
        public Address Address { get; set; }
    }
}
