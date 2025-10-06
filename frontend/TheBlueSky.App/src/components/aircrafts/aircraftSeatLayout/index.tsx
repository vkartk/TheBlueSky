import { useMemo, useCallback } from 'react';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
import { cn } from '@/lib/utils';
import { generateSeatGrid } from '@/utils/generateSeatGrid';

import type { Aircraft, AircraftModel } from '@/types/aircraft';
import type { AircraftSeat } from '@/types/aircraftSeat';
import { SeatClasses, type SeatClass } from '@/types/aircraftSeat';

import { SeatRow } from './SeatRow';
import { DEFAULT_ROWS, SEAT_CLASS_COLORS } from './constants';

interface AircraftSeatLayoutProps {
  aircraft: Aircraft;
  seats: AircraftSeat[];
  onSelectSeat: (seatData: Partial<AircraftSeat & { isNew: boolean }>) => void;
}

type SeatSelectionDetails = {
  row: number;
  column: number;
  letter: string;
  seatData?: AircraftSeat;
};

export function AircraftSeatLayout({
  aircraft,
  seats,
  onSelectSeat,
}: AircraftSeatLayoutProps) {

  const seatsMap = useMemo(
    () => new Map(seats.map((s) => [`${s.seatRow}-${s.seatColumn}`, s] as const)),
    [seats]
  );

  const seatGrid = useMemo(
    () =>
      generateSeatGrid({
        seatsMap,
        aircraftModel: aircraft.aircraftModel as AircraftModel,
        rows: DEFAULT_ROWS,
      }),
    [seatsMap, aircraft.aircraftModel]
  );

  const handleSeatSelect = useCallback(
    (details: SeatSelectionDetails) => {
      if (details.seatData) {
        onSelectSeat(details.seatData);
      } else {
        onSelectSeat({
          isNew: true,
          seatRow: details.row,
          seatColumn: details.column,
          seatNumber: `${details.row}${details.letter}`,
          aircraftId: aircraft.aircraftId,
          seatClass: 'Economy',
        });
      }
    },
    [aircraft.aircraftId, onSelectSeat]
  );

  return (
    <Card className="flex-1">
      <CardHeader>
        <div className="flex flex-wrap items-center justify-between gap-4">
          <div>
            <CardTitle>Seat Layout</CardTitle>
            <CardDescription>Click a seat to add or edit its details.</CardDescription>
          </div>

          <div className="flex items-center gap-4">
            {SeatClasses.map((name) => {
              const colorClass = SEAT_CLASS_COLORS[name as SeatClass] ?? SEAT_CLASS_COLORS.default;
              const firstClass = colorClass.split(' ')[0];
              return (
                <div key={name} className="flex items-center gap-2">
                  <div className={cn('h-4 w-4 rounded-full border', firstClass)} />
                  <span className="text-sm font-medium">{name}</span>
                </div>
              );
            })}
          </div>
        </div>
      </CardHeader>

      <CardContent className="overflow-x-auto">
        <div className="flex flex-col gap-y-2">
          {seatGrid.map(({ rowNumber, sections }) => (
            <SeatRow
              key={rowNumber}
              rowNumber={rowNumber}
              sections={sections}
              onSeatSelect={handleSeatSelect}
            />
          ))}
        </div>
      </CardContent>
    </Card>
  );
}