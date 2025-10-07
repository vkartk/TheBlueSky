export const refundStatuses = [
  'Pending',
  'Processed',
  'Failed',
] as const;

export type RefundStatus = (typeof refundStatuses)[number];

export type BookingCancellation = {
  bookingCancellationId: number;
  bookingId: number;
  cancellationDate: string;
  cancelledByUserId: string;
  refundAmount: number;
  refundStatus: RefundStatus;
  refundDate?: string | null;
  cancellationReason?: string | null;
  adminNotes?: string | null;
};

export type NewBookingCancellation = Omit<BookingCancellation, 'bookingCancellationId'>;