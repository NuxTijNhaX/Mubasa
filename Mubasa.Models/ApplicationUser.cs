using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mubasa.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string Region { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
        public string Address { get; set; }
    }
}
