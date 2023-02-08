using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mubasa.Utility
{
    public static class SD
    {
        public const string Role_Individual_User = "Individual";
        public const string Role_Company_User = "Company";
        public const string Role_Admin = "Admin";
        public const string Role_Employee = "Employee";

        public const string OrderStatusPending = "Pending";
        public const string OrderStatusApproved = "Approved";
        public const string OrderStatusProcessing = "Processing";
        public const string OrderStatusShipped = "Shipped";
        public const string OrderStatusCancelled = "Cancelled";
        public const string OrderStatusRefunded = "Refunded";

        public const string PaymentStatusPending = "Pending";
        public const string PaymentStatusApproved = "Approved";
        public const string OrderStatusDelayed = "ApprovedForDelay";
        public const string PaymentStatusRejected = "Rejected";
    }
}
