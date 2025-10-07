import { Link } from 'react-router';
import { Badge } from '@/components/ui/badge';
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from '@/components/ui/table';
import type { BookingCancellation } from '@/types/bookingCancellation';
import { getRefundStatusVariant } from '@/utils/booking';

interface BookingCancellationsTableProps {
  cancellations: BookingCancellation[];
  isAdmin: boolean;
}

export const BookingCancellationsTable = ({ cancellations, isAdmin }: BookingCancellationsTableProps) => {
  return (
    <div className="rounded-md border">
      <Table>
        <TableHeader>
          <TableRow>
            <TableHead>Cancellation ID</TableHead>
            <TableHead>Booking ID</TableHead>
            <TableHead>Cancellation Date</TableHead>
            <TableHead>Refund Amount</TableHead>
            <TableHead>Refund Status</TableHead>
            <TableHead>Reason</TableHead>
            {isAdmin && <TableHead>Admin Notes</TableHead>}
          </TableRow>
        </TableHeader>
        <TableBody>
          {cancellations.length > 0 ? (
            cancellations.map((cancellation) => (
              <TableRow key={cancellation.bookingCancellationId}>
                <TableCell className="font-medium">{cancellation.bookingCancellationId}</TableCell>
                <TableCell>
                  <Link
                    to={`/dashboard/bookings?bookingId=${cancellation.bookingId}`}
                    className="font-medium text-primary underline-offset-4 hover:underline"
                  >
                    {cancellation.bookingId}
                  </Link>
                </TableCell>
                <TableCell>{new Date(cancellation.cancellationDate).toLocaleString()}</TableCell>
                <TableCell>
                  {cancellation.refundAmount.toLocaleString('en-IN', {
                    style: 'currency',
                    currency: 'INR',
                  })}
                </TableCell>
                <TableCell>
                  <Badge variant={getRefundStatusVariant(cancellation.refundStatus)}>
                    {cancellation.refundStatus}
                  </Badge>
                </TableCell>
                <TableCell className="max-w-xs truncate">{cancellation.cancellationReason || 'N/A'}</TableCell>
                {isAdmin && <TableCell className="max-w-xs truncate">{cancellation.adminNotes || 'N/A'}</TableCell>}
              </TableRow>
            ))
          ) : (
            <TableRow>
              <TableCell colSpan={isAdmin ? 7 : 6} className="h-24 text-center">
                No cancellation records found.
              </TableCell>
            </TableRow>
          )}
        </TableBody>
      </Table>
    </div>
  );
};