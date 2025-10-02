'use client';

import { useEffect, useMemo, useState } from 'react';
import { useNavigate } from 'react-router';
import { Plus } from 'lucide-react';
import { toast } from 'sonner';

import { useAppDispatch, useAppSelector } from '@/store';
import { Button } from '@/components/ui/button';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Input } from '@/components/ui/input';
import { Sheet, SheetTrigger } from '@/components/ui/sheet';

import { FlightSchedulesTable } from '@/components/flightSchedule/FlightSchedulesTable';
import { FlightScheduleSheetForm } from '@/components/flightSchedule/FlightScheduleSheetForm';
import { selectAllFlightSchedules, selectFlightSchedulesLoading } from '@/features/flightSchedule/flightScheduleSlice';
import { createFlightSchedule, fetchFlightSchedules } from '@/features/flightSchedule/flightScheduleThunks';
import type { FlightSchedule, NewFlightSchedule } from '@/types/flightSchedule';


import { selectAllRoutes } from '@/features/routes/routesSlice';
import { selectAllAirports } from '@/features/airports/airportsSlice';
import { selectAllAircrafts } from '@/features/aircrafts/aircraftsSlice';

import { fetchRoutes } from '@/features/routes/routesThunks';
import { fetchAirports } from '@/features/airports/airportsThunks';
import { fetchAircrafts } from '@/features/aircrafts/aircraftsThunks';



const FlightSchedulesPage = () => {
    const dispatch = useAppDispatch();
    const navigate = useNavigate();


    const schedules = useAppSelector(selectAllFlightSchedules);
    const loadingSchedules = useAppSelector(selectFlightSchedulesLoading);
    const aircrafts = useAppSelector(selectAllAircrafts);
    const routes = useAppSelector(selectAllRoutes);
    const airports = useAppSelector(selectAllAirports);

    console.log('Routes:', routes);

    // Local state
    const [searchTerm, setSearchTerm] = useState('');
    const [isSheetOpen, setSheetOpen] = useState(false);

    useEffect(() => {
        dispatch(fetchFlightSchedules());
        dispatch(fetchAircrafts());
        dispatch(fetchRoutes());
        dispatch(fetchAirports());
    }, [dispatch]);

    const filteredSchedules = useMemo(() => {
        if (!searchTerm) return schedules;
        return schedules.filter(
            (s) =>
                s.flightNumber.toLowerCase().includes(searchTerm.toLowerCase()) ||
                s.flightName?.toLowerCase().includes(searchTerm.toLowerCase())
        );
    }, [schedules, searchTerm]);

    const handleAddNew = () => {
        setSheetOpen(true);
    };

    const handleManage = (schedule: FlightSchedule) => {
        console.log('Navigating to manage page for schedule ID:', schedule.flightScheduleId);
        navigate(`/dashboard/flight-schedules/manage/${schedule.flightScheduleId}`);
    };


    const handleSave = async (data: NewFlightSchedule) => {
        try {
            const newSchedule = await dispatch(createFlightSchedule(data)).unwrap();
            setSheetOpen(false);

            toast.success(`Flight schedule ${newSchedule.flightNumber} created successfully.`);
            handleManage(newSchedule);
        } catch (error: any) {
            toast.error(error?.message || 'Failed to create flight schedule.');
        }
    };

    const isSaving = loadingSchedules === 'pending';

    return (
        <div className="container mx-auto py-8">
            <Card className="p-0">
                <CardHeader className="bg-primary text-primary-foreground p-4 rounded-t-lg">
                    <CardTitle>Manage Flight Schedules</CardTitle>
                </CardHeader>
                <CardContent className="p-4">
                    <div className="flex items-center justify-between mb-4">
                        <Input
                            placeholder="Search by flight number or name..."
                            value={searchTerm}
                            onChange={(e) => setSearchTerm(e.target.value)}
                            className="max-w-sm"
                        />
                        <Sheet open={isSheetOpen} onOpenChange={setSheetOpen}>
                            <SheetTrigger asChild>
                                <Button onClick={handleAddNew}>
                                    <Plus className="mr-2 h-4 w-4" /> Add Schedule
                                </Button>
                            </SheetTrigger>
                            <FlightScheduleSheetForm
                                aircrafts={aircrafts}
                                routes={routes}
                                airports={airports}
                                onSave={handleSave}
                                onClose={() => setSheetOpen(false)}
                                isLoading={isSaving}
                            />
                        </Sheet>
                    </div>
                    <div className="border rounded-md">
                        <FlightSchedulesTable schedules={filteredSchedules} onEdit={handleManage} />
                    </div>
                </CardContent>
            </Card>
        </div>
    );
};

export default FlightSchedulesPage;