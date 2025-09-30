import { Pencil } from 'lucide-react';
import { Link } from 'react-router';
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

import type { Aircraft } from '@/types/aircraft';

interface AircraftsTableProps {
  aircrafts: Aircraft[];
}

export const AircraftsTable = ({ aircrafts }: AircraftsTableProps) => {
  return (
    <Table>
      <TableHeader>
        <TableRow>
          <TableHead>Name</TableHead>
          <TableHead>Model</TableHead>
          <TableHead>Manufacturer</TableHead>
          <TableHead>Seats (Economy/Business/First)</TableHead>
          <TableHead>Status</TableHead>
          <TableHead className="text-right">Actions</TableHead>
        </TableRow>
      </TableHeader>
      <TableBody>
        {aircrafts.length > 0 ? (
          aircrafts.map((aircraft) => (
            <TableRow key={aircraft.aircraftId}>
              <TableCell className="font-medium">{aircraft.aircraftName}</TableCell>
              <TableCell>{aircraft.aircraftModel}</TableCell>
              <TableCell>{aircraft.manufacturer}</TableCell>
              <TableCell>
                <div className="flex items-center gap-2">
                  <Badge variant="secondary" title="Economy Class Seats">
                    E: {aircraft.economySeats}
                  </Badge>
                  <Badge variant="default" title="Business Class Seats">
                    B: {aircraft.businessSeats}
                  </Badge>
                  <Badge
                    className="bg-sky-800 hover:bg-sky-700 text-white"
                    title="First Class Seats"
                  >
                    F: {aircraft.firstClassSeats}
                  </Badge>
                </div>
              </TableCell>
              <TableCell>
                <Badge variant={aircraft.isActive ? 'default' : 'destructive'}>
                  {aircraft.isActive ? 'Active' : 'Inactive'}
                </Badge>
              </TableCell>
              <TableCell className="text-right">
                <Button asChild variant="ghost" size="icon">
                  <Link to={`/aircrafts/edit/${aircraft.aircraftId}`}>
                    <Pencil className="h-4 w-4" />
                    <span className="sr-only">Edit Aircraft</span>
                  </Link>
                </Button>
              </TableCell>
            </TableRow>
          ))
        ) : (
          <TableRow>
            <TableCell colSpan={7} className="h-24 text-center">
              No aircraft found.
            </TableCell>
          </TableRow>
        )}
      </TableBody>
    </Table>
  );
};