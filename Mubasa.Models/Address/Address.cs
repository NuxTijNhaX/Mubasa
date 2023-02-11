using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mubasa.Models
{
    [Table("Addresses", Schema = "Address")]
    public class Address
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string ReceiverName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }

        [NotMapped]
        public string FullAddress { get; set; }
        [NotMapped]
        public bool IsDefault { get; set; }

        [Required]
        public string HomeNumber { get; set; }

        public int WardId { get; set; }
        public Ward Ward { get; set; }

        public int DistrictId { get; set; }
        public District District { get; set; }

        public int ProvinceId { get; set; }
        public Province Province { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
