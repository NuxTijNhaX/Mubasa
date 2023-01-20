using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Mubasa.Models
{
    public class Publisher
    {
        public int Id { get; set; }

        [DisplayName("Tên")]
        [Required(ErrorMessage = "Vui lòng không để trống.")]
        public string Name { get; set; } = string.Empty;

        public string LogoUrl { get; set; } = string.Empty;
    }
}
