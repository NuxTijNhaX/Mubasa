using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mubasa.Models
{
    [Table("CoverTypes", Schema = "Production")]
    public class CoverType
    {
        public int Id { get; set; }

        [DisplayName("Tên")]
        [Required(ErrorMessage = "Vui lòng không để trống.")]
        public string Name { get; set; } = string.Empty;
    }
}
