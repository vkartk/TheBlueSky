import { useState } from 'react';
import { Link } from 'react-router';
import { MoreHorizontal, Ban } from 'lucide-react';
import { Badge } from '@/components/ui/badge';
import { Button } from '@/components/ui/button';
import {
    DropdownMenu,
    DropdownMenuContent,
    DropdownMenuItem,
    DropdownMenuLabel,
    DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu';
import {
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableHeader,
    TableRow,
} from '@/components/ui/table';

import type { Booking, BookingStatus, PaymentStatus } from '@/types/booking';
import { bookingStatuses, paymentStatuses } from '@/types/booking';
import { getBookingStatusVariant, getPaymentStatusVariant } from '@/utils/booking';
import { BookingCancellationForm } from './BookingCancellationForm';
import type { BookingCancellationFormData } from '@/pages/dashboard/bookings';

interface BookingsTableProps {
    bookings: Booking[];
    currentUserRole: 'Admin' | 'User';
    onUpdate: (booking: Booking) => void;
    onCancel: (bookingId: number, details: BookingCancellationFormData) => void;
}

export const BookingsTable = ({
    bookings,
    currentUserRole,
    onUpdate,
    onCancel,
}: BookingsTableProps) => {
    const [isCancelDialogOpen, setIsCancelDialogOpen] = useState(false);
    const [bookingToCancel, setBookingToCancel] = useState<Booking | null>(null);

    const handleOpenCancelDialog = (booking: Booking) => {
        setBookingToCancel(booking);
        setIsCancelDialogOpen(true);
    };

    const handleCloseCancelDialog = () => {
        setIsCancelDialogOpen(false);
        setBookingToCancel(null);
    };

    const handleCancellationSubmit = (formData: BookingCancellationFormData) => {
        if (!bookingToCancel) return;
        onCancel(bookingToCancel.bookingId, formData);
    };

    return (
        <>
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
                                        <Link to={`/booking?flightId=${booking.flightId}`} className="font-medium text-primary underline-offset-4 hover:underline">
                                            {booking.flightId}
                                        </Link>
                                    </TableCell>
                                    <TableCell>{new Date(booking.bookingDate).toLocaleDateString()}</TableCell>
                                    <TableCell>{booking.totalAmount.toLocaleString('en-IN', { style: 'currency', currency: 'INR' })}</TableCell>
                                    <TableCell>
                                        <DropdownMenu>

                                            <DropdownMenuTrigger asChild>
                                                <Badge variant={getBookingStatusVariant(booking.bookingStatus)} className="cursor-pointer">
                                                    {booking.bookingStatus}
                                                </Badge>
                                            </DropdownMenuTrigger>

                                            <DropdownMenuContent align="end">

                                                {bookingStatuses.filter(bs => bs != "Cancelled").map((status: BookingStatus) => (
                                                    <DropdownMenuItem key={status} onClick={() => onUpdate({ ...booking, bookingStatus: status })}>
                                                        {status}
                                                    </DropdownMenuItem>
                                                ))}

                                            </DropdownMenuContent>
                                        </DropdownMenu>
                                    </TableCell>
                                    <TableCell>
                                        <DropdownMenu>

                                            <DropdownMenuTrigger asChild>
                                                <Badge variant={getPaymentStatusVariant(booking.paymentStatus)} className="cursor-pointer">
                                                    {booking.paymentStatus}
                                                </Badge>
                                            </DropdownMenuTrigger>

                                            <DropdownMenuContent align="end">
                                                {paymentStatuses.map((status: PaymentStatus) => (
                                                    <DropdownMenuItem key={status} onClick={() => onUpdate({ ...booking, paymentStatus: status })}>
                                                        {status}
                                                    </DropdownMenuItem>
                                                ))}
                                            </DropdownMenuContent>

                                        </DropdownMenu>
                                    </TableCell>
                                    <TableCell className="text-right">
                                        <DropdownMenu>
                                            <DropdownMenuTrigger asChild><Button variant="ghost" className="h-8 w-8 p-0"><span className="sr-only">Open menu</span><MoreHorizontal className="h-4 w-4" /></Button></DropdownMenuTrigger>
                                            <DropdownMenuContent align="end">
                                                <DropdownMenuLabel>Actions</DropdownMenuLabel>

                                                <DropdownMenuItem onClick={() => handleOpenCancelDialog(booking)}>
                                                    <Ban className="mr-2 h-4 w-4" />
                                                    <span>Cancel Booking</span>
                                                </DropdownMenuItem>

                                            </DropdownMenuContent>
                                        </DropdownMenu>
                                    </TableCell>
                                </TableRow>
                            ))
                        ) : (
                            <TableRow><TableCell colSpan={7} className="h-24 text-center">No bookings found.</TableCell></TableRow>
                        )}
                    </TableBody>
                </Table>
            </div>

            <BookingCancellationForm
                isOpen={isCancelDialogOpen}
                onClose={handleCloseCancelDialog}
                onSubmit={handleCancellationSubmit}
                booking={bookingToCancel}
                currentUserRole={currentUserRole}
            />
        </>
    );
};