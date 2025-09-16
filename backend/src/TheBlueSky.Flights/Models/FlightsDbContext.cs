using Microsoft.EntityFrameworkCore;

namespace TheBlueSky.Flights.Models
{
    public class FlightsDbContext : DbContext
    {
        public FlightsDbContext(DbContextOptions<FlightsDbContext> options) : base(options) { }

        public DbSet<Country> Countries { get; set; }
        public DbSet<Airport> Airports { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<Aircraft> Aircrafts { get; set; }
        public DbSet<SeatClass> SeatClasses { get; set; }
        public DbSet<AircraftSeat> AircraftSeats { get; set; }
        public DbSet<FlightSchedule> FlightSchedules { get; set; }
        public DbSet<ScheduleDay> ScheduleDays { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<FlightSeatStatus> FlightSeatStatuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Airport <-> Route (Origin)
            modelBuilder.Entity<Route>()
                .HasOne(r => r.OriginAirport)
                .WithMany(a => a.OriginRoutes)
                .HasForeignKey(r => r.OriginAirportId)
                .OnDelete(DeleteBehavior.Restrict);

            // Airport <-> Route (Destination)
            modelBuilder.Entity<Route>()
                .HasOne(r => r.DestinationAirport)
                .WithMany(a => a.DestinationRoutes)
                .HasForeignKey(r => r.DestinationAirportId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FlightSeatStatus>(s =>
            {
                s.HasKey(x => x.FlightSeatStatusId);

                s.HasOne(x => x.Flight)
                 .WithMany(f => f.SeatStatuses)
                 .HasForeignKey(x => x.FlightId)
                 .OnDelete(DeleteBehavior.Restrict);

                s.HasOne(x => x.AircraftSeat)
                 .WithMany(s => s.FlightSeatStatuses)
                 .HasForeignKey(x => x.AircraftSeatId)
                 .OnDelete(DeleteBehavior.Restrict);
            });
        }


    }
}
