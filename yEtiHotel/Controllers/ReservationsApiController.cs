﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using yEtiHotel.Models;
using System.Web.Security;
using Microsoft.AspNet.Identity;

namespace yEtiHotel.Controllers
{
    public class RoomReservation
    {
        public int roomId { get; set; }

        public List<string> dates { get; set; }
    }

    [RoutePrefix("api")]
    public class ReservationsApiController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Route("reservations")]
        public List<RoomReservation> GetReservations()
        {
            return GetRoomReservations();
        }

        [Route("reservations/cost")]
        [Authorize]
        public int PostReservationsCost(Reservation reservation)
        {
            if (!validateModel(reservation)) {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            return calculateCost(reservation);
        }

        [Route("reservations")]
        [Authorize]
        public List<RoomReservation> PostReservation(Reservation reservation)
        {
            if (!validateModel(reservation))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            reservation.Cost = calculateCost(reservation);
            reservation.UserId = User.Identity.GetUserId();

            db.Reservation.Add(reservation);
            db.SaveChanges();


            return GetRoomReservations();
        }

        private List<RoomReservation> GetRoomReservations()
        {
            List<RoomReservation> roomReservations = new List<RoomReservation>();
            var reservations = db.Reservation
                .Where(r => r.EndDate > DateTime.Today)
                .Select(r => (Reservation)r)
                .ToList();
            foreach (Reservation reservation in reservations)
            {
                DateTime date = reservation.StartDate;
                while (date <= reservation.EndDate)
                {
                    var roomReservation = roomReservations.Find(r => r.roomId == reservation.RoomId);
                    if (roomReservation != null)
                    {
                        roomReservation.dates.Add(date.ToShortDateString());
                    }
                    else
                    {
                        List<string> list = new List<string>();
                        list.Add(date.ToShortDateString());
                        roomReservations.Add(new RoomReservation { roomId = reservation.RoomId, dates = list });
                    }
                    date = date.AddDays((double)1);
                }
            }

            return roomReservations;
        }

        private bool validateModel(Reservation reservation)
        {
            Room room = db.Room.Where(r => r.Id == reservation.RoomId).First();

            return reservation.StartDate != null &&
                reservation.EndDate != null &&
                reservation.EndDate >= reservation.StartDate &&
                room != null;
        }

        private int calculateCost(Reservation reservation)
        {
            int diff = (reservation.EndDate - reservation.StartDate).Days + 1;
            Room room = db.Room.Where(r => r.Id == reservation.RoomId).First();

            return diff * room.Cost;
        }
    }
}