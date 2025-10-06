using Microsoft.EntityFrameworkCore;
using TheBlueSky.Flights.Enums;

namespace TheBlueSky.Flights.Models
{
    public class FlightsDbContext : DbContext
    {
        public FlightsDbContext(DbContextOptions<FlightsDbContext> options) : base(options) { }

        public DbSet<Country> Countries { get; set; }
        public DbSet<Airport> Airports { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<Aircraft> Aircrafts { get; set; }
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

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await UpdateAircraftSeatCounts();
            return await base.SaveChangesAsync(cancellationToken);
        }


        private async Task UpdateAircraftSeatCounts()
        {
            var changedSeatEntries = ChangeTracker.Entries<AircraftSeat>()
                .Where(e => e.State == EntityState.Added ||
                            e.State == EntityState.Deleted ||
                            e.State == EntityState.Modified);

            var affectedAircraftIds = changedSeatEntries
                .Select(e => e.Entity.AircraftId)
                .Distinct()
                .ToList();

            if (!affectedAircraftIds.Any())
            {
                return;
            }

            foreach (var aircraftId in affectedAircraftIds)
            {
                var aircraft = await Aircrafts.FindAsync(aircraftId);
                if (aircraft == null) continue;

                await Entry(aircraft).Collection(a => a.Seats).LoadAsync();

                var localSeatsForAircraft = AircraftSeats.Local
                    .Where(s => s.AircraftId == aircraftId);

                aircraft.EconomySeats = localSeatsForAircraft.Count(s => s.SeatClass == SeatClass.Economy);
                aircraft.BusinessSeats = localSeatsForAircraft.Count(s => s.SeatClass == SeatClass.Business);
                aircraft.FirstClassSeats = localSeatsForAircraft.Count(s => s.SeatClass == SeatClass.FirstClass);

                Entry(aircraft).State = EntityState.Modified;
            }
        }



    }
}
