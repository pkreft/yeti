using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Collections.Generic;
using System;

namespace yEtiHotel.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public virtual ICollection<Reservation> Reservations { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Room> Room { get; set; }

        public DbSet<Reservation> Reservation { get; set; }
    }

    public class ApplicationDbInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            List<Room> rooms = new List<Room>
            {
                new Room{Id = 1, Number = 10, Bedrooms = 0, Area = 30, Cost = 220, Exposure = Exposure.Eastern},
                new Room{Id = 2, Number = 11, Bedrooms = 0, Area = 28, Cost = 180, Exposure = Exposure.Eastern},
                new Room{Id = 3, Number = 12, Bedrooms = 0, Area = 41, Cost = 260, Exposure = Exposure.Northern},
                new Room{Id = 4, Number = 13, Bedrooms = 1, Area = 30, Cost = 290, Exposure = Exposure.Northern},
                new Room{Id = 5, Number = 14, Bedrooms = 1, Area = 48, Cost = 400, Exposure = Exposure.Western},
                new Room{Id = 6, Number = 15, Bedrooms = 1, Area = 50, Cost = 400, Exposure = Exposure.Southern},
                new Room{Id = 7, Number = 16, Bedrooms = 1, Area = 48, Cost = 380, Exposure = Exposure.Eastern},
                new Room{Id = 8, Number = 17, Bedrooms = 1, Area = 43, Cost = 320, Exposure = Exposure.Western},
                new Room{Id = 9, Number = 20, Bedrooms = 0, Area = 30, Cost = 230, Exposure = Exposure.Eastern},
                new Room{Id = 10, Number = 21, Bedrooms = 0, Area = 28, Cost = 190, Exposure = Exposure.Northern},
                new Room{Id = 11, Number = 21, Bedrooms = 1, Area = 41, Cost = 270, Exposure = Exposure.Northern},
                new Room{Id = 12, Number = 23, Bedrooms = 1, Area = 30, Cost = 300, Exposure = Exposure.Western},
                new Room{Id = 13, Number = 24, Bedrooms = 1, Area = 48, Cost = 410, Exposure = Exposure.Western},
                new Room{Id = 14, Number = 25, Bedrooms = 2, Area = 50, Cost = 410, Exposure = Exposure.Southern},
                new Room{Id = 15, Number = 26, Bedrooms = 1, Area = 48, Cost = 390, Exposure = Exposure.Southern},
                new Room{Id = 16, Number = 27, Bedrooms = 2, Area = 43, Cost = 330, Exposure = Exposure.Southern},
                new Room{Id = 17, Number = 30, Bedrooms = 2, Area = 44, Cost = 450, Exposure = Exposure.Eastern},
                new Room{Id = 18, Number = 31, Bedrooms = 2, Area = 52, Cost = 510, Exposure = Exposure.Eastern},
                new Room{Id = 19, Number = 32, Bedrooms = 2, Area = 50, Cost = 480, Exposure = Exposure.Northern},
                new Room{Id = 20, Number = 33, Bedrooms = 2, Area = 62, Cost = 600, Exposure = Exposure.Northern},
                new Room{Id = 21, Number = 34, Bedrooms = 2, Area = 58, Cost = 560, Exposure = Exposure.Western},
                new Room{Id = 22, Number = 35, Bedrooms = 2, Area = 44, Cost = 420, Exposure = Exposure.Southern},
                new Room{Id = 23, Number = 40, Bedrooms = 2, Area = 44, Cost = 460, Exposure = Exposure.Eastern},
                new Room{Id = 24, Number = 41, Bedrooms = 2, Area = 52, Cost = 530, Exposure = Exposure.Northern},
                new Room{Id = 25, Number = 42, Bedrooms = 2, Area = 50, Cost = 500, Exposure = Exposure.Western},
                new Room{Id = 26, Number = 43, Bedrooms = 2, Area = 52, Cost = 620, Exposure = Exposure.Southern},
                new Room{Id = 27, Number = 44, Bedrooms = 2, Area = 62, Cost = 580, Exposure = Exposure.Eastern},
                new Room{Id = 28, Number = 45, Bedrooms = 3, Area = 58, Cost = 440, Exposure = Exposure.Northern},
                new Room{Id = 29, Number = 50, Bedrooms = 2, Area = 80, Cost = 990, Exposure = Exposure.Northern},
                new Room{Id = 31, Number = 51, Bedrooms = 4, Area = 130, Cost = 1800, Exposure = Exposure.Eastern},
                new Room{Id = 32, Number = 52, Bedrooms = 3, Area = 108, Cost = 1320, Exposure = Exposure.Eastern},
            };
            rooms.ForEach(s => context.Room.Add(s));
            context.SaveChanges();

            List<ApplicationUser> users = new List<ApplicationUser>();
            UserManager<ApplicationUser> manager = new ApplicationUserManager(new UserStore<ApplicationUser>());
            PasswordHasher hasher = new PasswordHasher();
            string[] names = {"adrian", "marcin", "kuba", "piotrek", "arek", "marcel"};

            int userId = 1;
            foreach(string name in names)
            {
                users.Add(new ApplicationUser { Id = userId.ToString(), PasswordHash = hasher.HashPassword(name), Email = name + "@mail.pl", UserName = name + "@mail.pl", SecurityStamp = Guid.NewGuid().ToString() });
                userId++;
            }

            users.ForEach(s => context.Users.Add(s));
            context.SaveChanges();

            List<Reservation> reservations = new List<Reservation>();

            int roomId = 1;
            Random random = new Random();
            for (int i = 0; i < 2; i++)
            { 
                foreach (ApplicationUser user in users)
                {
                    Room room = rooms.Find(r => r.Id == roomId);
                    DateTime startTime = DateTime.Today.AddDays((double)random.Next(3, 40));
                    int reservationDuration = random.Next(1, 10);
                    DateTime endTime = new DateTime(startTime.Year, startTime.Month, startTime.Day).AddDays((double)reservationDuration);

                    reservations.Add(
                        new Reservation { Cost = room.Cost * reservationDuration, RoomId = room.Id, UserId = user.Id, StartDate = startTime, EndDate = endTime }
                    );
                    roomId += 2;
                }
            }
            reservations.ForEach(s => context.Reservation.Add(s));
            context.SaveChanges();
        }
    }
}
