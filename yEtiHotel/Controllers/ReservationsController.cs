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

namespace yEtiHotel.Controllers
{
    [Authorize]
    public class ReservationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Reservations
        public ActionResult Index()
        {
            string UserId = User.Identity.GetUserId();
            var reservation = db.Reservation.Include(r => r.Room).Include(r => r.User).Where(r => r.UserId == UserId);
            return View(reservation.ToList());
        }

        // GET: Reservations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string UserId = User.Identity.GetUserId();
            Reservation reservation = db.Reservation.Where(r => r.Id == id && r.UserId == UserId).FirstOrDefault();
            if (reservation == null)
            {
                return HttpNotFound();
            }
            if (isReservationTooNear(reservation))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.RoomId = new SelectList(db.Room, "Id", "Id", reservation.RoomId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", reservation.UserId);
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StartDate,EndDate")] Reservation reservation)
        {
            string UserId = User.Identity.GetUserId();
            Reservation reservationToUpdate = db.Reservation.Where(r => r.Id == reservation.Id && r.UserId == UserId).FirstOrDefault();
            if (reservationToUpdate == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (isReservationTooNear(reservationToUpdate))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            reservationToUpdate.StartDate = reservation.StartDate;
            reservationToUpdate.EndDate = reservation.EndDate;
            db.Entry(reservationToUpdate).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Reservations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string UserId = User.Identity.GetUserId();
            Reservation reservation = db.Reservation.Where(r => r.Id == id && r.UserId == UserId).FirstOrDefault();
            if (reservation == null)
            {
                return HttpNotFound();
            }
            if (isReservationTooNear(reservation))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            string UserId = User.Identity.GetUserId();
            Reservation reservation = db.Reservation.Where(r => r.Id == id && r.UserId == UserId).FirstOrDefault();
            if (isReservationTooNear(reservation))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
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

        public static bool isReservationTooNear(Reservation reservation)
        {
            string allowEditWithin = ConfigurationManager.AppSettings["allowEditReservationWithin"];

            if ((reservation.StartDate - DateTime.Today).Days < Int32.Parse(allowEditWithin))
            {
                return true;
            } else
            {
                return false;
            }
        }
    }
}
