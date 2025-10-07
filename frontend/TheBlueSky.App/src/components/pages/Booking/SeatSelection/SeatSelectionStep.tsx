import { User, ChevronLeft, ChevronRight } from "lucide-react";
import { useState, useCallback } from "react";
import { Card, CardHeader, CardTitle, CardDescription, CardContent } from "@/components/ui/card";
import { cn } from "@/lib/utils";
import type { GetFlight } from "@/types/flight";
import type { Passenger } from "@/types/passenger";
import { Badge } from "@/components/ui/badge";
import { BookingSeatLayout } from "./BookingSeatLayout";
import { Button } from "@/components/ui/button";
import type { PassengerSeats } from "@/pages/public/BookingPage";


export default function SeatSelectionStep({
    passengers,
    flightData,
    onNext,
    onBack
}: {
    passengers: Passenger[];
    flightData: GetFlight;
    onNext: (selections: PassengerSeats[]) => void;
    onBack: () => void;
}) {
    const [seatSelections, setSeatSelections] = useState<Map<number, number>>(new Map());
    const [currentPassengerIndex, setCurrentPassengerIndex] = useState(0);

    const currentPassengerSelectionId = currentPassengerIndex;

    const handleSeatSelect = useCallback((seatStatusId: number) => {
        setSeatSelections(prev => {
            const newSelections = new Map(prev);

            const existingPassengerId = newSelections.get(seatStatusId);
            if (existingPassengerId === currentPassengerSelectionId) {
                newSelections.delete(seatStatusId);
                return newSelections;
            }

            const currentPassengerSeat = Array.from(newSelections.entries())
                .find(([, passengerId]) => passengerId === currentPassengerSelectionId);

            if (currentPassengerSeat) {
                newSelections.delete(currentPassengerSeat[0]);
            }

            newSelections.set(seatStatusId, currentPassengerSelectionId);
            return newSelections;
        });
    }, [currentPassengerSelectionId]);

    const allSeatsSelected = passengers.every((_, idx) =>
        Array.from(seatSelections.values()).includes(idx)
    );

    const getPassengerSeat = (passengerIdx: number) => {
        const seatStatusId = Array.from(seatSelections.entries())
            .find(([, pid]) => pid === passengerIdx)?.[0];

        if (!seatStatusId) return null;

        return flightData.seatStatuses.find(
            s => s.flightSeatStatusId === seatStatusId
        )?.aircraftSeat;
    };

    const calculateTotalPrice = () => {
        let total = flightData.baseFare * passengers.length;
        seatSelections.forEach((_passengerId, seatStatusId) => {
            const seatStatus = flightData.seatStatuses.find(
                s => s.flightSeatStatusId === seatStatusId
            );
            if (seatStatus) {
                total += seatStatus.aircraftSeat.additionalFare;
            }
        });
        return total;
    };

    const handleContinue = () => {
        const finalSelections: PassengerSeats[] = [];
        
        seatSelections.forEach((passengerIndex, flightSeatStatusId) => {
            const passenger = passengers[passengerIndex];
            const seatStatus = flightData.seatStatuses.find(
                s => s.flightSeatStatusId === flightSeatStatusId
            );
            
            if (passenger && seatStatus) {
                finalSelections.push({
                    passengerId: passenger.passengerId,
                    seat: seatStatus
                });
            }
        });
        
        onNext(finalSelections);
    };

    return (
        <div className="space-y-6">
            <div className="grid gap-6 lg:grid-cols-[300px_1fr]">
                <Card>
                    <CardHeader>
                        <CardTitle>Passengers</CardTitle>
                        <CardDescription>Select a seat for each passenger</CardDescription>
                    </CardHeader>
                    <CardContent className="space-y-2">
                        {passengers.map((passenger, idx) => {
                            const seat = getPassengerSeat(idx);
                            const isActive = idx === currentPassengerIndex;

                            return (
                                <div
                                    key={passenger.passengerId}
                                    className={cn(
                                        'flex cursor-pointer items-center justify-between rounded-lg border-2 p-3 transition-all',
                                        isActive
                                            ? 'border-primary bg-muted'
                                            : 'border-border hover:border-gray-300'
                                    )}
                                    onClick={() => setCurrentPassengerIndex(idx)}
                                >
                                    <div className="flex items-center gap-3">
                                        <div className={cn(
                                            'flex h-8 w-8 items-center justify-center rounded-full',
                                            isActive ? 'bg-primary text-primary-foreground' : 'bg-gray-200'
                                        )}>
                                            <User className="h-4 w-4" />
                                        </div>
                                        <div>
                                            <div className="font-medium">
                                                {passenger.firstName} {passenger.lastName}
                                            </div>
                                            {seat ? (
                                                <div className="text-xs font-medium text-green-600">
                                                    Seat {seat.seatNumber}
                                                </div>
                                            ) : (
                                                <div className="text-xs text-muted-foreground">
                                                    No seat selected
                                                </div>
                                            )}
                                        </div>
                                    </div>
                                    {seat && (
                                        <Badge variant="secondary" className="text-xs">
                                            {seat.seatClass}
                                        </Badge>
                                    )}
                                </div>
                            );
                        })}
                    </CardContent>
                </Card>

                <BookingSeatLayout
                    flightData={flightData}
                    seatSelections={seatSelections}
                    currentPassengerId={currentPassengerSelectionId}
                    onSeatSelect={handleSeatSelect}
                />
            </div>

            <Card>
                <CardContent className="flex flex-wrap items-center justify-between gap-4 p-6">
                    <div>
                        <div className="text-sm text-muted-foreground">Total Amount</div>
                        <div className="text-2xl font-bold">₹{calculateTotalPrice().toLocaleString()}</div>
                        <div className="mt-1 text-xs text-muted-foreground">
                            {passengers.length} passenger{passengers.length > 1 ? 's' : ''} •
                            {seatSelections.size} seat{seatSelections.size !== 1 ? 's' : ''} selected
                        </div>
                    </div>

                    <div className="flex gap-3">
                        <Button variant="outline" onClick={onBack}>
                            <ChevronLeft className="mr-2 h-4 w-4" />
                            Back
                        </Button>
                        <Button
                            onClick={handleContinue}
                            disabled={!allSeatsSelected}
                        >
                            Continue to Payment
                            <ChevronRight className="ml-2 h-4 w-4" />
                        </Button>
                    </div>
                </CardContent>
            </Card>
        </div>
    );
}