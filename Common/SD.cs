using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class SD//SD=StaticDetail
    {
        public const string RoleAdmin = "Admin";
        public const string RoleCustomer = "Customer";
        public const string RoleEmployeee = "Employee";

        public const string LocalInitialBooking = "InitialRoomBookingInfo";
        public const string LocalRoomOrderDetail = "LocalRoomOrderDetail";
        public const string LocalToken = "JWT Token";
        public const string LocalUserDetails = "User Details";

        public const string StatusPending = "Pending";
        public const string StatusBooked = "Booked";
        public const string StatusCheckedIn = "CheckedIn";
        public const string StatusCheckedOut = "CheckedOut";
        public const string StatusNoShow = "NoShow";
        public const string StatusCancelled = "Cancelled";
    }
}
