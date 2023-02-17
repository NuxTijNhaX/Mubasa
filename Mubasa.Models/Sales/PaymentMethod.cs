using System.ComponentModel.DataAnnotations.Schema;

namespace Mubasa.Models
{
    [Table("PaymentMethods", Schema = "Sales")]
    public class PaymentMethod
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
