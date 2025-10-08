'use client';

import { useEffect } from 'react';
import { useParams } from 'react-router';
import { toast } from 'sonner';
import { useAppDispatch, useAppSelector } from '@/store';
import {
  fetchScheduleDetails,
  fetchFlightsForSchedule,
  updateScheduleDays,
  generateFlights,
} from '@/features/flightSchedule/Manage/flightScheduleManageThunks';
import {
  selectScheduleDetails,
  selectScheduleDays,
  selectGeneratedFlights,
  selectScheduleManageLoading,
} from '@/features/flightSchedule/Manage/flightScheduleManageSlice';

import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { ScheduleDaySelector } from '@/components/flightSchedule/Manage/ScheduleDaySelector';
import { FlightGenerator } from '@/components/flightSchedule/Manage/FlightGenerator';
import { GeneratedFlightsTable } from '@/components/flightSchedule/Manage/GeneratedFlightsTable';
import type { DayOfWeek } from '@/types/scheduleDay';
import { ScheduleDetailsCard } from '@/components/flightSchedule/Manage/ScheduleDetailsCard';


const ManageFlightSchedulePage = () => {
  const { scheduleId } = useParams<{ scheduleId: string }>();
  const dispatch = useAppDispatch();

  const details = useAppSelector(selectScheduleDetails);
  const scheduleDays = useAppSelector(selectScheduleDays);
  const generatedFlights = useAppSelector(selectGeneratedFlights);
  const loading = useAppSelector(selectScheduleManageLoading);

  useEffect(() => {
    if (scheduleId) {
      const id = parseInt(scheduleId, 10);
      dispatch(fetchScheduleDetails(id));
      dispatch(fetchFlightsForSchedule(id));
    }
  }, [dispatch, scheduleId]);

  const handleSaveDays = async (days: DayOfWeek[]) => {
    if (!scheduleId) return;
    const action = await dispatch(updateScheduleDays({ scheduleId: parseInt(scheduleId), days }));
    if (updateScheduleDays.fulfilled.match(action)) {
      toast.success('Weekly schedule saved successfully.');
    } else {
      toast.error(action.payload as string || 'Failed to save schedule.');
    }
  };

  const handleGenerateFlights = async (range: { startDate: string; endDate: string }) => {
    if (!scheduleId) return;
    const action = await dispatch(generateFlights({
      scheduleId: parseInt(scheduleId),
      startDate: range.startDate,
      endDate: range.endDate,
    }));

    if (generateFlights.fulfilled.match(action)) {
      const { count } = action.payload;
      toast.success(`Successfully generated ${count} flight${count !== 1 ? 's' : ''}.`);
    } else {
      toast.error(action.payload as string || 'Failed to generate flights.');
    }
  };

  if (!scheduleId) {
    return <div>Error: No Schedule ID provided.</div>;
  }

  return (
    <div className="container mx-auto py-8 space-y-6">
      <ScheduleDetailsCard details={details} />

      <ScheduleDaySelector
        initialDays={scheduleDays}
        onSave={handleSaveDays}
        isLoading={loading.isSavingDays}
      />

      <FlightGenerator
        onGenerate={handleGenerateFlights}
        isLoading={loading.isGenerating}
      />

      <Card>
        <CardHeader>
          <CardTitle>Generated Flights</CardTitle>
        </CardHeader>
        <CardContent>
          <GeneratedFlightsTable flights={generatedFlights} />
        </CardContent>
      </Card>
    </div>
  );
};

export default ManageFlightSchedulePage;