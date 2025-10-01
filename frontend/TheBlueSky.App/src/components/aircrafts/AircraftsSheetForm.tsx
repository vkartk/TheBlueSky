import { zodResolver } from '@hookform/resolvers/zod';
import { useForm, useWatch } from 'react-hook-form';
import * as z from 'zod';
import { useMemo } from 'react';

import { Button } from '@/components/ui/button';
import { Checkbox } from '@/components/ui/checkbox';
import {
    Form,
    FormControl,
    FormField,
    FormItem,
    FormLabel,
    FormMessage,
} from '@/components/ui/form';
import { Input } from '@/components/ui/input';
import {
    Select,
    SelectContent,
    SelectItem,
    SelectTrigger,
    SelectValue,
} from '@/components/ui/select';
import {
    SheetContent,
    SheetHeader,
    SheetTitle,
    SheetFooter,
    SheetClose,
} from '@/components/ui/sheet';

import {
    type NewAircraft,
    AIRCRAFT_MANUFACTURERS,
    MODELS_BY_MANUFACTURER,
    ALL_AIRCRAFT_MODELS,
    type AircraftModel,
} from '@/types/aircraft';

const formSchema = z.object({
    aircraftName: z.string().min(1, 'Aircraft name is required.').max(100),
    manufacturer: z.enum(AIRCRAFT_MANUFACTURERS),
    aircraftModel: z.string().refine((val) => ALL_AIRCRAFT_MODELS.includes(val as AircraftModel), {
        message: 'Please select a valid model.',
    }),
    isActive: z.boolean(),
});

type AircraftFormValues = z.infer<typeof formSchema>;

interface AircraftsSheetFormProps {
    onSave: (data: NewAircraft) => Promise<void>;
    onClose: () => void;
    isLoading: boolean;
}

export const AircraftsSheetForm = ({ onSave, onClose, isLoading }: AircraftsSheetFormProps) => {
    const form = useForm<AircraftFormValues>({
        resolver: zodResolver(formSchema),
        defaultValues: {
            aircraftName: '',
            manufacturer: 'Boeing',
            aircraftModel: '',
            isActive: true,
        },
    });

    const selectedManufacturer = useWatch({
        control: form.control,
        name: 'manufacturer',
    });

    const filteredModels = useMemo(() => {
        return MODELS_BY_MANUFACTURER[selectedManufacturer] || [];
    }, [selectedManufacturer]);

    const handleSubmit = async (data: AircraftFormValues) => {
        const dataToSave: NewAircraft = {
            ...data,
            economySeats: 0,
            businessSeats: 0,
            firstClassSeats: 0,
        };
        await onSave(dataToSave);
    };

    return (
        <SheetContent className="sm:max-w-lg p-4">
            <SheetHeader>
                <SheetTitle>Create New Aircraft</SheetTitle>
            </SheetHeader>
            <Form {...form}>
                <form onSubmit={form.handleSubmit(handleSubmit)} className="space-y-4 py-4">
                    <FormField
                        control={form.control}
                        name="aircraftName"
                        render={({ field }) => (
                            <FormItem>
                                <FormLabel>Aircraft Name</FormLabel>
                                <FormControl>
                                    <Input placeholder="e.g., Spirit of St. Louis" {...field} />
                                </FormControl>
                                <FormMessage />
                            </FormItem>
                        )}
                    />

                    <div className="grid grid-cols-2 gap-4">
                        <FormField
                            control={form.control}
                            name="manufacturer"
                            render={({ field }) => (
                                <FormItem>
                                    <FormLabel>Manufacturer</FormLabel>
                                    <Select onValueChange={field.onChange} value={field.value}>
                                        <FormControl>
                                            <SelectTrigger className="w-full">
                                                <SelectValue placeholder="Select..." />
                                            </SelectTrigger>
                                        </FormControl>
                                        <SelectContent>
                                            {AIRCRAFT_MANUFACTURERS.map((manufacturer) => (
                                                <SelectItem key={manufacturer} value={manufacturer}>
                                                    {manufacturer}
                                                </SelectItem>
                                            ))}
                                        </SelectContent>
                                    </Select>
                                    <FormMessage />
                                </FormItem>
                            )}
                        />
                        <FormField
                            control={form.control}
                            name="aircraftModel"
                            render={({ field }) => (
                                <FormItem>
                                    <FormLabel>Aircraft Model</FormLabel>
                                    <Select
                                        onValueChange={field.onChange}
                                        value={field.value}
                                        disabled={filteredModels.length === 0}
                                    >
                                        <FormControl>
                                            <SelectTrigger className="w-full">
                                                <SelectValue placeholder="Select..." />
                                            </SelectTrigger>
                                        </FormControl>
                                        <SelectContent>
                                            {filteredModels.map((model) => (
                                                <SelectItem key={model} value={model}>
                                                    {model}
                                                </SelectItem>
                                            ))}
                                        </SelectContent>
                                    </Select>
                                    <FormMessage />
                                </FormItem>
                            )}
                        />
                    </div>

                    <FormField
                        control={form.control}
                        name="isActive"
                        render={({ field }) => (
                            <FormItem className="flex flex-row items-start space-x-3 space-y-0 rounded-md border p-4">
                                <FormControl>
                                    <Checkbox checked={field.value} onCheckedChange={field.onChange} />
                                </FormControl>
                                <div className="space-y-1 leading-none">
                                    <FormLabel>Active</FormLabel>
                                </div>
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
                            {isLoading ? 'Creating...' : 'Create Aircraft'}
                        </Button>
                    </SheetFooter>
                </form>
            </Form>
        </SheetContent>
    );
};