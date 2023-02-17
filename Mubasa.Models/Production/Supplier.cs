using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mubasa.Models
{
    [Table("Suppliers", Schema = "Production")]
    public class Supplier
    {
        public int Id { get; set; }

        [DisplayName("Tên")]
        [Required(ErrorMessage = "Vui lòng không để trống.")]
        public string Name { get; set; } = string.Empty;
    }
}
