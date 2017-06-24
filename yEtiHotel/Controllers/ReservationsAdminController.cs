using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using yEtiHotel.Models;
using Microsoft.AspNet.Identity;
using System.Configuration;
using System.Collections.Specialized;

namespace yEtiHotel.Controllers
{
    [Authorize(Roles = ApplicationUser.ROLE_ADMIN)]
    public class ReservationsAdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ReservationsAdmin
        public ActionResult Index()
        {
            var reservation = db.Reservation.Include(r => r.Room).Include(r => r.User);
            return View(reservation.ToList());
        }

        // GET: ReservationsAdmin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservation.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoomId = new SelectList(db.Room, "Id", "Id", reservation.RoomId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", reservation.UserId);
            return View(reservation);
        }

        // POST: ReservationsAdmin/Admin/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Cost,StartDate,EndDate,RoomId,UserId")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reservation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RoomId = new SelectList(db.Room, "Id", "Id", reservation.RoomId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", reservation.UserId);
            return View(reservation);
        }

        // GET: ReservationsAdmin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservation.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }

            return View(reservation);
        }

        // POST: ReservationsAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reservation reservation = db.Reservation.Find(id);
            db.Reservation.Remove(reservation);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
