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
import type { FlightSchedule } from '@/types/flightSchedule';

interface FlightSchedulesTableProps {
  schedules: FlightSchedule[];
  onEdit: (schedule: FlightSchedule) => void;
}

export const FlightSchedulesTable = ({ schedules, onEdit }: FlightSchedulesTableProps) => {
  return (
    <Table>
      <TableHeader>
        <TableRow>
          <TableHead>Flight No. / Flight Name</TableHead>
          <TableHead>Departure</TableHead>
          <TableHead>Arrival</TableHead>
          <TableHead>Fare</TableHead>
          <TableHead>Status</TableHead>
          <TableHead className="text-right">Actions</TableHead>
        </TableRow>
      </TableHeader>
      <TableBody>
        {schedules.length > 0 ? (
          schedules.map((schedule) => (
            <TableRow key={schedule.flightScheduleId}>
              <TableCell className="font-medium">{schedule.flightNumber} / {schedule.flightName ? schedule.flightName : 'N/A'}</TableCell>
              <TableCell>{schedule.departureTime}</TableCell>
              <TableCell>{schedule.arrivalTime}</TableCell>
              <TableCell>{schedule.baseFare.toFixed(2)}</TableCell>
              <TableCell>
                <Badge variant={schedule.isActive ? 'default' : 'destructive'}>
                  {schedule.isActive ? 'Active' : 'Inactive'}
                </Badge>
              </TableCell>
              <TableCell className="text-right">
                <Button variant="ghost" size="icon" onClick={() => onEdit(schedule)}>
                  <Pencil className="h-4 w-4" />
                  <span className="sr-only">Edit</span>
                </Button>
              </TableCell>
            </TableRow>
          ))
        ) : (
          <TableRow>
            <TableCell colSpan={6} className="h-24 text-center">
              No flight schedules found.
            </TableCell>
          </TableRow>
        )}
      </TableBody>
    </Table>
  );
};