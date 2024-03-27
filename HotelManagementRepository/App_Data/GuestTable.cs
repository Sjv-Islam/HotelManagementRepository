using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementRepository.App_Data
{
    internal class GuestTable
    {
        public int GuestID { get; set; }
        public string GuestName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        public List<RoomsTable> roomsTables { get; set; } = new List<RoomsTable>();
    }
}
