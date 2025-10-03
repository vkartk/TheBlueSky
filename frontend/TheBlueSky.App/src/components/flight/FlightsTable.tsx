import { Pencil } from 'lucide-react';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import {
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableHeader,
    TableRow,
} from '@/components/ui/table';
import type { Flight } from '@/types/flight';
import { getFlightStatusVariant } from '@/utils/flight';

interface FlightsTableProps {
    flights: Flight[];
    onEdit: (flight: Flight) => void;
}

export const FlightsTable = ({ flights, onEdit }: FlightsTableProps) => {
    return (
        <Table>
            <TableHeader>
                <TableRow>
                    <TableHead>Flight ID</TableHead>
                    <TableHead>Flight Date</TableHead>
                    <TableHead>Departure</TableHead>
                    <TableHead>Arrival</TableHead>
                    <TableHead>Seats Left</TableHead>
                    <TableHead>Status</TableHead>
                    <TableHead className="text-right">Actions</TableHead>
                </TableRow>
            </TableHeader>
            <TableBody>
                {flights.length > 0 ? (
                    flights.map((flight) => (
                        <TableRow key={flight.flightId}>
                            <TableCell className="font-medium">{flight.flightId}</TableCell>
                            <TableCell>{flight.flightDate}</TableCell>
                            <TableCell>
                                {new Date(flight.departureDateTime).toLocaleString()}
                            </TableCell>
                            <TableCell>
                                {new Date(flight.arrivalDateTime).toLocaleString()}
                            </TableCell>
                            <TableCell>{flight.availableSeats}</TableCell>
                            <TableCell>
                                <Badge variant={getFlightStatusVariant(flight.flightStatus)}>
                                    {flight.flightStatus}
                                </Badge>
                            </TableCell>
                            <TableCell className="text-right">
                                <Button
                                    variant="ghost"
                                    size="icon"
                                    onClick={() => onEdit(flight)}
                                >
                                    <Pencil className="h-4 w-4" />
                                    <span className="sr-only">Edit</span>
                                </Button>
                            </TableCell>
                        </TableRow>
                    ))
                ) : (
                    <TableRow>
                        <TableCell colSpan={7} className="h-24 text-center">
                            No flights found.
                        </TableCell>
                    </TableRow>
                )}
            </TableBody>
        </Table>
    );
};