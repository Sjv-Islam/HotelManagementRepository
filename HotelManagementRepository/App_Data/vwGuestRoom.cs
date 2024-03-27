using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementRepository.App_Data
{
    internal class vwGuestRoom
    {
        public int GuestID { get; set; }
        public string GuestName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int RoomNumber { get; set; }
        public string RoomType { get; set; }
        public decimal RoomPerNight { get; set; }
    }
}
