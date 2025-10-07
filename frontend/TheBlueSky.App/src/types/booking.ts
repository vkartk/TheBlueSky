export const bookingStatuses = [
  'Confirmed',
  'Pending',
  'Cancelled',
] as const;

export type BookingStatus = (typeof bookingStatuses)[number];

export const paymentStatuses = [
  'Pending',
  'Paid',
  'Failed',
  'Refunded',
] as const;

export type PaymentStatus = (typeof paymentStatuses)[number];

export type Booking = {
  bookingId: number;
  userId: string;
  flightId: number;
  bookingDate: string;
  numberOfPassengers: number;
  subtotalAmount: number;
  taxAmount: number;
  totalAmount: number;
  bookingStatus: BookingStatus;
  paymentStatus: PaymentStatus;
  lastUpdated: string;
};

export type PassengerSeatSelection = {
  passengerId: number;
  flightSeatStatusId: number;
  ticketNumber: string;
  ticketPrice: number;
};

export type CreateBookingRequest = {
  flightId: number;
  userId: string;
  subtotal: number;
  tax: number;
  passengerSeatSelections: PassengerSeatSelection[];
};
