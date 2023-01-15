using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Mubasa.Web.Models
{
    public class Category
    {
        public int Id { get; set; }

        [DisplayName("Tên")]
        [Required(ErrorMessage="Vui lòng không để trống.")]
        public string Name { get; set; } = string.Empty;
    }
}
