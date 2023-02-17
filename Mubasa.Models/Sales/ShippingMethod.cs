using System.ComponentModel.DataAnnotations.Schema;

namespace Mubasa.Models
{
    [Table("ShippingMethods", Schema = "Sales")]
    public class ShippingMethod
    {
        public string ServiceId { get; set; }
        public string ServiceTypeId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string ExpectedDay{ get; set; }
    }
}
