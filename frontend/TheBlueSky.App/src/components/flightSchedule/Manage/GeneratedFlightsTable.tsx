import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from '@/components/ui/table';
import { Badge } from '@/components/ui/badge';
import type { Flight } from '@/types/flight';
import { getFlightStatusVariant } from '@/utils/flight';

interface GeneratedFlightsTableProps {
  flights: Flight[];
}

export const GeneratedFlightsTable = ({ flights }: GeneratedFlightsTableProps) => {

  const dateOptions: Intl.DateTimeFormatOptions = {
    year: 'numeric',
    month: 'long',
    day: 'numeric',
    timeZone: 'UTC', // Treat the YYYY-MM-DD date as UTC to prevent off-by-one day errors
  };
  const timeOptions: Intl.DateTimeFormatOptions = {
    hour: 'numeric',
    minute: '2-digit',
  };

  return (
    <div className="border rounded-md">
      <Table>
        <TableHeader>
          <TableRow>
            <TableHead>Flight Date</TableHead>
            <TableHead>Departure</TableHead>
            <TableHead>Arrival</TableHead>
            <TableHead>Status</TableHead>
            <TableHead className="text-right">Available Seats</TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          {flights.length > 0 ? (
            flights.map((flight) => (
              <TableRow key={flight.flightId}>
                <TableCell className="font-medium">
                  {new Date(flight.flightDate).toLocaleDateString(undefined, dateOptions)}
                </TableCell>
                <TableCell>
                  {new Date(flight.departureDateTime).toLocaleTimeString(undefined, timeOptions)}
                </TableCell>
                <TableCell>
                  {new Date(flight.arrivalDateTime).toLocaleTimeString(undefined, timeOptions)}
                </TableCell>
                <TableCell>
                  <Badge variant={getFlightStatusVariant(flight.flightStatus)}>
                    {flight.flightStatus}
                  </Badge>
                </TableCell>
                <TableCell className="text-right">{flight.availableSeats}</TableCell>
              </TableRow>
            ))
          ) : (
            <TableRow>
              <TableCell colSpan={5} className="h-24 text-center">
                No flights generated for this schedule yet.
              </TableCell>
            </TableRow>
          )}
        </TableBody>
      </Table>
    </div>
  );
};