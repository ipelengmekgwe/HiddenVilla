using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class RoomOrderDetailDto
    {
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string StripeSessionId { get; set; }
        [Required]
        public DateTime CheckInDate { get; set; }
        [Required]
        public DateTime CheckOutDate { get; set; }
        public DateTime ActualCheckInDate { get; set; }
        public DateTime ActualCheckOutDate { get; set; }
        [Required]
        public double TotalCost { get; set; }
        [Required]
        public int RoomId { get; set; }
        public bool IsPaymentSuccessful { get; set; } = false;
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public HotelRoomDto HotelRoomDto { get; set; } = new HotelRoomDto();
        public string Status { get; set; }
    }
}
