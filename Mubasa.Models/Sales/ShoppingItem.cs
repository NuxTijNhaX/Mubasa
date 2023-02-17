using System.ComponentModel.DataAnnotations.Schema;

namespace Mubasa.Models
{
    [Table("ShoppingItems", Schema = "Sales")]
    public class ShoppingItem
    {
        public int Id { get; set; }
        
        public int Count { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [NotMapped]
        public double SubTotal { get; set; }
    }
}
