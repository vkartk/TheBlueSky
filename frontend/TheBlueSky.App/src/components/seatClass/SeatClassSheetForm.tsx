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
import { Textarea } from '@/components/ui/textarea';
import { SheetContent, SheetHeader, SheetTitle, SheetFooter, SheetDescription, SheetClose } from '@/components/ui/sheet';
import type { SeatClass } from '@/types/seatClass';
import { isPriorityAvailable } from '@/utils/seatClass';

const formSchema = z.object({
    className: z.string()
        .min(1, { message: 'Class name is required.' })
        .max(50, { message: 'Class name must be 50 characters or less.' }),
    classDescription: z.string()
        .max(255, { message: 'Description must be 255 characters or less.' })
        .nullable(),
    priorityOrder: z.number()
        .min(1, { message: 'Priority must be at least 1.' })
        .multipleOf(1, { message: 'Priority must be a whole number.' })
});

type SeatClassFormValues = z.infer<typeof formSchema>;

interface SeatClassSheetFormProps {
    initialData?: SeatClass | null;
    onSave: (data: SeatClassFormValues) => Promise<void>;
    onClose: () => void;
    isLoading: boolean;
    seatClasses: SeatClass[];
}

export const SeatClassSheetForm = ({ initialData, onSave, onClose, isLoading, seatClasses }: SeatClassSheetFormProps) => {

    const isEditMode = !!initialData;

    const usedPriorities = seatClasses
        .filter(sc => !initialData || sc.seatClassId !== initialData.seatClassId)
        .map(sc => sc.priorityOrder);

    const defaultValues = {
        className: initialData?.className ?? '',
        classDescription: initialData?.classDescription ?? '',
        priorityOrder: initialData?.priorityOrder ?? 0,
    };

    const form = useForm<SeatClassFormValues>({
        resolver: zodResolver(formSchema),
        defaultValues,
    });

    useEffect(() => {
        form.reset(defaultValues);
    }, [initialData, form]);

    const handleSubmit = async (data: SeatClassFormValues) => {

        const original = initialData?.priorityOrder;

        if (!isPriorityAvailable(data.priorityOrder, usedPriorities, original)) {
            form.setError('priorityOrder', { type: 'manual', message: 'This priority order is already in use.' });
            return;
        }
        await onSave(data);
    };

    return (
        <SheetContent className="sm:max-w-lg p-4">
            <SheetHeader>
                <SheetTitle>{isEditMode ? 'Edit Seat Class' : 'Create New Seat Class'}</SheetTitle>
                <SheetDescription>
                    {isEditMode ? "Update the seat class's details." : 'Enter details for the new seat class.'}
                </SheetDescription>
            </SheetHeader>
            <Form {...form}>
                <form onSubmit={form.handleSubmit(handleSubmit)} className="space-y-4 py-4">
                    <FormField
                        control={form.control}
                        name="className"
                        render={({ field }) => (
                            <FormItem>
                                <FormLabel>Class Name</FormLabel>
                                <FormControl>
                                    <Input placeholder="e.g., Business Class" {...field} />
                                </FormControl>
                                <FormMessage />
                            </FormItem>
                        )}
                    />

                    <FormField
                        control={form.control}
                        name="classDescription"
                        render={({ field }) => (
                            <FormItem>
                                <FormLabel>Description (Optional)</FormLabel>
                                <FormControl>
                                    <Textarea placeholder="e.g., Extra legroom and premium meals" {...field} value={field.value ?? ''} rows={6} />
                                </FormControl>
                                <FormMessage />
                            </FormItem>
                        )}
                    />

                    <FormField
                        control={form.control}
                        name="priorityOrder"
                        render={({ field }) => (
                            <FormItem>
                                <FormLabel>Priority Order</FormLabel>
                                <FormControl>
                                    <Input
                                        type="number"
                                        placeholder="e.g., 1"
                                        value={field.value || ''}
                                        onChange={(e) => {
                                            const value = e.target.value;
                                            const intValue = value === '' ? 0 : parseInt(value, 10);
                                            if(!isPriorityAvailable(intValue, usedPriorities, initialData?.priorityOrder)) {
                                                form.setError('priorityOrder', { type: 'manual', message: 'This priority order is already in use.' });
                                            } else {
                                                form.clearErrors('priorityOrder');
                                            }
                                            field.onChange(intValue || 0);
                                        }}
                                    />
                                </FormControl>
                                <FormMessage />
                            </FormItem>
                        )}
                    />

                    <SheetFooter className="pt-4">
                        <SheetClose asChild>
                            <Button type="button" variant="outline" onClick={onClose} disabled={isLoading}>
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