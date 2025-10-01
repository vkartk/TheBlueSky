import type { AircraftModel } from '@/types/aircraft';
import type { AircraftSeat } from '@/types/aircraftSeat';
import type { SeatClass } from '@/types/seatClass';
import { getLayoutFromModel } from '@/utils/aircraftLayouts';

interface GenerateSeatGridParams {
    seatsMap: Map<string, AircraftSeat>;
    seatClassMap: Map<number, SeatClass>;
    aircraftModel: AircraftModel;
    rows: number;
}

export function generateSeatGrid({
  seatsMap,
  seatClassMap,
  aircraftModel,
  rows,
}: GenerateSeatGridParams) {
  // Get the layout config (e.g., [3, 4, 3]) for the model.
  const layoutConfig = getLayoutFromModel(aircraftModel);

  // Build grid row by row.
  return Array.from({ length: rows }).map((_, rowIndex) => {
    const rowNumber = rowIndex + 1;
    let overallColIndex = 0; // Tracks column index across aisles.

    // Build sections within the row (e.g., left, middle, right).
    const sections = layoutConfig.seatSections.map((sectionSize) => {
      
      // Create each seat's data object.
      const sectionSeats = Array.from({ length: sectionSize }).map((__, seatIndex) => {
        const colIndex = overallColIndex + seatIndex;

        // Look up existing seat data from maps.
        const seat = seatsMap.get(`${rowNumber}-${colIndex + 1}`);
        const seatClass = seat ? seatClassMap.get(seat.seatClassId) : undefined;
        const letter = layoutConfig.columnLetters[colIndex];

        // Return a props object for the <Seat> component.
        return {
          key: `${rowIndex}-${colIndex}`,
          row: rowNumber,
          column: colIndex + 1,
          letter,
          seatData: seat, // Undefined if it's a new seat.
          seatClass,
        };
      });

      // Advance column index past the current section.
      overallColIndex += sectionSize;
      return sectionSeats;
    });

    return { rowNumber, sections };
  });
}