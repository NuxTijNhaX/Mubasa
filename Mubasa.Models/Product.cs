using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mubasa.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = "Không xác định";
        public string Description { get; set; } = "Không xác định";

        [Required]
        public string ISBN { get; set; } = "Không xác định";

        [Required]
        [Range(0, double.MaxValue)]
        public double Price { get; set; }

        [Required]
        public string ImgUrl { get; set; } = "Không xác định";

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int CoverTypeId { get; set; }
        public CoverType CoverType { get; set; }

        public int AuthorId { get; set; }
        public Author Author { get; set; }

        public int PublisherId { get; set; }
        public Publisher Publisher { get; set; }

        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }
    }
}
