import type { BookingData } from "@/pages/public/BookingPage";
import type { BookingStatus, PaymentStatus } from "@/types/booking";
import type { RefundStatus } from "@/types/bookingCancellation";

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


export const getBookingStatusVariant = (status: BookingStatus) => {
  switch (status) {
    case 'Confirmed':
      return 'default';
    case 'Pending':
      return 'secondary';
    case 'Cancelled':
      return 'destructive';
    default:
      return 'default';
  }
};

export const getPaymentStatusVariant = (status: PaymentStatus) => {
    switch (status) {
      case 'Paid':
        return 'default';
      case 'Pending':
        return 'secondary';
      case 'Failed':
        return 'destructive';
      case 'Refunded':
        return 'outline';
      default:
        return 'default';
    }
  };

  export const getRefundStatusVariant = (status: RefundStatus) => {
    switch (status) {
      case 'Processed':
        return 'default';
      case 'Pending':
        return 'secondary';
      case 'Failed':
        return 'destructive';
      default:
        return 'outline';
    }
  };
