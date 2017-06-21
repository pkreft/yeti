using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace yEtiHotel.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        public int Cost { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int RoomId { get; set; }

        public string UserId { get; set; }

        public virtual Room Room { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
