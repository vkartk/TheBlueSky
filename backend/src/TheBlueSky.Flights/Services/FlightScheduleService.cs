using AutoMapper;
using TheBlueSky.Flights.DTOs.Requests.FlightSchedule;
using TheBlueSky.Flights.DTOs.Responses.Flight;
using TheBlueSky.Flights.DTOs.Responses.FlightSchedule;
using TheBlueSky.Flights.DTOs.Responses.ScheduleDay;
using TheBlueSky.Flights.Enums;
using TheBlueSky.Flights.Models;
using TheBlueSky.Flights.Repositories;
using TheBlueSky.Flights.Repositories.Interfaces;

namespace TheBlueSky.Flights.Services
{
    public class FlightScheduleService : IFlightScheduleService
    {
        private readonly IFlightScheduleRepository _flightScheduleRepository;
        private readonly IFlightRepository _flightRepository;
        private readonly IScheduleDayRepository _scheduleDayRepository;
        private readonly IMapper _mapper;

        public FlightScheduleService(IFlightScheduleRepository flightScheduleRepository,IFlightRepository flightRepository, IScheduleDayRepository scheduleDayRepository, IMapper mapper)
        {
            _flightScheduleRepository = flightScheduleRepository;
            _flightRepository = flightRepository;
            _scheduleDayRepository = scheduleDayRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<FlightScheduleResponse>> GetAllFlightSchedulesAsync()
        {
            var schedules = await _flightScheduleRepository.GetAllFlightSchedulesAsync();
            return _mapper.Map<IEnumerable<FlightScheduleResponse>>(schedules);
        }

        public async Task<FlightScheduleResponse?> GetFlightScheduleByIdAsync(int id)
        {
            var schedule = await _flightScheduleRepository.GetFlightScheduleByIdAsync(id);
            return _mapper.Map<FlightScheduleResponse>(schedule);
        }

        public async Task<FlightScheduleResponse> CreateFlightScheduleAsync(CreateFlightScheduleRequest request)
        {
            var schedule = _mapper.Map<FlightSchedule>(request);
            var createdSchedule = await _flightScheduleRepository.AddFlightScheduleAsync(schedule);
            return _mapper.Map<FlightScheduleResponse>(createdSchedule);
        }

        public async Task<bool> UpdateFlightScheduleAsync(UpdateFlightScheduleRequest request)
        {
            var existingSchedule = await _flightScheduleRepository.GetFlightScheduleByIdAsync(request.FlightScheduleId);
            if (existingSchedule == null)
            {
                return false;
            }

            _mapper.Map(request, existingSchedule);
            return await _flightScheduleRepository.UpdateFlightScheduleAsync(existingSchedule);
        }

        public async Task<bool> DeleteFlightScheduleAsync(int id)
        {
            return await _flightScheduleRepository.DeleteFlightScheduleAsync(id);
        }

        public async Task<IEnumerable<FlightResponse>> GetFlightsForScheduleAsync(int flightScheduleId)
        {
            var flights = await _flightRepository.GetFlightsByScheduleIdAsync(flightScheduleId);
            return _mapper.Map<IEnumerable<FlightResponse>>(flights);
        }

        public async Task<IEnumerable<ScheduleDayResponse>> UpdateScheduleDaysAsync(int flightScheduleId, IEnumerable<string> daysOfWeek)
        {
            var desiredDays = daysOfWeek.Select(Enum.Parse<DayOfWeek>).ToHashSet();
            var existingDays = await _scheduleDayRepository.GetByScheduleIdAsync(flightScheduleId);

            var daysToDelete = existingDays.Where(d => !desiredDays.Contains(d.DayOfWeek)).ToList();
            if (daysToDelete.Any())
            {
                await _scheduleDayRepository.RemoveRangeAsync(daysToDelete);
            }

            var existingDayEnums = existingDays.Select(d => d.DayOfWeek).ToHashSet();
            var daysToAdd = desiredDays.Where(d => !existingDayEnums.Contains(d))
                .Select(d => new ScheduleDay { FlightScheduleId = flightScheduleId, DayOfWeek = d, IsActive = true })
                .ToList();

            if (daysToAdd.Any())
            {
                await _scheduleDayRepository.AddRangeAsync(daysToAdd);
            }

            var finalDays = await _scheduleDayRepository.GetByScheduleIdAsync(flightScheduleId);
            return _mapper.Map<IEnumerable<ScheduleDayResponse>>(finalDays);
        }

        public async Task<int> GenerateFlightsAsync(int flightScheduleId, DateOnly startDate, DateOnly endDate)
        {
            var schedule = await _flightScheduleRepository.GetFlightScheduleByIdAsync(flightScheduleId);

            if (schedule?.ScheduleDays == null || !schedule.ScheduleDays.Any(d => d.IsActive))
            {
                return 0; // No active schedule days to generate flights for
            }

            var activeDays = schedule.ScheduleDays.Where(d => d.IsActive).Select(d => d.DayOfWeek).ToHashSet();
            var newFlights = new List<Flight>();

            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                if (activeDays.Contains(date.DayOfWeek))
                {
                    var departureDateTime = new DateTimeOffset(date.ToDateTime(schedule.DepartureTime));

                    // Assuming ArrivalTime is on the same day or next day
                    var arrivalDateTime = new DateTimeOffset(date.ToDateTime(schedule.ArrivalTime));
                    if (schedule.ArrivalTime < schedule.DepartureTime)
                    {
                        // Arrival is on the next day
                        arrivalDateTime = arrivalDateTime.AddDays(1);
                    }

                    var totalSeats = (schedule.Aircraft?.EconomySeats ?? 0) +
                                     (schedule.Aircraft?.BusinessSeats ?? 0) +
                                     (schedule.Aircraft?.FirstClassSeats ?? 0);

                    newFlights.Add(new Flight
                    {
                        FlightScheduleId = flightScheduleId,
                        FlightDate = date,
                        DepartureDateTime = departureDateTime,
                        ArrivalDateTime = arrivalDateTime,
                        FlightStatus = FlightStatus.Scheduled,
                        AvailableSeats = totalSeats
                    });
                }
            }

            if (newFlights.Any())
            {
                await _flightRepository.AddFlightsAsync(newFlights);
            }

            return newFlights.Count;
        }



    }
}
