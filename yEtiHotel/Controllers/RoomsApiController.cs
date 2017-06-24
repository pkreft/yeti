using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using yEtiHotel.Models;

namespace yEtiHotel.Controllers
{
    public class Search
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }

    [RoutePrefix("api")]
    public class RoomsApiController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Route("rooms")]
        [HttpGet]
        public DbSet<Room> Rooms()
        {
            return db.Room;
        }

        [Route("rooms/search")]
        [HttpPost]
        public List<Room> Search(Search search)
        {
            List<Room> searched = new List<Room>();
            foreach (Room room in db.Room)
            {
                bool found = false;
                DateTime date = search.StartDate;
                while (date <= search.EndDate)
                {
                    var reservation = room.Reservations
                        .Where(r => r.StartDate <= date && r.EndDate >= date)
                        .Select(r => (Reservation)r)
                        .FirstOrDefault();
                    if (reservation != null)
                    {
                        found = true;
                        break;
                    }

                    date = date.AddDays((double)1);
                }
                if (found == false)
                {
                    searched.Add(room);
                }

            }

            return searched;
        }
    }
}
