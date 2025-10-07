import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { useEffect } from 'react';

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
import { Textarea } from '@/components/ui/textarea';

import type { Booking } from '@/types/booking';

const cancelFormSchema = z.object({
    refundAmount: z.number().min(0, 'Refund amount cannot be negative.'),
    cancellationReason: z.string().min(10, 'Reason must be at least 10 characters long.'),
    adminNotes: z.string().optional(),
});

type CancelFormValues = z.infer<typeof cancelFormSchema>;

interface BookingCancellationFormProps {
    isOpen: boolean;
    onClose: () => void;
    onSubmit: (data: CancelFormValues) => void;
    booking: Booking | null;
    currentUserRole: 'Admin' | 'User';
}

export const BookingCancellationForm = ({
    isOpen,
    onClose,
    onSubmit,
    booking,
    currentUserRole,
}: BookingCancellationFormProps) => {
    
    const form = useForm<z.input<typeof cancelFormSchema>>({
        resolver: zodResolver(cancelFormSchema),
        defaultValues: {
            refundAmount: 0,
            cancellationReason: '',
            adminNotes: '',
        },
    });

    useEffect(() => {
        if (booking) {
            form.setValue('refundAmount', booking.totalAmount);
        } else {
            form.reset();
        }
    }, [booking, form]);

    const handleFormSubmit = (values: CancelFormValues) => {
        onSubmit(values);
        onClose();
    };

    return (
        <Dialog open={isOpen} onOpenChange={onClose}>
            <DialogContent className="sm:max-w-[425px]">
                <DialogHeader>
                    <DialogTitle>Cancel Booking #{booking?.bookingId}</DialogTitle>
                    <DialogDescription>
                        Provide the details for this cancellation. Click confirm when done.
                    </DialogDescription>
                </DialogHeader>
                <Form {...form}>
                    <form onSubmit={form.handleSubmit(handleFormSubmit)} className="space-y-4 pt-2">
                        <FormField
                            control={form.control}
                            name="refundAmount"
                            render={({ field }) => (
                                <FormItem>
                                    <FormLabel>Refund Amount (INR)</FormLabel>
                                    <FormControl>
                                        <Input type="number"
                                            value={String(field.value) ?? ''}
                                            onChange={(e) => {
                                                const value = e.target.value;
                                                field.onChange(value === '' ? 0 : parseInt(value) || 0);
                                            }} placeholder="5500" />
                                    </FormControl>
                                    <FormMessage />
                                </FormItem>
                            )}
                        />
                        <FormField
                            control={form.control}
                            name="cancellationReason"
                            render={({ field }) => (
                                <FormItem>
                                    <FormLabel>Reason for Cancellation</FormLabel>
                                    <FormControl>
                                        <Textarea placeholder="e.g., Customer requested..." {...field} />
                                    </FormControl>
                                    <FormDescription>This reason will be visible to the customer.</FormDescription>
                                    <FormMessage />
                                </FormItem>
                            )}
                        />
                        {currentUserRole === 'Admin' && (
                            <FormField
                                control={form.control}
                                name="adminNotes"
                                render={({ field }) => (
                                    <FormItem>
                                        <FormLabel>Admin Notes (Internal)</FormLabel>
                                        <FormControl>
                                            <Textarea placeholder="e.g., Processed refund via..." {...field} />
                                        </FormControl>
                                        <FormDescription>These notes are for internal records only.</FormDescription>
                                        <FormMessage />
                                    </FormItem>
                                )}
                            />
                        )}
                        <DialogFooter className="pt-4">
                            <Button type="button" variant="outline" onClick={onClose}>
                                Cancel
                            </Button>
                            <Button type="submit">Confirm Cancellation</Button>
                        </DialogFooter>
                    </form>
                </Form>
            </DialogContent>
        </Dialog>
    );
};