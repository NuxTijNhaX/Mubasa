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
        // public const string Role_Company_User = "Company";
        public const string Role_Admin = "Admin";
        public const string Role_Employee = "Employee";

        public const string PayMethod_COD = "COD";
        public const string PayMethod_Zalo = "Zalo";
        public const string PayMethod_MoMo = "MoMo";

        public const string OrderWait4Pay = "Wait4Paying";
        public const string OrderPending = "Pending";
        public const string OrderProcessing = "Processing";
        public const string OrderShipping= "Shipping";
        public const string OrderCompleted = "Completed";
        public const string OrderReturned = "Returned";
        public const string OrderCancelled = "Cancelled";
        public const string OrderAll = "All";

        public const string PaymentWaiting = "Waiting";
        public const string PaymentPaid = "Paid";
        public const string PaymentRefunded = "Refunded";
        public const string PaymentCancelled = "Cancelled";

        public const string SessionCart = "CartCount";
    }
}
