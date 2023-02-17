using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mubasa.Models
{
    [Table("Authors", Schema = "Production")]
    public class Author
    {
        public int Id { get; set; }

        [DisplayName("Tên")]
        [Required(ErrorMessage = "Vui lòng không để trống.")]
        public string Name { get; set; } = string.Empty;
    }
}
