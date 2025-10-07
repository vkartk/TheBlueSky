import { useMemo } from "react";

import  { Card, CardHeader, CardTitle, CardDescription, CardContent } from "@/components/ui/card";
import type { GetFlight } from "@/types/flight";
import type { FlightSeatStatus } from "@/types/flightSeatStatus";
import { BookingSeatRow } from "./BookingSeatRow";

export function BookingSeatLayout({
  flightData,
  seatSelections,
  currentPassengerId,
  onSeatSelect
}: {
  flightData: GetFlight;
  seatSelections: Map<number, number>;
  currentPassengerId: number | null;
  onSeatSelect: (seatStatusId: number) => void;
}) {
  const seatGrid = useMemo(() => {
    const grid = new Map<number, (FlightSeatStatus | null)[]>();
    
    flightData.seatStatuses.forEach(seatStatus => {
      const row = seatStatus.aircraftSeat.seatRow;
      if (!grid.has(row)) {
        grid.set(row, new Array(6).fill(null));
      }
      const seats = grid.get(row)!;
      const col = seatStatus.aircraftSeat.seatColumn - 1;
      if (col >= 0 && col < 6) {
        seats[col] = seatStatus;
      }
    });

    return Array.from(grid.entries())
      .sort(([a], [b]) => a - b)
      .map(([rowNumber, seats]) => ({ rowNumber, seats }));
  }, [flightData.seatStatuses]);

  return (
    <Card className="flex-1">
      <CardHeader>
        <div className="flex flex-wrap items-center justify-between gap-4">
          <div>
            <CardTitle>Select Your Seats</CardTitle>
            <CardDescription>
              {flightData.schedule.aircraft.aircraftModel} - Choose seats for all passengers
            </CardDescription>
          </div>

          <div className="flex items-center gap-4 text-sm">
            <div className="flex items-center gap-2">
              <div className="h-4 w-4 rounded border-2 border-gray-300 bg-white" />
              <span>Available</span>
            </div>
            <div className="flex items-center gap-2">
              <div className="h-4 w-4 rounded border-2 border-green-600 bg-green-500" />
              <span>Selected</span>
            </div>
            <div className="flex items-center gap-2">
              <div className="h-4 w-4 rounded border-2 border-gray-400 bg-gray-200" />
              <span>Booked</span>
            </div>
          </div>
        </div>
      </CardHeader>

      <CardContent className="overflow-x-auto">
        <div className="flex flex-col gap-2 py-4">
          {seatGrid.map(({ rowNumber, seats }) => (
            <BookingSeatRow
              key={rowNumber}
              rowNumber={rowNumber}
              seats={seats}
              seatSelections={seatSelections}
              currentPassengerId={currentPassengerId}
              onSeatSelect={onSeatSelect}
            />
          ))}
        </div>

        <div className="mt-6 flex flex-wrap gap-4 border-t pt-4">
          <div className="flex items-center gap-2">
            <div className="h-5 w-5 rounded border-2 border-blue-300 bg-blue-100" />
            <span className="text-sm font-medium">Economy</span>
          </div>
          <div className="flex items-center gap-2">
            <div className="h-5 w-5 rounded border-2 border-purple-300 bg-purple-100" />
            <span className="text-sm font-medium">Business</span>
          </div>
          <div className="flex items-center gap-2">
            <div className="h-5 w-5 rounded border-2 border-amber-300 bg-amber-100" />
            <span className="text-sm font-medium">First Class</span>
          </div>
        </div>
      </CardContent>
    </Card>
  );
}
