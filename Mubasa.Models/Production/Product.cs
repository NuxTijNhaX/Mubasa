using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mubasa.Models
{
    [Table("Products", Schema = "Production")]
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string ISBN { get; set; } = string.Empty;

        [Required]
        [Range(0, double.MaxValue)]
        public double Price { get; set; }

        [Required]
        [ValidateNever]
        public string ImgUrl { get; set; } = string.Empty;

        public int CategoryId { get; set; }
        [ValidateNever]
        public Category Category { get; set; }

        public int CoverTypeId { get; set; }
        [ValidateNever] 
        public CoverType CoverType { get; set; }

        public int AuthorId { get; set; }
        [ValidateNever] 
        public Author Author { get; set; }

        public int PublisherId { get; set; }
        [ValidateNever] 
        public Publisher Publisher { get; set; }

        public int SupplierId { get; set; }
        [ValidateNever]
        public Supplier Supplier { get; set; }
    }
}
