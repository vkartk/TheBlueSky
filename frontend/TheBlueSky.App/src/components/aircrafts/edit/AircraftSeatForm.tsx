import { useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import * as z from 'zod';

import type { AircraftSeat, SeatClass } from '@/types/aircraftSeat';
import { SeatClasses } from '@/types/aircraftSeat';

import { Button } from '@/components/ui/button';
import {
    Dialog,
    DialogContent,
    DialogDescription,
    DialogFooter,
    DialogHeader,
    DialogTitle,
} from '@/components/ui/dialog';
import {
    Form,
    FormControl,
    FormDescription,
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
import { Switch } from '@/components/ui/switch';

interface AircraftSeatFormProps {
    isOpen: boolean;
    onClose: () => void;
    onSave: (data: Partial<AircraftSeat>) => void;
    seatData: Partial<AircraftSeat>;
    isSaving: boolean;
}

const SEAT_POSITIONS = ['Window', 'Aisle', 'Middle'] as const;

const formSchema = z.object({
    seatNumber: z.string().min(2, 'Required').max(4, 'Too long'),
    seatClass: z.enum(SeatClasses),
    seatPosition: z.enum(SEAT_POSITIONS),
    additionalFare: z.number().min(0, 'Fare must be a positive number'),
    isActive: z.boolean(),
});

type SeatFormValues = z.infer<typeof formSchema>;

export function AircraftSeatForm({
    isOpen,
    onClose,
    onSave,
    seatData,
    isSaving,
}: AircraftSeatFormProps) {
    const isEditing = !!seatData.aircraftSeatId;

    const form = useForm<SeatFormValues>({
        resolver: zodResolver(formSchema),
        defaultValues: {
            seatNumber: '',
            seatClass: 'Economy',
            seatPosition: undefined as unknown as SeatFormValues['seatPosition'],
            additionalFare: 0,
            isActive: true,
        },
    });

    useEffect(() => {
        if (isOpen) {
            form.reset({
                seatNumber: seatData.seatNumber ?? '',
                seatClass: (seatData.seatClass as SeatClass) ?? 'Economy',
                seatPosition: (seatData.seatPosition as SeatFormValues['seatPosition']) ?? SEAT_POSITIONS[0],
                additionalFare: seatData.additionalFare ?? 0,
                isActive: seatData.isActive ?? true,
            });
        }
    }, [isOpen, seatData, form]);

    const handleSubmit = (values: SeatFormValues) => {
        onSave({
            ...seatData,
            ...values,
        });
    };

    return (
        <Dialog open={isOpen} onOpenChange={onClose}>
            <DialogContent className="sm:max-w-[425px]">
                <DialogHeader>
                    <DialogTitle>{isEditing ? 'Edit Seat' : 'Add New Seat'}</DialogTitle>
                    <DialogDescription>Configure all details for this seat.</DialogDescription>
                </DialogHeader>

                <Form {...form}>
                    <form onSubmit={form.handleSubmit(handleSubmit)} className="space-y-4 py-4">
                        <div className="grid grid-cols-2 gap-4">
                            <FormItem>
                                <FormLabel>Row</FormLabel>
                                <Input value={seatData.seatRow ?? ''} disabled />
                            </FormItem>
                            <FormItem>
                                <FormLabel>Column</FormLabel>
                                <Input value={seatData.seatColumn ?? ''} disabled />
                            </FormItem>
                        </div>

                        <FormField
                            control={form.control}
                            name="seatNumber"
                            render={({ field }) => (
                                <FormItem>
                                    <FormLabel>Seat Number</FormLabel>
                                    <FormControl>
                                        <Input placeholder="e.g., 24A" {...field} />
                                    </FormControl>
                                    <FormMessage />
                                </FormItem>
                            )}
                        />

                        <FormField
                            control={form.control}
                            name="seatClass"
                            render={({ field }) => (
                                <FormItem>
                                    <FormLabel>Seat Class</FormLabel>
                                    <Select value={field.value} onValueChange={field.onChange}>
                                        <FormControl>
                                            <SelectTrigger className="w-full">
                                                <SelectValue placeholder="Select a class" />
                                            </SelectTrigger>
                                        </FormControl>
                                        <SelectContent>
                                            {SeatClasses.map((sc) => (
                                                <SelectItem key={sc} value={sc}>
                                                    {sc}
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
                            name="seatPosition"
                            render={({ field }) => (
                                <FormItem>
                                    <FormLabel>Position</FormLabel>
                                    <Select value={field.value} onValueChange={field.onChange}>
                                        <FormControl>
                                            <SelectTrigger className="w-full">
                                                <SelectValue placeholder="Select position" />
                                            </SelectTrigger>
                                        </FormControl>
                                        <SelectContent>
                                            {SEAT_POSITIONS.map((p) => (
                                                <SelectItem key={p} value={p}>
                                                    {p}
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
                            name="additionalFare"
                            render={({ field }) => (
                                <FormItem>
                                    <FormLabel>Additional Fare</FormLabel>
                                    <FormControl>
                                        <Input
                                            type="number"
                                            min={0}
                                            step="0.01"
                                            inputMode="decimal"
                                            value={Number.isFinite(field.value as number) ? field.value : ''}
                                            onChange={(e) => {
                                                const v = e.target.value;
                                                field.onChange(v === '' ? '' : Number(v));
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
                                <FormItem className="flex flex-row items-center justify-between rounded-lg border p-3">
                                    <div className="space-y-0.5">
                                        <FormLabel>Active</FormLabel>
                                        <FormDescription>Inactive seats are not available for booking.</FormDescription>
                                    </div>
                                    <FormControl>
                                        <Switch checked={field.value} onCheckedChange={field.onChange} />
                                    </FormControl>
                                </FormItem>
                            )}
                        />

                        <DialogFooter>
                            <Button type="button" variant="ghost" onClick={onClose}>
                                Cancel
                            </Button>
                            <Button type="submit" disabled={isSaving}>
                                {isSaving ? 'Saving...' : 'Save Seat'}
                            </Button>
                        </DialogFooter>
                    </form>
                </Form>
            </DialogContent>
        </Dialog>
    );
}
