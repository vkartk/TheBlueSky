'use client';

import { useEffect, useMemo, useState } from 'react';
import { Plus } from 'lucide-react';
import { toast } from "sonner";

import { useAppDispatch, useAppSelector } from '@/store';
import {
  selectAllAirports,
  selectAirportsLoading,
} from '@/features/airports/airportsSlice';
import {
  createAirport,
  fetchAirports,
  updateAirport,
} from '@/features/airports/airportsThunks';

import { Button } from '@/components/ui/button';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Input } from '@/components/ui/input';
import { Sheet, SheetTrigger } from '@/components/ui/sheet';
import { AirportsTable } from '@/components/airports/AirportsTable';
import { AirportSheetForm } from '@/components/airports/AirportSheetForm';

import type { Airport, NewAirport } from '@/types/airports';

export const AirportsPage = () => {
  const dispatch = useAppDispatch();
  const airports = useAppSelector(selectAllAirports);
  const loading = useAppSelector(selectAirportsLoading);

  const [searchTerm, setSearchTerm] = useState('');
  const [isSheetOpen, setSheetOpen] = useState(false);
  const [selectedAirport, setSelectedAirport] = useState<Airport | null>(null);

  useEffect(() => {
    dispatch(fetchAirports());
  }, [dispatch]);

  const filteredAirports = useMemo(() => {
    if (!searchTerm) return airports;
    return airports.filter(
      (airport) =>
        airport.airportCode.toLowerCase().includes(searchTerm.toLowerCase()) ||
        airport.airportName.toLowerCase().includes(searchTerm.toLowerCase()) ||
        airport.city.toLowerCase().includes(searchTerm.toLowerCase()) ||
        airport.countryId.toLowerCase().includes(searchTerm.toLowerCase())
    );
  }, [airports, searchTerm]);

  const handleAddNew = () => {
    setSelectedAirport(null);
    setSheetOpen(true);
  };

  const handleEdit = (airport: Airport) => {
    setSelectedAirport(airport);
    setSheetOpen(true);
  };

  const handleSave = async (data: NewAirport | Airport) => {
    const isEditing = 'airportId' in data;
    const action = isEditing ? updateAirport(data as Airport) : createAirport(data as NewAirport);

    try {
      await dispatch(action).unwrap();
      toast.success(`Airport ${isEditing ? 'updated' : 'created'} successfully.`);
      setSheetOpen(false);
    } catch (error: any) {
      toast.error(error || `Failed to ${isEditing ? 'update' : 'create'} airport.`);
    }
  };

  const isSaving = loading === 'pending';

  return (
    <div className="container mx-auto py-8">
      <Card className="p-0">
        <CardHeader className="bg-primary text-primary-foreground p-4 rounded-t-lg">
          <CardTitle>Manage Airports</CardTitle>
        </CardHeader>
        <CardContent className="p-4">
          <div className="flex items-center justify-between mb-4">
            <Input
              placeholder="Search by code, name, city, or country..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              className="max-w-sm"
            />
            <Sheet open={isSheetOpen} onOpenChange={setSheetOpen}>
              <SheetTrigger asChild>
                <Button onClick={handleAddNew}>
                  <Plus className="mr-2 h-4 w-4" /> Add Airport
                </Button>
              </SheetTrigger>
              <AirportSheetForm
                key={selectedAirport?.airportId ?? 'new'}
                initialData={selectedAirport}
                onSave={handleSave}
                onClose={() => setSheetOpen(false)}
                isLoading={isSaving}
              />
            </Sheet>
          </div>

          <div className="border rounded-md">
            <AirportsTable airports={filteredAirports} onEdit={handleEdit} />
          </div>
        </CardContent>
      </Card>
    </div>
  );
};