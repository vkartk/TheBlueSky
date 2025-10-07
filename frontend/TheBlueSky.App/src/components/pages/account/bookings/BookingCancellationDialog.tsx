import { useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { Loader2 } from 'lucide-react';
import type { Booking } from '@/types/booking';

import {
    AlertDialog,
    AlertDialogAction,
    AlertDialogCancel,
    AlertDialogContent,
    AlertDialogDescription,
    AlertDialogFooter,
    AlertDialogHeader,
    AlertDialogTitle,
} from '@/components/ui/alert-dialog';
import {
    Form,
    FormControl,
    FormField,
    FormItem,
    FormLabel,
    FormMessage,
} from '@/components/ui/form';
import { Input } from '@/components/ui/input';
import { Textarea } from '@/components/ui/textarea';

export interface CancellationFormData {
    bookingId: number;
    refundAmount: number;
    cancellationReason?: string;
}

interface BookingCancellationDialogProps {
    isOpen: boolean;
    onClose: () => void;
    onSubmit: (data: CancellationFormData) => void;
    booking: Booking | null;
    isLoading: boolean;
}

const formSchema = z.object({
    refundAmount: z.number().min(0, 'Refund amount cannot be negative.'),
    cancellationReason: z.string().optional(),
});

type DialogFormData = z.infer<typeof formSchema>;

export const BookingCancellationDialog = ({
    isOpen,
    onClose,
    onSubmit,
    booking,
    isLoading,
}: BookingCancellationDialogProps) => {

    const form = useForm<DialogFormData>({
        resolver: zodResolver(formSchema),
    });

    useEffect(() => {
        if (booking) {
            form.reset({
                refundAmount: booking.totalAmount,
                cancellationReason: '',
            });
        }
    }, [booking, form]);

    const handleSubmit = (values: DialogFormData) => {
        if (!booking) return;

        const formData: CancellationFormData = {
            bookingId: booking.bookingId,
            refundAmount: values.refundAmount,
            cancellationReason: values.cancellationReason || 'Not provided by user.',
        };
        onSubmit(formData);
    };

    return (
        <AlertDialog open={isOpen} onOpenChange={onClose}>
            <AlertDialogContent>
                <AlertDialogHeader>
                    <AlertDialogTitle>Are you sure you want to cancel?</AlertDialogTitle>
                    <AlertDialogDescription>
                        This will cancel booking #{booking?.bookingId}. Please confirm the
                        details below. This action cannot be undone.
                    </AlertDialogDescription>
                </AlertDialogHeader>
                <Form {...form}>
                    <form onSubmit={form.handleSubmit(handleSubmit)} className="space-y-4">
                        <FormField
                            control={form.control}
                            name="refundAmount"
                            render={({ field }) => (
                                <FormItem>
                                    <FormLabel>Refund Amount</FormLabel>
                                    <FormControl>
                                        <Input type="number" {...field} />
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
                                    <FormLabel>Reason for Cancellation (Optional)</FormLabel>
                                    <FormControl>
                                        <Textarea placeholder="Let us know why you're canceling..." {...field} />
                                    </FormControl>
                                    <FormMessage />
                                </FormItem>
                            )}
                        />
                        <AlertDialogFooter className="pt-4">
                            <AlertDialogCancel>Back</AlertDialogCancel>
                            <AlertDialogAction type="submit" disabled={isLoading}>
                                {isLoading && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
                                Confirm Cancellation
                            </AlertDialogAction>
                        </AlertDialogFooter>
                    </form>
                </Form>
            </AlertDialogContent>
        </AlertDialog>
    );
};