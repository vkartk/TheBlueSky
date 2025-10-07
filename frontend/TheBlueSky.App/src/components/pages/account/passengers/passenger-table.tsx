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
import type { Passenger } from '@/types/passenger';

interface PassengerTableProps {
  passengers: Passenger[];
  onEdit: (passenger: Passenger) => void;
}

export const PassengerTable = ({ passengers, onEdit }: PassengerTableProps) => {
  return (
    <Table>
      <TableHeader>
        <TableRow>
          <TableHead>Name</TableHead>
          <TableHead>Date of Birth</TableHead>
          <TableHead>Passport Number</TableHead>
          <TableHead>Status</TableHead>
          <TableHead className="text-right">Actions</TableHead>
        </TableRow>
      </TableHeader>
      <TableBody>
        {passengers.length > 0 ? (
          passengers.map((passenger) => (
            <TableRow key={passenger.passengerId}>
              <TableCell className="font-medium">
                {passenger.firstName} {passenger.lastName}
              </TableCell>
              <TableCell>{passenger.dateOfBirth}</TableCell>
              <TableCell>{passenger.passportNumber || 'N/A'}</TableCell>
              <TableCell>
                <Badge variant={passenger.isActive ? 'default' : 'destructive'}>
                  {passenger.isActive ? 'Active' : 'Inactive'}
                </Badge>
              </TableCell>
              <TableCell className="text-right">
                <Button
                  variant="ghost"
                  size="icon"
                  onClick={() => onEdit(passenger)}
                >
                  <Pencil className="h-4 w-4" />
                  <span className="sr-only">Edit</span>
                </Button>
              </TableCell>
            </TableRow>
          ))
        ) : (
          <TableRow>
            <TableCell colSpan={5} className="h-24 text-center">
              No passengers found.
            </TableCell>
          </TableRow>
        )}
      </TableBody>
    </Table>
  );
};