import { Link } from 'react-router';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from '@/components/ui/table';
import type { Booking } from '@/types/booking';
import { getBookingStatusVariant, getPaymentStatusVariant } from '@/utils/booking';

interface BookingsTableProps {
  bookings: Booking[];
  onCancel: (booking: Booking) => void;
}

export const BookingsTable = ({ bookings, onCancel }: BookingsTableProps) => {
  return (
    <div className="rounded-md border">
      <Table>
        <TableHeader>
          <TableRow>
            <TableHead>Booking ID</TableHead>
            <TableHead>Flight ID</TableHead>
            <TableHead>Booking Date</TableHead>
            <TableHead>Total Amount</TableHead>
            <TableHead>Booking Status</TableHead>
            <TableHead>Payment Status</TableHead>
            <TableHead className="text-right">Actions</TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          {bookings.length > 0 ? (
            bookings.map((booking) => (
              <TableRow key={booking.bookingId}>
                <TableCell className="font-medium">{booking.bookingId}</TableCell>
                <TableCell>
                  <Button variant="link" asChild className="p-0">
                    <Link to={`/booking?flightId=${booking.flightId}`}>
                      {booking.flightId}
                    </Link>
                  </Button>
                </TableCell>
                <TableCell>
                  {new Date(booking.bookingDate).toLocaleDateString()}
                </TableCell>
                <TableCell>
                  ${booking.totalAmount.toFixed(2)}
                </TableCell>
                <TableCell>
                  <Badge variant={getBookingStatusVariant(booking.bookingStatus)}>
                    {booking.bookingStatus}
                  </Badge>
                </TableCell>
                <TableCell>
                  <Badge variant={getPaymentStatusVariant(booking.paymentStatus)}>
                    {booking.paymentStatus}
                  </Badge>
                </TableCell>
                <TableCell className="text-right">
                  <div className="flex items-center justify-end gap-2">
                    <Link to={`/account/ticket/${booking.bookingId}/${booking.flightId}`}>
                      <Button variant="outline" size="sm">
                        View Ticket
                      </Button>
                    </Link>
                    <Button
                      variant="destructive"
                      size="sm"
                      onClick={() => onCancel(booking)}
                      disabled={booking.bookingStatus === 'Cancelled'}
                    >
                      Cancel
                    </Button>
                  </div>
                </TableCell>
              </TableRow>
            ))
          ) : (
            <TableRow>
              <TableCell colSpan={7} className="h-24 text-center">
                No bookings found.
              </TableCell>
            </TableRow>
          )}
        </TableBody>
      </Table>
    </div>
  );
};