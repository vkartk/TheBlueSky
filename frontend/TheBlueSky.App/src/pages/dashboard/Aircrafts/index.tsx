'use client';

import { useEffect, useMemo, useState } from 'react';
import { useNavigate } from 'react-router';
import { Plus } from 'lucide-react';
import { toast } from "sonner";

import { useAppDispatch, useAppSelector } from '@/store';
import { selectAllAircrafts, selectAircraftsLoading } from '@/features/aircrafts/aircraftsSlice';
import { createAircraft, fetchAircrafts } from '@/features/aircrafts/aircraftsThunks';

import { Button } from '@/components/ui/button';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Input } from '@/components/ui/input';
import { Sheet, SheetTrigger } from '@/components/ui/sheet';
import { AircraftsTable } from '@/components/aircrafts/AircraftsTable';
import { AircraftsSheetForm } from '@/components/aircrafts/AircraftsSheetForm';

import type { NewAircraft } from '@/types/aircraft';

const AircraftsPage = () => {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();
  const aircrafts = useAppSelector(selectAllAircrafts);
  const loading = useAppSelector(selectAircraftsLoading);

  const [searchTerm, setSearchTerm] = useState('');
  const [isSheetOpen, setSheetOpen] = useState(false);

  useEffect(() => {
    dispatch(fetchAircrafts());
  }, [dispatch]);

  const filteredAircrafts = useMemo(() => {
    if (!searchTerm) return aircrafts;
    return aircrafts.filter(
      (aircraft) =>
        aircraft.aircraftName.toLowerCase().includes(searchTerm.toLowerCase()) ||
        aircraft.aircraftModel.toLowerCase().includes(searchTerm.toLowerCase())
    );
  }, [aircrafts, searchTerm]);

  const handleSave = async (data: NewAircraft) => {
    try {
      const newAircraft = await dispatch(createAircraft(data)).unwrap();
      toast.success(`Aircraft "${newAircraft.aircraftName}" created successfully.`);
      setSheetOpen(false);

      toast.success(`Redirecting to edit the aircraft...`);
      navigate(`/aircrafts/edit/${newAircraft.aircraftId}`);
    } catch (error: any) {
      toast.error(error?.message || `Failed to create aircraft.`);
    }
  };

  const isSaving = loading === 'pending';

  return (
    <div className="container mx-auto py-8">
      <Card className="p-0">
        <CardHeader className="bg-primary text-primary-foreground p-4 rounded-t-lg">
          <CardTitle>Manage Aircraft</CardTitle>
        </CardHeader>
        <CardContent className="p-4">
          <div className="flex items-center justify-between mb-4">
            <Input
              placeholder="Search by name or model..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              className="max-w-sm"
            />
            <Sheet open={isSheetOpen} onOpenChange={setSheetOpen}>
              <SheetTrigger asChild>
                <Button>
                  <Plus className="mr-2 h-4 w-4" /> Add Aircraft
                </Button>
              </SheetTrigger>
              <AircraftsSheetForm
                onSave={handleSave}
                onClose={() => setSheetOpen(false)}
                isLoading={isSaving}
              />
            </Sheet>
          </div>
          <div className="border rounded-md">
            <AircraftsTable aircrafts={filteredAircrafts} />
          </div>
        </CardContent>
      </Card>
    </div>
  );
};

export default AircraftsPage;