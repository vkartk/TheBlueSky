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

type BookingData = {
    flight: GetFlight | null;
    passengers: Passenger[];
    seats: PassengerSeats[];
};


const BookingPage = () => {
    const [searchParams] = useSearchParams();

    const navigate = useNavigate();
    const [currentStep, setCurrentStep] = useState(1);
    const [isLoading, setIsLoading] = useState(true);
    const [bookingData, setBookingData] = useState<BookingData>({
        flight: null,
        passengers: [],
        seats: [],
    });

    console.log(bookingData);

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

    console.log(bookingData)


    const handleBookingComplete = () => {
        console.log("Booking Finalized:", bookingData);
        handleNextStep();
    };

    const renderStep = () => {
        if (isLoading || !bookingData.flight) {
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
                return <ConfirmationStep bookingData={bookingData} />;
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