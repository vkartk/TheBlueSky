import { zodResolver } from '@hookform/resolvers/zod';
import { useForm } from 'react-hook-form';
import * as z from 'zod';
import { useEffect } from 'react';

import { Button } from '@/components/ui/button';
import {
    Form,
    FormControl,
    FormField,
    FormItem,
    FormLabel,
    FormMessage,
} from '@/components/ui/form';
import { Input } from '@/components/ui/input';
import { SheetContent, SheetHeader, SheetTitle, SheetFooter, SheetDescription, SheetClose } from '@/components/ui/sheet';
import { Switch } from '@/components/ui/switch';
import { AirportCombobox } from '@/components/airports/AirportCombobox';
import type { Route } from '@/types/route';
import type { Airport } from '@/types/airports';

const routeSchema = z.object({
    originAirportId: z.number()
        .positive({ message: 'Please select an origin airport.' }),
    destinationAirportId: z.number()
        .positive({ message: 'Please select a destination airport.' }),
    distanceKm: z.number()
        .min(0, "Distance can't be negative.")
        .max(50000, 'Distance seems too high.'),
    estimatedDurationMinutes: z.number()
        .min(0, "Duration can't be negative.")
        .max(2880, 'Duration seems too long.'),
    isActive: z.boolean(),
}).refine(data => data.originAirportId !== data.destinationAirportId, {
    message: "Origin and Destination cannot be the same.",
    path: ["destinationAirportId"],
});

type RouteFormValues = z.infer<typeof routeSchema>;

interface RouteSheetFormProps {
    initialData?: Route | null;
    onSave: (data: RouteFormValues) => Promise<void>;
    onClose: () => void;
    isLoading: boolean;
    airports: Airport[];
}

export const RouteSheetForm = ({ initialData, onSave,onClose, isLoading, airports }: RouteSheetFormProps) => {

    const isEditMode = !!initialData;

    const defaultValues: RouteFormValues = {
        originAirportId: initialData?.originAirportId ?? 0,
        destinationAirportId: initialData?.destinationAirportId ?? 0,
        distanceKm: initialData?.distanceKm ?? 0,
        estimatedDurationMinutes: initialData?.estimatedDurationMinutes ?? 0,
        isActive: initialData?.isActive ?? true,
    };

    const form = useForm<RouteFormValues>({
        resolver: zodResolver(routeSchema),
        defaultValues,
    });

    useEffect(() => {
            const resetValues: RouteFormValues = defaultValues;
            form.reset(resetValues);
    }, [initialData, form]);


    const handleSubmit = async (data: RouteFormValues) => {
        const updateData = isEditMode && initialData ? { ...initialData, ...data } : data;
        await onSave(updateData);
        onClose()
    };

    return (
        <SheetContent className="sm:max-w-lg p-4">
            <SheetHeader>
                <SheetTitle>{isEditMode ? 'Edit Route' : 'Add New Route'}</SheetTitle>
                <SheetDescription>
                    {isEditMode ? "Update the route's details." : 'Enter details for the new route.'}
                </SheetDescription>
            </SheetHeader>
            <Form {...form}>
                <form onSubmit={form.handleSubmit(handleSubmit)} className="space-y-6 py-6">

                    <FormField
                        control={form.control}
                        name="originAirportId"
                        render={({ field }) => (
                            <FormItem className="flex flex-col">
                                <FormLabel>Origin Airport</FormLabel>
                                <FormControl>
                                    <AirportCombobox
                                        airports={airports.filter(a => a.airportId !== form.getValues('destinationAirportId'))}
                                        value={field.value || undefined}
                                        onChange={(value) => field.onChange(value || 0)}
                                        placeholder="Select origin..."
                                    />
                                </FormControl>
                                <FormMessage />
                            </FormItem>
                        )}
                    />

                    <FormField
                        control={form.control}
                        name="destinationAirportId"
                        render={({ field }) => (
                            <FormItem className="flex flex-col">
                                <FormLabel>Destination Airport</FormLabel>
                                <FormControl>
                                    <AirportCombobox
                                        airports={airports.filter(a => a.airportId !== form.getValues('originAirportId'))}
                                        value={field.value || undefined}
                                        onChange={(value) => field.onChange(value || 0)}
                                        placeholder="Select destination..."
                                    />
                                </FormControl>
                                <FormMessage />
                            </FormItem>
                        )}
                    />

                    <FormField
                        control={form.control}
                        name="distanceKm"
                        render={({ field }) => (
                            <FormItem>
                                <FormLabel>Distance (km)</FormLabel>
                                <FormControl>
                                    <Input
                                        type="number"
                                        placeholder="e.g., 850"
                                        value={field.value || ''}
                                        onChange={(e) => {
                                            const value = e.target.value;
                                            field.onChange(value === '' ? 0 : parseFloat(value) || 0);
                                        }}
                                    />
                                </FormControl>
                                <FormMessage />
                            </FormItem>
                        )}
                    />

                    <FormField
                        control={form.control}
                        name="estimatedDurationMinutes"
                        render={({ field }) => (
                            <FormItem>
                                <FormLabel>Estimated Duration (minutes)</FormLabel>
                                <FormControl>
                                    <Input
                                        type="number"
                                        placeholder="e.g., 90"
                                        value={field.value || ''}
                                        onChange={(e) => {
                                            const value = e.target.value;
                                            field.onChange(value === '' ? 0 : parseInt(value) || 0);
                                        }}
                                    />
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
                    <SheetFooter className="mt-8">
                        <SheetClose asChild>
                            <Button type="button" variant="outline" onClick={onClose}>
                                Cancel
                            </Button>
                        </SheetClose>
                        <Button type="submit" disabled={isLoading}>
                            {isLoading ? 'Saving...' : 'Save Changes'}
                        </Button>
                    </SheetFooter>
                </form>
            </Form>
        </SheetContent>
    );
};