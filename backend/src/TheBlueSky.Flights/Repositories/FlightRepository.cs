using Microsoft.EntityFrameworkCore;
using TheBlueSky.Flights.Models;
using TheBlueSky.Flights.Repositories.Interfaces;

namespace TheBlueSky.Flights.Repositories
{
    public class FlightRepository : IFlightRepository
    {
        private readonly FlightsDbContext _context;

        public FlightRepository(FlightsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Flight>> GetAllFlightsAsync()
        {
            return await _context.Flights.ToListAsync();
        }

        public async Task<Flight?> GetFlightByIdAsync(int id)
        {
            return await _context.Flights.FindAsync(id);
        }

        public async Task<Flight> AddFlightAsync(Flight flight)
        {
            _context.Flights.Add(flight);
            await _context.SaveChangesAsync();
            return flight;
        }

        public async Task<bool> UpdateFlightAsync(Flight flight)
        {
            _context.Entry(flight).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ExistsAsync(flight.FlightId))
                {
                    return false;
                }
                throw;
            }
        }

        public async Task<bool> DeleteFlightAsync(int id)
        {
            var flight = await _context.Flights.FindAsync(id);
            if (flight == null)
            {
                return false;
            }

            _context.Flights.Remove(flight);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Flights.AnyAsync(e => e.FlightId == id);
        }

    }
}
