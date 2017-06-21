using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace yEtiHotel.Models
{
    public enum Exposure
    {
        Northern, Eastern, Southern, Western
    }

    public class Room
    {
        public int Id { get; set; }

        public int Number { get; set; }

        public int Bedrooms { get; set; }

        public int Area { get; set; }

        public int Cost { get; set; }

        public Exposure Exposure { get; set; }

        public virtual ICollection<Reservation> Reservations { get; set; }
    }

    public class PopularRoomComparer : IComparer<Room>
    {
        public int Compare(Room a, Room b)
        {
            return a.Reservations.Count() > b.Reservations.Count() ? -1 : 1;
        }
    }
}
