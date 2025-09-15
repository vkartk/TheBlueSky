using TheBlueSky.Flights.Models;
using TheBlueSky.Flights.Repositories;

namespace TheBlueSky.Flights.Services
{
    public class AirportService : IAirportService
    {
        private readonly IAirportRepository _airportRepository;

        public AirportService(IAirportRepository airportRepository)
        {
            _airportRepository = airportRepository;
        }

        public async Task<IEnumerable<Airport>> GetAllAirportsAsync()
        {
            return await _airportRepository.GetAllAirportsAsync();
        }

        public async Task<Airport?> GetAirportByIdAsync(int id)
        {
            return await _airportRepository.GetAirportByIdAsync(id);
        }

        public async Task<Airport> CreateAirportAsync(Airport airport)
        {
            await _airportRepository.AddAirportAsync(airport);
            return airport;
        }

        public async Task<bool> UpdateAirportAsync(int id, Airport airport)
        {
            if (id != airport.AirportId)
            {
                return false;
            }

            var exists = await _airportRepository.AirportExists(id);
            if (!exists)
            {
                return false;
            }

            await _airportRepository.UpdateAirportAsync(airport);
            return true;
        }

        public async Task<bool> DeleteAirportAsync(int id)
        {
            var exists = await _airportRepository.AirportExists(id);
            if (!exists)
            {
                return false;
            }

            await _airportRepository.DeleteAirportAsync(id);
            return true;
        }

    }
}
