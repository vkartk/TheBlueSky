import { User, ChevronLeft, ChevronRight } from "lucide-react";
import { useState, useCallback } from "react";
import  { Card, CardHeader, CardTitle, CardDescription, CardContent } from "@/components/ui/card";
import  { cn } from "@/lib/utils";
import type { GetFlight } from "@/types/flight";
import type { Passenger } from "@/types/passenger";
import { Badge } from "@/components/ui/badge";
import { BookingSeatLayout } from "./BookingSeatLayout";
import { Button } from "@/components/ui/button";


export default function SeatSelectionStep({
  passengers,
  flightData,
  onNext,
  onBack
}: {
  passengers: Passenger[];
  flightData: GetFlight;
  onNext: (selections: Map<number, number>) => void;
  onBack: () => void;
}) {
  const [seatSelections, setSeatSelections] = useState<Map<number, number>>(new Map());
  const [currentPassengerIndex, setCurrentPassengerIndex] = useState(0);

  const currentPassenger = passengers[currentPassengerIndex];
  const currentPassengerId = currentPassengerIndex;

  const handleSeatSelect = useCallback((seatStatusId: number) => {
    setSeatSelections(prev => {
      const newSelections = new Map(prev);
      
      const existingPassengerId = newSelections.get(seatStatusId);
      if (existingPassengerId === currentPassengerId) {
        newSelections.delete(seatStatusId);
        return newSelections;
      }

      const currentPassengerSeat = Array.from(newSelections.entries())
        .find(([, passengerId]) => passengerId === currentPassengerId);
      
      if (currentPassengerSeat) {
        newSelections.delete(currentPassengerSeat[0]);
      }

      newSelections.set(seatStatusId, currentPassengerId);
      return newSelections;
    });
  }, [currentPassengerId]);

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
    seatSelections.forEach((passengerId, seatStatusId) => {
      const seatStatus = flightData.seatStatuses.find(
        s => s.flightSeatStatusId === seatStatusId
      );
      if (seatStatus) {
        total += seatStatus.aircraftSeat.additionalFare;
      }
    });
    return total;
  };

  return (
    <div className="space-y-6">
      <div className="grid gap-6 lg:grid-cols-[300px_1fr]">
        <Card>
          <CardHeader>
            <CardTitle>Passengers</CardTitle>
            <CardDescription>Select seats for each passenger</CardDescription>
          </CardHeader>
          <CardContent className="space-y-2">
            {passengers.map((passenger, idx) => {
              const seat = getPassengerSeat(idx);
              const isActive = idx === currentPassengerIndex;
              
              return (
                <div
                  key={idx}
                  className={cn(
                    'flex cursor-pointer items-center justify-between rounded-lg border-2 p-3 transition-all',
                    isActive
                      ? 'border-blue-500 bg-blue-50'
                      : 'border-gray-200 hover:border-gray-300'
                  )}
                  onClick={() => setCurrentPassengerIndex(idx)}
                >
                  <div className="flex items-center gap-3">
                    <div className={cn(
                      'flex h-8 w-8 items-center justify-center rounded-full',
                      isActive ? 'bg-blue-500 text-white' : 'bg-gray-200'
                    )}>
                      <User className="h-4 w-4" />
                    </div>
                    <div>
                      <div className="font-medium">
                        {passenger.firstName} {passenger.lastName}
                      </div>
                      {seat ? (
                        <div className="text-xs text-green-600 font-medium">
                          Seat {seat.seatNumber}
                        </div>
                      ) : (
                        <div className="text-xs text-gray-500">
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
          currentPassengerId={currentPassengerId}
          onSeatSelect={handleSeatSelect}
        />
      </div>

      <Card>
        <CardContent className="flex items-center justify-between p-6">
          <div>
            <div className="text-sm text-muted-foreground">Total Amount</div>
            <div className="text-2xl font-bold">₹{calculateTotalPrice().toLocaleString()}</div>
            <div className="text-xs text-muted-foreground mt-1">
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
              onClick={() => onNext(seatSelections)} 
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