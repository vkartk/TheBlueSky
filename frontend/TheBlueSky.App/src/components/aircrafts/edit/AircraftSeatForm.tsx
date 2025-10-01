import { useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import * as z from 'zod';
import type { AircraftSeat } from '@/types/aircraftSeat';
import type { SeatClass } from '@/types/seatClass';
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
    seatClasses: SeatClass[];
    isSaving: boolean;
}

const SEAT_POSITIONS = ['Window', 'Aisle', 'Middle'];

const formSchema = z.object({
    seatNumber: z.string().min(2, 'Required').max(4, 'Too long'),
    seatClassId: z.number().min(1, 'Please select a class'),
    seatPosition: z.string().refine((val) => SEAT_POSITIONS.includes(val), {
        message: 'Please select a valid position',
    }),
    additionalFare: z.number().min(0, 'Fare must be a positive number'),
    isActive: z.boolean(),
});

type SeatFormValues = z.infer<typeof formSchema>;

export function AircraftSeatForm({
    isOpen,
    onClose,
    onSave,
    seatData,
    seatClasses,
    isSaving,
}: AircraftSeatFormProps) {

    const isEditing = !!seatData.aircraftSeatId;

    const form = useForm<SeatFormValues>({
        resolver: zodResolver(formSchema),
    });

    useEffect(() => {
        if (isOpen) {
            form.reset({
                seatNumber: seatData.seatNumber || '',
                seatClassId: seatData.seatClassId || 0,
                seatPosition: seatData.seatPosition || '',
                additionalFare: seatData.additionalFare || 0,
                isActive: seatData.isActive === undefined ? true : seatData.isActive,
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
                                <Input value={seatData.seatRow || ''} disabled />
                            </FormItem>
                            <FormItem>
                                <FormLabel>Column</FormLabel>
                                <Input value={seatData.seatColumn || ''} disabled />
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
                            name="seatClassId"
                            render={({ field }) => (
                                <FormItem>
                                    <FormLabel>Seat Class</FormLabel>
                                    <Select
                                        onValueChange={(value) => field.onChange(Number(value))}
                                        defaultValue={field.value?.toString()}
                                    >
                                        <FormControl>
                                            <SelectTrigger className="w-full">
                                                <SelectValue placeholder="Select a class" />
                                            </SelectTrigger>
                                        </FormControl>
                                        <SelectContent>
                                            {seatClasses.map((sc) => (
                                                <SelectItem key={sc.seatClassId} value={sc.seatClassId.toString()}>
                                                    {sc.className}
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
                                    <Select
                                        value={field.value ?? undefined}
                                        onValueChange={field.onChange}
                                    >
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
                            name="isActive"
                            render={({ field }) => (
                                <FormItem className="flex flex-row items-center justify-between rounded-lg border p-3">
                                    <div className="space-y-0.5">
                                        <FormLabel>Active</FormLabel>
                                        <FormDescription>
                                            Inactive seats are not available for booking.
                                        </FormDescription>
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