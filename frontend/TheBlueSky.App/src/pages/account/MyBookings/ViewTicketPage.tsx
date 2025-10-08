import { useEffect, useState } from 'react';
import { useParams } from 'react-router';
import { Loader2, AlertTriangle } from 'lucide-react';

import { Button } from '@/components/ui/button';
import { TicketDetails } from '@/components/pages/Booking/viewTicket/TicketDetails';

import type { Booking } from '@/types/booking';
import type { GetFlight } from '@/types/flight';
import type { Passenger } from '@/types/passenger';
import { bookingService } from '@/services/bookings/bookingService';
import { flightService } from '@/services/flights/flightService';
import { passengerService } from '@/services/bookings/passengerService';
import { useAppSelector } from '@/store';

function ViewTicketPage() {

    const { bookingId, flightId } = useParams<{ bookingId: string; flightId: string }>();
    const user = useAppSelector(state => state.auth.user);

    const [booking, setBooking] = useState<Booking | null>(null);
    const [flight, setFlight] = useState<GetFlight | null>(null);
    const [allPassengers, setAllPassengers] = useState<Passenger[]>([]);
    
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const fetchAllTicketData = async () => {
            if (!bookingId || !flightId || !user) {
                setError('Required information is missing.');
                setIsLoading(false);
                return;
            }
            
            try {
                const [bookingRes, flightRes, passengersRes] = await Promise.all([
                    bookingService.getById(Number(bookingId)),
                    flightService.getById(Number(flightId)),
                    passengerService.getByUserId(user.userId)
                ]);

                setBooking(bookingRes);
                setFlight(flightRes);
                setAllPassengers(passengersRes);

            } catch (err) {
                console.error("Failed to fetch ticket data:", err);
                setError("Sorry, we couldn't retrieve the ticket. Please try again later.");
            } finally {
                setIsLoading(false);
            }
        };

        fetchAllTicketData();
    }, [bookingId, flightId, user]);

    if (isLoading) {
        return (
            <div className="flex min-h-screen items-center justify-center">
                <div className="flex flex-col items-center gap-4">
                    <Loader2 className="h-12 w-12 animate-spin text-primary" />
                    <p className="text-lg text-muted-foreground">Loading Your Ticket...</p>
                </div>
            </div>
        );
    }

    if (error) {
        return (
            <div className="flex min-h-screen items-center justify-center">
                <div className="flex flex-col items-center gap-4 rounded-lg border bg-card p-8 text-card-foreground shadow-sm">
                    <AlertTriangle className="h-12 w-12 text-destructive" />
                    <h2 className="text-2xl font-semibold">An Error Occurred</h2>
                    <p className="text-center text-muted-foreground">{error}</p>
                    <Button onClick={() => window.location.reload()}>Try Again</Button>
                </div>
            </div>
        );
    }

    if (!booking || !flight || !allPassengers.length) {
        return <div className="text-center py-10">Missing required ticket information.</div>;
    }

    return (
        <div className="container py-8">
            <TicketDetails
                booking={booking}
                flight={flight}
                allPassengers={allPassengers}
            />
        </div>
    );
}

export default ViewTicketPage;