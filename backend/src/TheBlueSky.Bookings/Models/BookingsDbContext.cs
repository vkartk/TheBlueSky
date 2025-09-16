using Microsoft.EntityFrameworkCore;

namespace TheBlueSky.Bookings.Models
{
    public class BookingsDbContext : DbContext
    {
        public BookingsDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Booking> Bookings {  get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Passenger> Passengers { get; set; }
        public DbSet<BookingPassenger> BookingPassengers { get; set; }
        public DbSet<BookingCancellation> BookingCancellations { get; set; }
        public DbSet<BookingAudit> BookingAudits { get; set; }
        public DbSet<MealPreference> MealPreferences { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Payment>()
            .HasOne(p => p.Booking)
            .WithOne(b => b.Payment)
            .HasForeignKey<Payment>(p => p.BookingId)
            .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<BookingCancellation>()
            .HasOne(c => c.Booking)
            .WithOne(b => b.Cancellation)
            .HasForeignKey<BookingCancellation>(c => c.BookingId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BookingPassenger>()
            .HasOne(bp => bp.Booking)
            .WithMany(b => b.Passengers)
            .HasForeignKey(bp => bp.BookingId)
            .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<BookingPassenger>()
            .HasOne(bp => bp.MealPreference)
            .WithMany(mp => mp.BookingPassengers)
            .HasForeignKey(bp => bp.MealPreferenceId)
            .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<BookingAudit>()
            .HasOne(a => a.Booking)
            .WithMany(b => b.Audits)
            .HasForeignKey(a => a.BookingId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BookingPassenger>()
            .HasOne(bp => bp.Passenger)
            .WithMany(p => p.Bookings)
            .HasForeignKey(bp => bp.PassengerId)
            .OnDelete(DeleteBehavior.Restrict);

        }

    }
}
