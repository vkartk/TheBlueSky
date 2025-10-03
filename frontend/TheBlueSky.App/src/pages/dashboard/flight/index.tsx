'use client';

import { useEffect, useMemo, useState } from 'react';
import { toast } from 'sonner';

import { useAppDispatch, useAppSelector } from '@/store';
import {
  selectAllFlights,
  selectFlightsLoading,
} from '@/features/flight/flightSlice';
import { fetchFlights, updateFlight } from '@/features/flight/flightThunks';

import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Input } from '@/components/ui/input';
import { Sheet } from '@/components/ui/sheet';
import { FlightsTable } from '@/components/flight/FlightsTable';
import { FlightSheetForm } from '@/components/flight/FlightSheetForm';

import type { Flight } from '@/types/flight';

const FlightsPage = () => {
  const dispatch = useAppDispatch();
  const flights = useAppSelector(selectAllFlights);
  const loading = useAppSelector(selectFlightsLoading);

  const [searchTerm, setSearchTerm] = useState('');
  const [isSheetOpen, setSheetOpen] = useState(false);
  const [selectedFlight, setSelectedFlight] = useState<Flight | null>(null);

  useEffect(() => {
    if (flights.length === 0) {
      dispatch(fetchFlights());
    }
  }, [dispatch, flights.length]);

  const filteredFlights = useMemo(() => {
    if (!searchTerm) return flights;
    return flights.filter(
      (flight) =>
        flight.flightId.toString().includes(searchTerm) ||
        flight.flightDate.toLowerCase().includes(searchTerm.toLowerCase()),
    );
  }, [flights, searchTerm]);

  const handleEdit = (flight: Flight) => {
    setSelectedFlight(flight);
    setSheetOpen(true);
  };

  const handleSave = async (data: Flight) => {
    try {
      await dispatch(updateFlight(data)).unwrap();
      toast.success(`Flight #${data.flightId} updated successfully.`);
      setSheetOpen(false);
      setSelectedFlight(null);
    } catch (error: any) {
      toast.error(error?.message || `Failed to update flight #${data.flightId}.`);
    }
  };

  const isSaving = loading === 'pending';

  return (
    <div className="container mx-auto py-8">
      <Card className="p-0">
        <CardHeader className="bg-primary text-primary-foreground p-4 rounded-t-lg">
          <CardTitle>Manage Flights</CardTitle>
        </CardHeader>
        <CardContent className="p-4">
          <div className="flex items-center justify-between mb-4">
            <Input
              placeholder="Search by Flight ID or Date..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              className="max-w-sm"
            />
          </div>

          <Sheet
            open={isSheetOpen}
            onOpenChange={(isOpen) => {
              setSheetOpen(isOpen);
              if (!isOpen) {
                setSelectedFlight(null);
              }
            }}
          >
            {selectedFlight && (
              <FlightSheetForm
                key={selectedFlight.flightId}
                initialData={selectedFlight}
                onSave={handleSave}
                onClose={() => setSheetOpen(false)}
                isLoading={isSaving}
              />
            )}
            <div className="border rounded-md">
              <FlightsTable flights={filteredFlights} onEdit={handleEdit} />
            </div>
          </Sheet>
        </CardContent>
      </Card>
    </div>
  );
};

export default FlightsPage;