using TheBlueSky.Bookings.DTOs.Requests.Flight;
using TheBlueSky.Bookings.Services.Interfaces;

namespace TheBlueSky.Bookings.Services
{
    public class FlightSeatStatusService : IFlightSeatStatusService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<FlightSeatStatusService> _logger;

        public FlightSeatStatusService(IHttpClientFactory httpClientFactory, ILogger<FlightSeatStatusService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task UpdateSeatStatusAsync(IEnumerable<int> seatStatusIds, string newStatus)
        {
            var httpClient = _httpClientFactory.CreateClient("FlightsService");

            foreach (var seatId in seatStatusIds)
            {
                var requestPayload = new UpdateSeatStatusRequest
                {
                    FlightSeatStatusId = seatId,
                    SeatStatus = newStatus
                };


                var response = await httpClient.PutAsJsonAsync("api/FlightSeatStatus", requestPayload);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError(
                        "Failed to update seat status for ID {SeatId}. Status: {StatusCode}. Response: {Error}",
                        seatId, response.StatusCode, errorContent);

                    throw new ApplicationException($"Failed to update seat status for ID {seatId}.");
                }

                _logger.LogInformation("Successfully updated seat status for ID {SeatId} to {Status}", seatId, newStatus);
            }
        }

    }
}
