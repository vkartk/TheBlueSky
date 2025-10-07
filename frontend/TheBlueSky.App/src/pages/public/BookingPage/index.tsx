import { useState, useEffect } from 'react';
import { useSearchParams, useNavigate } from 'react-router';

import type { GetFlight } from '@/types/flight';
import type { Passenger } from '@/types/passenger';
import { flightService } from '@/services/flights/flightService';

import { PassengerSelectStep } from '@/components/pages/Booking/PassengerSelectStep';
import { FlightDetailsStep } from '@/components/pages/Booking/FlightDetailsStep';
import { Stepper } from '@/components/pages/Booking/Stepper';
import SeatSelectionStep from '@/components/pages/Booking/SeatSelection/SeatSelectionStep';
import type { FlightSeatStatus } from '@/types/flightSeatStatus';
import { ReviewAndPayStep } from '@/components/pages/Booking/ReviewAndPayStep';
import { ConfirmationStep } from '@/components/pages/Booking/ConfirmationStep';
import { prepareBookingData } from '@/utils/booking';
import { useAppSelector } from '@/store';
import { toast } from 'sonner';
import { createBooking } from '@/services/bookings/bookingService';
import type { Booking } from '@/types/booking';


const steps = [
    { id: 1, name: 'Flight Details' },
    { id: 2, name: 'Passengers' },
    { id: 3, name: 'Seat Selection' },
    { id: 4, name: 'Review & Pay' },
    { id: 5, name: 'Confirmation' },
];

export type PassengerSeats = {
    passengerId: number,
    seat: FlightSeatStatus
}

export type BookingData = {
    flight: GetFlight | null;
    passengers: Passenger[];
    seats: PassengerSeats[];
    subtotal: number;
    tax: number;
};


const BookingPage = () => {
    const [searchParams] = useSearchParams();

    const navigate = useNavigate();
    const user = useAppSelector(state => state.auth.user);

    const [currentStep, setCurrentStep] = useState(1);
    const [isLoading, setIsLoading] = useState(true);
    const [bookingData, setBookingData] = useState<BookingData>({
        flight: null,
        passengers: [],
        seats: [],
        subtotal: 0,
        tax: 0
    });
    const [booking,setBooking] = useState<Booking>()

    useEffect(() => {
        const fetchFlights = async () => {
            setIsLoading(true);
            const flightId = searchParams.get('flightId');

            if (!flightId) {
                navigate('/');
                return;
            }

            try {
                const flight = await flightService.getById(Number(flightId));
                setBookingData((prev) => ({ ...prev, flight: flight }));
            } catch (error) {
                console.error("Failed to fetch flight details:", error);
            } finally {
                setIsLoading(false);
            }
        };

        fetchFlights();
    }, [searchParams, navigate]);

    const handleNextStep = () => setCurrentStep((prev) => prev + 1);
    const handlePrevStep = () => setCurrentStep((prev) => prev - 1);

    const handlePassengerSelect = (selectedPassengers: Passenger[]) => {
        setBookingData((prev) => ({ ...prev, passengers: selectedPassengers }));
        handleNextStep();
    };

    const handlePassengersSeatSelect = (selectedPassengerSeats: PassengerSeats[]) => {
        setBookingData((prev) => ({ ...prev, seats: selectedPassengerSeats }));
        handleNextStep();
    };


    const handleBookingComplete = async (subtotal: number, tax: number) => {

        const updatedBookingData = {
            ...bookingData,
            subtotal,
            tax
        };

        if (!user) return;

        setBookingData(updatedBookingData)
        const data = prepareBookingData(updatedBookingData, user.userId);

        try {
            if(!data) return;

            const result = await createBooking(data);
            setBooking(result)
            console.log('Booking created successfully:', result);

            handleNextStep();

        } catch (error) {
            toast.error('An error occurred while confirming your booking. Please try again.');
            console.error(error);
        }

    };

    const renderStep = () => {
        if (isLoading || !bookingData.flight || !user) {
            return <div>Loading flight details...</div>;
        }

        switch (currentStep) {
            case 1:
                return <FlightDetailsStep flight={bookingData.flight} onNext={handleNextStep} />;
            case 2:
                return <PassengerSelectStep onNext={handlePassengerSelect} onBack={handlePrevStep} />;
            case 3:
                return <SeatSelectionStep flightData={bookingData.flight} passengers={bookingData.passengers} onNext={handlePassengersSeatSelect} onBack={handlePrevStep} />;
            case 4:
                return <ReviewAndPayStep bookingData={bookingData} onConfirm={handleBookingComplete} onBack={handlePrevStep} />;
            case 5:
                return booking
                    ? <ConfirmationStep bookingData={bookingData} booking={booking} />
                    : <div>Loading confirmation...</div>;
            default:
                return <div>Invalid Step</div>;
        }
    };

    return (
        <div className="container mx-auto py-8 px-4">
            <div className="mb-12 flex justify-center">
                <Stepper steps={steps} currentStep={currentStep} />
            </div>

            <div className="flex justify-center">
                {renderStep()}
            </div>
        </div>
    );
};

export default BookingPage;