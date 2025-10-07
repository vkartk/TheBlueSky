import { useState, useEffect } from 'react';
import { useAppDispatch, useAppSelector } from '@/store';
import {
    selectAllPassengers,
    selectPassengerError,
    selectPassengersLoading,
} from '@/features/passenger/passengerSlice';
import { selectAllCountries } from '@/features/countries/countriesSlice';

import { Button } from '@/components/ui/button';
import { PassengerTable } from '@/components/pages/account/passengers/passenger-table';
import { PassengerSheetForm } from '@/components/pages/account/passengers/PassengerSheetForm';
import type { Passenger, NewPassenger } from '@/types/passenger';
import { createPassenger, fetchPassengerByUser, updatePassenger } from '@/features/passenger/passengerThunks';
import { fetchCountries } from '@/features/countries/countriesThunks';

export default function PassengersPage() {
    const dispatch = useAppDispatch();
    const user = useAppSelector((state) => state.auth.user);
    const countries = useAppSelector(selectAllCountries);

    const passengers = useAppSelector(selectAllPassengers);
    const status = useAppSelector(selectPassengersLoading);
    const error = useAppSelector(selectPassengerError);

    const [isSheetOpen, setIsSheetOpen] = useState(false);
    const [editingPassenger, setEditingPassenger] = useState<Passenger | null>(null);

    useEffect(() => {
        dispatch(fetchCountries());
        if (user?.userId) {
            dispatch(fetchPassengerByUser(user.userId));
        }
    }, [dispatch, user]);

    const handleOpenAdd = () => {
        setEditingPassenger(null);
        setIsSheetOpen(true);
    };

    const handleOpenEdit = (passenger: Passenger) => {
        setEditingPassenger(passenger);
        setIsSheetOpen(true);
    };

    const handleClose = () => {
        setIsSheetOpen(false);
        setEditingPassenger(null);
    };

    const handleSave = async (data: NewPassenger | Passenger) => {
        try {
            if ('passengerId' in data) {
                await dispatch(updatePassenger(data)).unwrap();
            } else {
                await dispatch(createPassenger(data)).unwrap();
            }
            handleClose();
        } catch (err) {
            console.error('Failed to save the passenger: ', err);
        }
    };

    return (
        <div className="container mx-auto p-8">
            <header className="flex items-center justify-between mb-6">
                <div>
                    <h1 className="text-3xl font-bold tracking-tight">Passengers</h1>
                    <p className="text-muted-foreground">Manage your travel companions.</p>
                </div>
                <Button onClick={handleOpenAdd}>Add Passenger</Button>
            </header>

            <div>
                {status === 'pending' && <p className="text-center">Loading passengers...</p>}
                {status === 'failed' && <p className="text-center text-red-500">{error}</p>}
                {status === 'succeeded' && (
                    <PassengerTable
                        passengers={passengers}
                        onEdit={handleOpenEdit}
                    />
                )}
            </div>

            <PassengerSheetForm
                isOpen={isSheetOpen}
                onClose={handleClose}
                initialData={editingPassenger}
                onSave={handleSave}
                isLoading={status === 'pending'} 
                currentUser={user}
                countries={countries}/>
        </div>
    );
}