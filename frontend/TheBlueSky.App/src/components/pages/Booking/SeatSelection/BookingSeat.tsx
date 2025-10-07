import { cn } from "@/lib/utils";
import type { AircraftSeat } from "@/types/aircraftSeat";
import type { SeatStatus } from "@/types/flightSeatStatus";
import { CheckCircle2 } from "lucide-react";
import { SEAT_STATUS_COLORS } from "./Constants";

export function BookingSeat({ 
  seat, 
  status, 
  isSelectedByCurrentPassenger,
  onClick 
}: { 
  seat: AircraftSeat; 
  status: SeatStatus;
  isSelectedByCurrentPassenger: boolean;
  onClick: () => void;
}) {
  const isBooked = status === 'Booked' as SeatStatus;
  const isSelected = status === 'Selected' as SeatStatus;
  
  const colorClass = isSelected 
    ? SEAT_STATUS_COLORS.Selected
    : isBooked
    ? SEAT_STATUS_COLORS.Booked
    : SEAT_STATUS_COLORS.Available;

  return (
    <div
      className={cn(
        'relative flex h-10 w-10 cursor-pointer items-center justify-center rounded border-2 text-xs font-semibold transition-all',
        colorClass,
        isBooked && 'cursor-not-allowed opacity-60',
        isSelectedByCurrentPassenger && 'ring-2 ring-green-400 ring-offset-2'
      )}
      onClick={isBooked ? undefined : onClick}
      title={`${seat.seatNumber} - ${seat.seatClass}${seat.additionalFare > 0 ? ` (+₹${seat.additionalFare})` : ''}`}
    >
      {seat.seatNumber}
      {isSelected && (
        <CheckCircle2 className="absolute -right-1 -top-1 h-4 w-4 text-green-600" />
      )}
    </div>
  );
}
