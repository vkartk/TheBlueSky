import { Pencil } from 'lucide-react';
import { Badge } from '@/components/ui/badge';
import { Button } from '@/components/ui/button';
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from '@/components/ui/table';

import type { Airport } from '@/types/airports';

interface AirportsTableProps {
  airports: Airport[];
  onEdit: (airport: Airport) => void;
}

export const AirportsTable = ({ airports, onEdit }: AirportsTableProps) => {
  return (
    <Table>
      <TableHeader>
        <TableRow>
          <TableHead className="w-[80px]">ID</TableHead>
          <TableHead className="w-[120px]">Code</TableHead>
          <TableHead>Name</TableHead>
          <TableHead>City</TableHead>
          <TableHead>Country</TableHead>
          <TableHead>Status</TableHead>
          <TableHead className="text-right">Actions</TableHead>
        </TableRow>
      </TableHeader>
      <TableBody>
        {airports.length > 0 ? (
          airports.map((airport) => (
            <TableRow key={airport.airportId}>
              <TableCell>{airport.airportId}</TableCell>
              <TableCell className="font-medium">{airport.airportCode}</TableCell>
              <TableCell>{airport.airportName}</TableCell>
              <TableCell>{airport.city}</TableCell>
              <TableCell>{airport.countryId}</TableCell>
              <TableCell>
                <Badge variant={airport.isActive ? 'default' : 'destructive'}>
                  {airport.isActive ? 'Active' : 'Inactive'}
                </Badge>
              </TableCell>
              <TableCell className="text-right">
                <Button variant="ghost" size="icon" onClick={() => onEdit(airport)}>
                  <Pencil className="h-4 w-4" />
                  <span className="sr-only">Edit</span>
                </Button>
              </TableCell>
            </TableRow>
          ))
        ) : (
          <TableRow>
            <TableCell colSpan={7} className="h-24 text-center">
              No airports found.
            </TableCell>
          </TableRow>
        )}
      </TableBody>
    </Table>
  );
};