using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using yEtiHotel.Models;

namespace yEtiHotel.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<Room> rooms = db.Room.ToList();
            rooms.Sort(new PopularRoomComparer());

            return View(rooms.Take(5));
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}
