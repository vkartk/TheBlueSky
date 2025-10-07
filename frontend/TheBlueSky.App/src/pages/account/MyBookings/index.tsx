import { useEffect, useState } from 'react';
import { Loader2 } from 'lucide-react';
import { toast } from 'sonner';

import { useAppDispatch, useAppSelector } from '@/store';
import { selectAllBookings, selectBookingsError, selectBookingsLoading } from '@/features/bookings/bookingSlice';
import { fetchBookingsByUser, updateBooking } from '@/features/bookings/bookingThunks';
import { createBookingCancellation } from '@/features/bookingCancellation/bookingCancellationThunks';

import { BookingsTable } from '@/components/pages/account/bookings/BookingsTable';
import { BookingCancellationDialog, type CancellationFormData } from '@/components/pages/account/bookings/BookingCancellationDialog';

import type { Booking } from '@/types/booking';
import type { NewBookingCancellation } from '@/types/bookingCancellation'


const MyBookingsPage = () => {
    const dispatch = useAppDispatch();

    const user = useAppSelector(state => state.auth.user);

    const bookings = useAppSelector(selectAllBookings);
    const status = useAppSelector(selectBookingsLoading);
    const error = useAppSelector(selectBookingsError);

    const [isCancelDialogOpen, setIsCancelDialogOpen] = useState(false);
    const [selectedBooking, setSelectedBooking] = useState<Booking | null>(null);

    useEffect(() => {
        if (user?.userId) {
            dispatch(fetchBookingsByUser(user.userId));
        }
    }, [dispatch, user]);

    const handleOpenCancelDialog = (booking: Booking) => {
        setSelectedBooking(booking);
        setIsCancelDialogOpen(true);
    };

    const handleCloseCancelDialog = () => {
        setSelectedBooking(null);
        setIsCancelDialogOpen(false);
    };

    const handleConfirmCancellation = async (formData: CancellationFormData) => {

        if (!selectedBooking || !user) return;

        const cancellationRequest: NewBookingCancellation = {
            bookingId: formData.bookingId,
            refundAmount: formData.refundAmount,
            cancellationReason: formData.cancellationReason || 'Not provided by user.',
            adminNotes: '',
            refundStatus: 'Pending',
            cancellationDate: new Date().toISOString(),
            cancelledByUserId: user.userId
        };

        const toastId = toast.loading('Updating booking...');
        try {
            const updatedBookingData = { ...selectedBooking, bookingStatus: 'Cancelled' as const };

            await dispatch(updateBooking(updatedBookingData)).unwrap();

            await dispatch(createBookingCancellation(cancellationRequest)).unwrap();
            toast.success('Booking Cancelled Successfully.', { id: toastId });

            dispatch(fetchBookingsByUser(user.userId));
        } catch (err) {
            console.error('Failed to cancel booking:', err);
            toast.error("Failed to cancel booking, Please try again latr.", { id: toastId })
        } finally {
            handleCloseCancelDialog();
        }
    };

    const renderContent = () => {
        if (status === 'pending') {
            return (
                <div className="flex justify-center items-center h-64">
                    <Loader2 className="h-8 w-8 animate-spin text-muted-foreground" />
                </div>
            );
        }
        if (status === 'failed') {
            return <p className="text-center text-destructive">{error || 'An unknown error occurred.'}</p>;
        }
        return <BookingsTable bookings={bookings} onCancel={handleOpenCancelDialog} />;
    };

    return (
        <div className="container mx-auto p-8">
            <header className="mb-6">
                <h1 className="text-3xl font-bold tracking-tight">My Bookings</h1>
                <p className="text-muted-foreground">View your booking history and manage upcoming trips.</p>
            </header>

            <main>{renderContent()}</main>

            <BookingCancellationDialog
                isOpen={isCancelDialogOpen}
                onClose={handleCloseCancelDialog}
                onSubmit={handleConfirmCancellation}
                booking={selectedBooking}
                isLoading={status === 'pending'}
            />
        </div>
    );
};

export default MyBookingsPage;