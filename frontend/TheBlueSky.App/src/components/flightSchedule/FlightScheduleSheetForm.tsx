import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';

import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Switch } from '@/components/ui/switch';
import {
    SheetContent,
    SheetHeader,
    SheetTitle,
    SheetFooter,
    SheetClose,
} from '@/components/ui/sheet';
import {
    Form,
    FormControl,
    FormField,
    FormItem,
    FormLabel,
    FormMessage,
} from '@/components/ui/form';
import { AircraftCombobox } from '@/components/aircrafts/AircraftCombobox';
import { RouteCombobox } from '@/components/route/RouteCombobox';

import type { NewFlightSchedule } from '@/types/flightSchedule';
import type { Aircraft } from '@/types/aircraft';
import type { Route } from '@/types/route';
import type { Airport } from '@/types/airports';

const formSchema = z.object({
    aircraftId: z.number().int().positive('Please select an aircraft.'),
    routeId: z.number().int().positive('Please select a route.'),
    flightNumber: z.string().min(1, 'Flight number is required.'),
    flightName: z.string().max(50).optional().nullable(),
    departureTime: z.string().regex(/^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$/, 'Invalid time format (HH:mm).'),
    arrivalTime: z.string().regex(/^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$/, 'Invalid time format (HH:mm).'),
    baseFare: z.number().min(0, 'Base fare must be a positive number.'),
    checkinBaggageWeightKg: z.number().int().min(0).max(50, 'Max check-in weight is 50kg.'),
    cabinBaggageWeightKg: z.number().int().min(0).max(25, 'Max cabin weight is 25kg.'),
    validFrom: z.string().min(1, 'Valid from date is required.'),
    validUntil: z.string().min(1, 'Valid until date is required.'),
    isActive: z.boolean(),
});

type FormValues = z.infer<typeof formSchema>;

interface FlightScheduleSheetFormProps {
    aircrafts: Aircraft[];
    routes: Route[];
    airports: Airport[];
    onSave: (data: NewFlightSchedule) => void;
    onClose: () => void;
    isLoading: boolean;
}

export const FlightScheduleSheetForm = ({
    aircrafts,
    routes,
    airports,
    onSave,
    onClose,
    isLoading,
}: FlightScheduleSheetFormProps) => {
    const form = useForm<FormValues>({
        resolver: zodResolver(formSchema),
        defaultValues: {
            aircraftId: undefined,
            routeId: undefined,
            flightNumber: '',
            flightName: '',
            departureTime: '09:00',
            arrivalTime: '11:00',
            baseFare: 100,
            checkinBaggageWeightKg: 20,
            cabinBaggageWeightKg: 7,
            validFrom: new Date().toISOString().split('T')[0], // Defaults to today
            validUntil: '',
            isActive: true,
        },
    });

    const onSubmit = (values: FormValues) => {
        // Format time as "HH:mm:ss"
        const data = {
            ...values,
            departureTime: `${values.departureTime}:00`,
            arrivalTime: `${values.arrivalTime}:00`,
        };
        onSave(data as NewFlightSchedule);
    };

    return (
        <SheetContent className="sm:max-w-lg p-4">
            <SheetHeader>
                <SheetTitle>Add New Flight Schedule</SheetTitle>
            </SheetHeader>
            <Form {...form}>
                <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-4 py-4 overflow-y-auto max-h-[calc(100vh-8rem)] pr-6">

                    <FormField
                        control={form.control}
                        name="aircraftId"
                        render={({ field }) => (
                            <FormItem>
                                <FormLabel>Aircraft</FormLabel>
                                <FormControl>
                                    <AircraftCombobox aircraftList={aircrafts} value={field.value} onChange={field.onChange} />
                                </FormControl>
                                <FormMessage />
                            </FormItem>
                        )}
                    />
                    <FormField
                        control={form.control}
                        name="routeId"
                        render={({ field }) => (
                            <FormItem>
                                <FormLabel>Route</FormLabel>
                                <FormControl>
                                    <RouteCombobox routes={routes} airports={airports} value={field.value} onChange={field.onChange} />
                                </FormControl>
                                <FormMessage />
                            </FormItem>
                        )}
                    />
                    <div className="grid grid-cols-2 gap-4">
                        <FormField
                            control={form.control}
                            name="flightNumber"
                            render={({ field }) => (
                                <FormItem>
                                    <FormLabel>Flight Number</FormLabel>
                                    <FormControl><Input placeholder="e.g., AI-202" {...field} /></FormControl>
                                    <FormMessage />
                                </FormItem>
                            )}
                        />
                        <FormField
                            control={form.control}
                            name="flightName"
                            render={({ field }) => (
                                <FormItem>
                                    <FormLabel>Flight Name (Optional)</FormLabel>
                                    <FormControl><Input placeholder="e.g., The Chennai Express" {...field} value={field.value ?? ''} /></FormControl>
                                    <FormMessage />
                                </FormItem>
                            )}
                        />
                        <FormField
                            control={form.control}
                            name="departureTime"
                            render={({ field }) => (
                                <FormItem>
                                    <FormLabel>Departure Time</FormLabel>
                                    <FormControl><Input type="time" {...field} /></FormControl>
                                    <FormMessage />
                                </FormItem>
                            )}
                        />
                        <FormField
                            control={form.control}
                            name="arrivalTime"
                            render={({ field }) => (
                                <FormItem>
                                    <FormLabel>Arrival Time</FormLabel>
                                    <FormControl><Input type="time" {...field} /></FormControl>
                                    <FormMessage />
                                </FormItem>
                            )}
                        />
                        <FormField
                            control={form.control}
                            name="validFrom"
                            render={({ field }) => (
                                <FormItem>
                                    <FormLabel>Valid From</FormLabel>
                                    <FormControl><Input type="date" {...field} /></FormControl>
                                    <FormMessage />
                                </FormItem>
                            )}
                        />
                        <FormField
                            control={form.control}
                            name="validUntil"
                            render={({ field }) => (
                                <FormItem>
                                    <FormLabel>Valid Until</FormLabel>
                                    <FormControl><Input type="date" {...field} /></FormControl>
                                    <FormMessage />
                                </FormItem>
                            )}
                        />
                        <FormField
                            control={form.control}
                            name="checkinBaggageWeightKg"
                            render={({ field }) => (
                                <FormItem>
                                    <FormLabel>Check-in Baggage (kg)</FormLabel>
                                    <FormControl><Input type="number" {...field} /></FormControl>
                                    <FormMessage />
                                </FormItem>
                            )}
                        />
                        <FormField
                            control={form.control}
                            name="cabinBaggageWeightKg"
                            render={({ field }) => (
                                <FormItem>
                                    <FormLabel>Cabin Baggage (kg)</FormLabel>
                                    <FormControl><Input type="number" {...field} /></FormControl>
                                    <FormMessage />
                                </FormItem>
                            )}
                        />
                    </div>
                    <FormField
                        control={form.control}
                        name="baseFare"
                        render={({ field }) => (
                            <FormItem >
                                <FormLabel>Base Fare</FormLabel>
                                <FormControl>
                                    <Input
                                        type="number"
                                        value={field.value || ''}
                                        onChange={(e) => {
                                            const value = e.target.value;
                                            field.onChange(value === '' ? 0 : parseInt(value) || 0);
                                        }} />
                                </FormControl>
                                <FormMessage />
                            </FormItem>
                        )}
                    />
                    <FormField
                        control={form.control}
                        name="isActive"
                        render={({ field }) => (
                            <FormItem className="flex flex-row items-center justify-between rounded-lg border p-3 shadow-sm">
                                <div className="space-y-0.5">
                                    <FormLabel>Active Status</FormLabel>
                                </div>
                                <FormControl>
                                    <Switch checked={field.value} onCheckedChange={field.onChange} />
                                </FormControl>
                            </FormItem>
                        )}
                    />

                    <SheetFooter className="pt-4">
                        <SheetClose asChild>
                            <Button type="button" variant="outline" onClick={onClose}>
                                Cancel
                            </Button>
                        </SheetClose>
                        <Button type="submit" disabled={isLoading}>
                            {isLoading ? 'Saving...' : 'Save Schedule'}
                        </Button>
                    </SheetFooter>
                </form>
            </Form>
        </SheetContent>
    );
};