using Microsoft.EntityFrameworkCore;
using TrainTicketsServer.Models.Entities;

namespace TrainTicketsServer.Models
{
    public class ApplicationContext : DbContext
    {
        //public DbSet<Player> Player { get; set; }
        //public DbSet<TrackRecord> TrackRecords { get; set; }

        public DbSet<User> usersDbSet { get; set; }
        public DbSet<TrainTicketsServer.Models.Entities.Route> routesDbSet { get; set; }
        public DbSet<Trip> tripsDbSet { get; set; }
        public DbSet<Ticket> ticketsDbSet { get; set; }
        public DbSet<Seat> seatsDbSet { get; set; }

        public ApplicationContext()
        {

        }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
