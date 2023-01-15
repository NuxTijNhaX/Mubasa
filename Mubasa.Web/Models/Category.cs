using System.ComponentModel.DataAnnotations;

namespace Mubasa.Web.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
