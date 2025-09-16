namespace TheBlueSky.Flights.DTOs.Requests.SeatClass
{
    public record SeatClassDto(
        int SeatClassId,
        string ClassName,
        string? ClassDescription,
        int PriorityOrder
    );

}
