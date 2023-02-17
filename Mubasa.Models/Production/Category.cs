using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mubasa.Models
{
    [Table("Categories", Schema = "Production")]
    public class Category
    {
        public int Id { get; set; }

        [DisplayName("Tên")]
        [Required(ErrorMessage = "Vui lòng không để trống.")]
        public string Name { get; set; } = string.Empty;
    }
}
