import { useMemo } from 'react';
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
import type { Route } from '@/types/route';
import type { Airport } from '@/types/airports'; 

interface RouteTableProps {
  routes: Route[];
  airports: Airport[];
  onEdit: (route: Route) => void;
}

export const RouteTable = ({ routes, airports, onEdit }: RouteTableProps) => {
  
  const airportMap = useMemo(() => {
    return new Map(airports.map((airport) => [airport.airportId, airport]));
  }, [airports]);

  return (
    <Table>
      <TableHeader>
        <TableRow>
          <TableHead className="w-[80px]">ID</TableHead>
          <TableHead>Origin</TableHead>
          <TableHead>Destination</TableHead>
          <TableHead>Distance (km)</TableHead>
          <TableHead>Duration (min)</TableHead>
          <TableHead>Status</TableHead>
          <TableHead className="text-right">Actions</TableHead>
        </TableRow>
      </TableHeader>
      <TableBody>
        {routes.length > 0 ? (
          routes.map((route) => {
            
            const origin = airportMap.get(route.originAirportId);
            const destination = airportMap.get(route.destinationAirportId);

            return (
              <TableRow key={route.routeId}>
                <TableCell>{route.routeId}</TableCell>
                
                <TableCell className="font-medium">
                  {origin ? `${origin.airportCode} - ${origin.city}` : `ID: ${route.originAirportId}`}
                </TableCell>
                <TableCell className="font-medium">
                  {destination ? `${destination.airportCode} - ${destination.city}` : `ID: ${route.destinationAirportId}`}
                </TableCell>
                
                <TableCell>{route.distanceKm}</TableCell>
                <TableCell>{route.estimatedDurationMinutes}</TableCell>
                <TableCell>
                  <Badge variant={route.isActive ? 'default' : 'destructive'}>
                    {route.isActive ? 'Active' : 'Inactive'}
                  </Badge>
                </TableCell>
                <TableCell className="text-right">
                  <Button variant="ghost" size="icon" onClick={() => onEdit(route)}>
                    <Pencil className="h-4 w-4" />
                    <span className="sr-only">Edit</span>
                  </Button>
                </TableCell>
              </TableRow>
            );
          })
        ) : (
          <TableRow>
            <TableCell colSpan={7} className="h-24 text-center">
              No routes found.
            </TableCell>
          </TableRow>
        )}
      </TableBody>
    </Table>
  );
};