using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementRepository.App_Data
{
    internal class RoomsTable
    {
        public int GuestID { get; set; }
        public int RoomNumber { get; set; }
        public string RoomType { get; set; }
        public decimal RoomPerNight { get; set; }
    }
}
