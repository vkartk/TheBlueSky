import type { AircraftModel } from '@/types/aircraft';
import type { AircraftSeat, SeatClass } from '@/types/aircraftSeat';
import { getLayoutFromModel } from '@/utils/aircraftLayouts';

interface GenerateSeatGridParams {
  seatsMap: Map<string, AircraftSeat>;
  aircraftModel: AircraftModel;
  rows: number;
}

export function generateSeatGrid({
  seatsMap,
  aircraftModel,
  rows,
}: GenerateSeatGridParams) {
  const layoutConfig = getLayoutFromModel(aircraftModel);

  // grid row by row
  return Array.from({ length: rows }).map((_, rowIndex) => {
    const rowNumber = rowIndex + 1;
    let overallColIndex = 0;

    // left, middle, right
    const sections = layoutConfig.seatSections.map((sectionSize) => {

      const sectionSeats = Array.from({ length: sectionSize }).map((__, seatIndex) => {
        const colIndex = overallColIndex + seatIndex;

        const seat = seatsMap.get(`${rowNumber}-${colIndex + 1}`);
        const seatClass: SeatClass = seat?.seatClass ?? 'Economy';
        const letter = layoutConfig.columnLetters[colIndex];

        return {
          key: `${rowIndex}-${colIndex}`,
          row: rowNumber,
          column: colIndex + 1,
          letter,
          seatData: seat,
          seatClass,
        };
      });

      overallColIndex += sectionSize;
      return sectionSeats;
    });

    return { rowNumber, sections };
  });
}