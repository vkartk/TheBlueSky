import { useEffect } from 'react';
import { Loader2 } from 'lucide-react';

import { useAppDispatch, useAppSelector } from '@/store';

import { selectAllBookings, selectBookingsError, selectBookingsLoading } from '@/features/bookings/bookingSlice';
import { fetchBookings, updateBooking } from '@/features/bookings/bookingThunks';
import type { Booking } from '@/types/booking';
import { toast } from 'sonner';
import { createBookingCancellation } from '@/features/bookingCancellation/bookingCancellationThunks';

import { BookingsTable } from '@/components/pages/dashboard/bookings/BookingsTable';
import type { NewBookingCancellation } from '@/types/bookingCancellation';

export type BookingCancellationFormData = Omit<NewBookingCancellation, 'bookingId' | 'cancelledByUserId' | 'cancellationDate' | 'refundStatus' | 'refundDate'>;

export function BookingsPage() {
    const dispatch = useAppDispatch();

    const user = useAppSelector(state => state.auth.user);
    const bookings = useAppSelector(selectAllBookings);
    const status = useAppSelector(selectBookingsLoading);
    const error = useAppSelector(selectBookingsError);

    useEffect(() => {
        dispatch(fetchBookings());
    }, [dispatch]);


    const handleUpdateBooking = async(bookingToUpdate: Booking) => {
        const toastId = toast.loading('Updating booking...');

        try {
            await dispatch(updateBooking(bookingToUpdate)).unwrap();
            toast.success('Booking updated', {id: toastId});

            setTimeout(() => {
                dispatch(fetchBookings());
            }, 1000);
        } catch {
            toast.error('Failed to update booking', { id: toastId });
        }

    };

    const handleCancelBooking = (bookingId: number, formData: BookingCancellationFormData) => {
        if (!user) {
            console.error("User not found. Cannot process cancellation.");
            toast.error("Cannot process cancellation. Please try again later.")
            return;
        }

        const originalBooking = bookings.find(b => b.bookingId === bookingId);
        if (!originalBooking) {
            console.error("Original booking not found.");
            return;
        }

        const updatedBookingData = { ...originalBooking, bookingStatus: 'Cancelled' as const };
        dispatch(updateBooking(updatedBookingData));

        const newCancellationData: NewBookingCancellation = {
            bookingId: bookingId,
            cancelledByUserId: user.userId,
            cancellationDate: new Date().toISOString(),
            refundStatus: 'Pending',
            refundDate: null,
            ...formData,
        };

        dispatch(createBookingCancellation(newCancellationData));
    };

    if (status === 'pending') {
        return (
            <div className="flex h-96 items-center justify-center">
                <Loader2 className="h-8 w-8 animate-spin text-muted-foreground" />
            </div>
        );
    }

    if (status === 'failed') {
        return (
            <div className="flex h-96 items-center justify-center">
                <p className="text-destructive">Error: {error}</p>
            </div>
        );
    }

    return (
        <div className="container mx-auto p-8">
            <header className="mb-6">
                <h1 className="text-3xl font-bold tracking-tight">Manage Bookings</h1>
                <p className="text-muted-foreground">
                    View and manage all customer bookings.
                </p>
            </header>
            <main>
                <BookingsTable
                    bookings={bookings}
                    currentUserRole={user?.roles.includes('Admin') ? 'Admin' : 'User'}
                    onUpdate={handleUpdateBooking}
                    onCancel={handleCancelBooking}
                />
            </main>
        </div>
    );

}