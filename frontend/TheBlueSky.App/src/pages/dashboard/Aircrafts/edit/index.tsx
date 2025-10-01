import { useEffect, useState } from 'react';
import { useParams } from 'react-router';
import { useAppDispatch, useAppSelector } from '@/store';
import { fetchAircraftWithSeats, createAircraftSeat, updateAircraftSeat } from '@/features/aircrafts/edit/editAircraftThunks';
import { updateAircraft } from '@/features/aircrafts/aircraftsThunks';
import {
  selectEditAircraft,
  selectEditAircraftSeats,
  selectEditAircraftLoading,
} from '@/features/aircrafts/edit/editAircraftSlice';
import { selectAllSeatClasses } from '@/features/seatClass/seatClassSlice';
import { fetchSeatClasses } from '@/features/seatClass/seatClassThunks';

import { AircraftDetailsForm } from '@/components/aircrafts/edit/AircraftDetailsForm';
import { AircraftSeatLayout } from '@/components/aircrafts/aircraftSeatLayout';
import { AircraftSeatForm } from '@/components/aircrafts/edit/AircraftSeatForm';
import type { Aircraft } from '@/types/aircraft';
import type { AircraftSeat, NewAircraftSeat } from '@/types/aircraftSeat';
import { Loader2 } from 'lucide-react';

export default function AircraftEditPage() {
  const { aircraftId } = useParams<{ aircraftId: string }>();
  const dispatch = useAppDispatch();

  const loading = useAppSelector(selectEditAircraftLoading);
  const aircraft = useAppSelector(selectEditAircraft);
  const seats = useAppSelector(selectEditAircraftSeats);
  const seatClasses = useAppSelector(selectAllSeatClasses);

  const [isFormOpen, setIsFormOpen] = useState(false);
  const [selectedSeat, setSelectedSeat] = useState<Partial<AircraftSeat & { isNew: boolean }>>({});

  useEffect(() => {
    dispatch(fetchSeatClasses());
    if (aircraftId) {
      dispatch(fetchAircraftWithSeats(Number(aircraftId)));
    }
  }, [aircraftId, dispatch]);

  const handleDetailsSave = (data: Pick<Aircraft, 'aircraftName' | 'isActive'>) => {
    if (aircraft) {
      const updatedAircraftData: Aircraft = {
        ...aircraft,
        ...data,
      };
      dispatch(updateAircraft(updatedAircraftData));
    }
  };

  const handleSeatSelect = (seatData: Partial<AircraftSeat & { isNew: boolean }>) => {
    setSelectedSeat(seatData);
    setIsFormOpen(true);
  };

  const handleSeatFormSave = (data: Partial<AircraftSeat>) => {
    if ('isNew' in selectedSeat && selectedSeat.isNew) {
      dispatch(createAircraftSeat(data as NewAircraftSeat)).then(() => setIsFormOpen(false));
    } else {
      const updatedSeatData: AircraftSeat = {
        ...(selectedSeat as AircraftSeat),
        ...data,
      };
      dispatch(updateAircraftSeat(updatedSeatData)).then(() => setIsFormOpen(false));
    }
  };

  if (loading === 'pending' || loading === 'idle') {
    return (
      <div className="flex h-screen items-center justify-center">
        <Loader2 className="h-8 w-8 animate-spin" />
      </div>
    );
  }

  if (!aircraft) {
    return <div>Aircraft not found or failed to load.</div>;
  }

  return (
    <div className="container mx-auto p-4">
      <div className="flex flex-col gap-8 lg:flex-row">
        <div className="w-full lg:w-1/3">
          <AircraftDetailsForm aircraft={aircraft} onSave={handleDetailsSave} isSaving={false} />
        </div>
        <div className="w-full lg:w-2/3">
          <AircraftSeatLayout
            aircraft={aircraft}
            seats={seats}
            seatClasses={seatClasses}
            onSelectSeat={handleSeatSelect}
          />
        </div>
      </div>
      <AircraftSeatForm
        isOpen={isFormOpen}
        onClose={() => setIsFormOpen(false)}
        onSave={handleSeatFormSave}
        seatData={selectedSeat}
        seatClasses={seatClasses}
        isSaving={false}
      />
    </div>
  );
}