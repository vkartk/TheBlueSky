import { useEffect } from 'react';
import { Loader2 } from 'lucide-react';
import { useAppDispatch, useAppSelector } from '@/store';
import { selectAllCancellations, selectCancellationsError, selectCancellationsLoading } from '@/features/bookingCancellation/bookingCancellationSlice';
import { fetchAllCancellations } from '@/features/bookingCancellation/bookingCancellationThunks';

import { BookingCancellationsTable } from '@/components/pages/dashboard/bookingCancellations/BookingCancellationsTable';


const  BookingCancellationsPage = () => {
  const dispatch = useAppDispatch();
  const user = useAppSelector(state => state.auth.user);
  const cancellations = useAppSelector(selectAllCancellations);
  const status = useAppSelector(selectCancellationsLoading);
  const error = useAppSelector(selectCancellationsError);

  const isAdmin = user?.roles.includes('Admin') ?? false;

  useEffect(() => {
    dispatch(fetchAllCancellations());
  }, [dispatch]);

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
        <h1 className="text-3xl font-bold tracking-tight">Booking Cancellations</h1>
        <p className="text-muted-foreground">
          A log of all processed booking cancellations.
        </p>
      </header>
      <main>
        <BookingCancellationsTable cancellations={cancellations} isAdmin={isAdmin} />
      </main>
    </div>
  );
}
export default BookingCancellationsPage;