using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mubasa.Models
{
    public class OrderHeader
    {
        public int Id { get; set; }
        [Required]
        public string? ReceiverName { get; set; }
        [Required]
        public string? ReceiverPhoneNumber { get; set; }
        public double GrandTotal { get; set; }
        public double ShippingCost { get; set; }
        public double Discount { get; set; } = 0;
        public string? OrderStatus { get; set; }
        public string? PaymentStatus { get; set; }
        public string? ShippingInfo { get; set; }
        public string? TrackingNumber { get; set; }
        [Required]
        public string? ShippingAddress { get; set; }

        public int WardId { get; set; }
        public Ward Ward { get; set; }

        public int DistrictId { get; set; }
        public District District { get; set; }

        public int ProvinceId { get; set; }
        public Province Province { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime PaymentedDate { get; set; }
        public DateTime PaymentDueDate { get; set; }

        public int PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }
    }
}
