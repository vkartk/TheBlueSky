import type { FlightSeatStatus, SeatStatus } from "@/types/flightSeatStatus";
import { BookingSeat } from "./BookingSeat";

export function BookingSeatRow({
    rowNumber,
    seats,
    seatSelections,
    currentPassengerId,
    onSeatSelect
}: {
    rowNumber: number;
    seats: (FlightSeatStatus | null)[];
    seatSelections: Map<number, number>;
    currentPassengerId: number | null;
    onSeatSelect: (seatStatusId: number) => void;
}) {
    const leftSection = seats.slice(0, 3);
    const rightSection = seats.slice(3);

    const getSeatStatus = (seatStatus: FlightSeatStatus | null): SeatStatus => {
        if (!seatStatus) return 'Booked' as SeatStatus;
        if (seatStatus.seatStatus !== 'Available') return 'Booked' as SeatStatus;
        if (seatSelections.has(seatStatus.flightSeatStatusId)) return 'Selected' as SeatStatus;
        return 'Available';
    };

    return (
        <div className="flex items-center gap-2">
            <div className="flex h-10 w-10 items-center justify-center font-mono text-xs text-muted-foreground">
                {rowNumber}
            </div>

            <div className="flex grow items-center justify-center gap-8">
                <div className="flex items-center gap-2">
                    {leftSection.map((seatStatus, idx) => (
                        seatStatus ? (
                            <BookingSeat
                                key={seatStatus.flightSeatStatusId}
                                seat={seatStatus.aircraftSeat}
                                status={getSeatStatus(seatStatus)}
                                isSelectedByCurrentPassenger={
                                    currentPassengerId !== null &&
                                    seatSelections.get(seatStatus.flightSeatStatusId) === currentPassengerId
                                }
                                onClick={() => onSeatSelect(seatStatus.flightSeatStatusId)}
                            />
                        ) : (
                            <div key={`empty-${rowNumber}-${idx}`} className="h-10 w-10" />
                        )
                    ))}
                </div>

                <div className="flex items-center gap-2">
                    {rightSection.map((seatStatus, idx) => (
                        seatStatus ? (
                            <BookingSeat
                                key={seatStatus.flightSeatStatusId}
                                seat={seatStatus.aircraftSeat}
                                status={getSeatStatus(seatStatus)}
                                isSelectedByCurrentPassenger={
                                    currentPassengerId !== null &&
                                    seatSelections.get(seatStatus.flightSeatStatusId) === currentPassengerId
                                }
                                onClick={() => onSeatSelect(seatStatus.flightSeatStatusId)}
                            />
                        ) : (
                            <div key={`empty-${rowNumber}-${idx + 3}`} className="h-10 w-10" />
                        )
                    ))}
                </div>
            </div>
        </div>
    );
}
