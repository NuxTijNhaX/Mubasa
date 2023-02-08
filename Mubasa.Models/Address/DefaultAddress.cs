using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mubasa.Models
{
    [Table("DefaultAddresses", Schema = "Address")]
    public class DefaultAddress
    {
        public int Id { get; set; }

        public int AddressId { get; set; }
        public Address Address { get; set; }

        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }
    }
}
