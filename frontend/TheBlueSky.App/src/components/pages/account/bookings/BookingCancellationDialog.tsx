import { useEffect } from 'react';
import { useForm } from 'react-hook-form';
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

type FormValues = {
  refundAmount: number;
  cancellationReason: string;
};

export const BookingCancellationDialog = ({
  isOpen,
  onClose,
  onSubmit,
  booking,
  isLoading,
}: BookingCancellationDialogProps) => {
  const form = useForm<FormValues>({
    mode: 'onChange',
    defaultValues: {
      refundAmount: booking?.totalAmount ?? 0,
      cancellationReason: '',
    },
  });

  useEffect(() => {
    form.reset({
      refundAmount: booking?.totalAmount ?? 0,
      cancellationReason: '',
    });
  }, [booking, form]);

  const handleSubmit = async () => {
    const isValid = await form.trigger();
    if (!isValid || !booking) return;

    const values = form.getValues();

    onSubmit({
      bookingId: booking.bookingId,
      refundAmount: Number(values.refundAmount) || 0,
      cancellationReason: values.cancellationReason?.trim() || 'Not provided by user.',
    });
  };

  return (
    <AlertDialog open={isOpen} onOpenChange={(open) => !open && onClose()}>
      <AlertDialogContent>
        <AlertDialogHeader>
          <AlertDialogTitle>Are you sure you want to cancel?</AlertDialogTitle>
          <AlertDialogDescription>
            This will cancel booking #{booking?.bookingId}. Please confirm the details below.
            This action cannot be undone.
          </AlertDialogDescription>
        </AlertDialogHeader>

        <Form {...form}>
          <form className="space-y-4" onSubmit={(e) => e.preventDefault()}>
            <FormField
              control={form.control}
              name="refundAmount"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Refund Amount</FormLabel>
                  <FormControl>
                    <Input type="number" value={field.value} disabled />
                  </FormControl>
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
              <AlertDialogCancel type="button">Back</AlertDialogCancel>
              <AlertDialogAction
                type="button"
                disabled={isLoading}
                onClick={handleSubmit}
              >
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