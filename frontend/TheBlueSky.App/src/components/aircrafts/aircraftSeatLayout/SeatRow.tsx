import type { AircraftSeat } from '@/types/aircraftSeat';
import { Seat } from './Seat';

type SeatDetails = {
  key: string;
  row: number;
  column: number;
  letter: string;
  seatData?: AircraftSeat;
};

interface SeatRowProps {
  rowNumber: number;
  sections: SeatDetails[][];
  onSeatSelect: (details: SeatDetails) => void;
}

export function SeatRow({ rowNumber, sections, onSeatSelect }: SeatRowProps) {
  return (
    <div className="flex items-center gap-2">
      <div className="flex h-10 w-8 items-center justify-center font-mono text-xs text-muted-foreground">
        {rowNumber}
      </div>

      <div className="flex grow items-center">
        {sections.map((sectionSeats, sectionIndex) => (
          <div key={sectionIndex} className="flex items-center gap-x-2">
            {sectionIndex > 0 && <div className="w-8" />}
            {sectionSeats.map((details) => (
              <Seat
                key={details.key}
                seatData={details.seatData}
                onClick={() => onSeatSelect(details)}
              />
            ))}
          </div>
        ))}
      </div>
    </div>
  );
}