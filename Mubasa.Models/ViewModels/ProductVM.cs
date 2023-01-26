using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mubasa.Models.ViewModels
{
    public class ProductVM
    {
        public Product Product { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> AuthorList { get; set; }
        [ValidateNever] 
        public IEnumerable<SelectListItem> CategoryList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> CoverTypeList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> PublisherList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> SupplierList { get; set; }
    }
}
