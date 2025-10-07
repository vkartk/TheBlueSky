import type { BookingData } from "@/pages/public/BookingPage";

export const prepareBookingData = (bookingData: BookingData, userId: string) => {

    if (bookingData.flight == null) return;

    const flightId = bookingData.flight.flightId;
    const subtotal = bookingData.subtotal;
    const tax = bookingData.tax;
    const flightBaseFare = bookingData.flight.baseFare;

    const passengerSeatSelections = bookingData.seats.map(selection => {
        const aircraftSeat = selection.seat.aircraftSeat;

        const ticketPrice = flightBaseFare + aircraftSeat.additionalFare;

        return {
            passengerId: selection.passengerId,
            flightSeatStatusId: selection.seat.flightSeatStatusId,
            ticketNumber: aircraftSeat.seatNumber,
            ticketPrice: ticketPrice,
        };
    });

    return {
        flightId,
        subtotal,
        tax,
        passengerSeatSelections,
        userId,
    };
}