import { zodResolver } from '@hookform/resolvers/zod';
import { useForm } from 'react-hook-form';
import { z } from 'zod';
import { Button } from '@/components/ui/button';
import {
    Form,
    FormControl,
    FormField,
    FormItem,
    FormLabel,
    FormMessage,
} from '@/components/ui/form';
import {
    SheetContent,
    SheetDescription,
    SheetHeader,
    SheetTitle,
    SheetFooter,
    SheetClose,
} from '@/components/ui/sheet';
import {
    Select,
    SelectContent,
    SelectItem,
    SelectTrigger,
    SelectValue,
} from '@/components/ui/select';
import { Input } from '@/components/ui/input';
import type { Flight } from '@/types/flight';
import { flightStatuses } from '@/types/flight';

interface FlightSheetFormProps {
    initialData: Flight | null;
    onSave: (data: Flight) => void;
    onClose: () => void;
    isLoading: boolean;
}

const formSchema = z.object({
    departureDateTime: z.string().min(1, 'Departure date and time is required.'),
    arrivalDateTime: z.string().min(1, 'Arrival date and time is required.'),
    flightStatus: z.enum(flightStatuses),
});

type FormValues = z.infer<typeof formSchema>;

const toLocalISOString = (date: Date) => {
    const tzoffset = date.getTimezoneOffset() * 60000;
    const localISOTime = new Date(date.getTime() - tzoffset)
        .toISOString()
        .slice(0, 16);
    return localISOTime;
};

export const FlightSheetForm = ({
    initialData,
    onSave,
    onClose,
    isLoading,
}: FlightSheetFormProps) => {

    const departureDateTime = initialData ? toLocalISOString(new Date(initialData.departureDateTime)) : '';
    const arrivalDateTime = initialData ? toLocalISOString(new Date(initialData.arrivalDateTime)) : '';

    const form = useForm<FormValues>({
        resolver: zodResolver(formSchema),
        defaultValues: {
            departureDateTime: departureDateTime,
            arrivalDateTime: arrivalDateTime,
            flightStatus: initialData?.flightStatus ?? 'Scheduled',
        },
    });

    const onSubmit = (values: FormValues) => {
        if (!initialData) return;

        const data: Flight = {
            ...initialData,
            departureDateTime: new Date(values.departureDateTime).toISOString(),
            arrivalDateTime: new Date(values.arrivalDateTime).toISOString(),
            flightStatus: values.flightStatus,
        };
        onSave(data);
    };

    return (
        <SheetContent className="sm:max-w-lg p-4">
            <SheetHeader>
                <SheetTitle>Edit Flight #{initialData?.flightId}</SheetTitle>
                <SheetDescription>
                    Update the status and times for this flight. Click save when you're
                    done.
                </SheetDescription>
            </SheetHeader>
            <Form {...form}>
                <form
                    onSubmit={form.handleSubmit(onSubmit)}
                    className="space-y-8 mt-4"
                >
                    <FormField
                        control={form.control}
                        name="departureDateTime"
                        render={({ field }) => (
                            <FormItem>
                                <FormLabel>Departure Date & Time</FormLabel>
                                <FormControl>
                                    <Input type="datetime-local" {...field} />
                                </FormControl>
                                <FormMessage />
                            </FormItem>
                        )}
                    />
                    <FormField
                        control={form.control}
                        name="arrivalDateTime"
                        render={({ field }) => (
                            <FormItem>
                                <FormLabel>Arrival Date & Time</FormLabel>
                                <FormControl>
                                    <Input type="datetime-local" {...field} />
                                </FormControl>
                                <FormMessage />
                            </FormItem>
                        )}
                    />
                    <FormField
                        control={form.control}
                        name="flightStatus"
                        render={({ field }) => (
                            <FormItem>
                                <FormLabel>Flight Status</FormLabel>
                                <Select
                                    onValueChange={field.onChange}
                                    defaultValue={field.value}
                                >
                                    <FormControl>
                                        <SelectTrigger className="w-full">
                                            <SelectValue placeholder="Select a status" />
                                        </SelectTrigger>
                                    </FormControl>
                                    <SelectContent>
                                        {flightStatuses.map((status) => (
                                            <SelectItem key={status} value={status}>
                                                {status}
                                            </SelectItem>
                                        ))}
                                    </SelectContent>
                                </Select>
                                <FormMessage />
                            </FormItem>
                        )}
                    />
                    <SheetFooter>
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