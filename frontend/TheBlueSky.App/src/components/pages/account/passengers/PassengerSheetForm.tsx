import { useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { format } from 'date-fns';
import { CalendarIcon, Loader2 } from 'lucide-react';

import { cn } from '@/lib/utils';
import { Button } from '@/components/ui/button';
import { Calendar } from '@/components/ui/calendar';
import { Checkbox } from '@/components/ui/checkbox';
import { Form, FormControl, FormDescription, FormField, FormItem, FormLabel, FormMessage } from '@/components/ui/form';
import { Input } from '@/components/ui/input';
import { Popover, PopoverContent, PopoverTrigger } from '@/components/ui/popover';
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select';
import { Sheet, SheetContent, SheetDescription, SheetHeader, SheetTitle, SheetFooter, SheetClose } from '@/components/ui/sheet';

import type { Passenger, NewPassenger } from '@/types/passenger';
import type { Country } from '@/types/country';
import type { User } from '@/types/auth';

interface PassengerSheetFormProps {
    initialData?: Passenger | null;
    onSave: (data: NewPassenger | Passenger) => Promise<void>;
    onClose: () => void;
    isLoading: boolean;
    isOpen: boolean;
    currentUser: User | null;
    countries: Country[];
}

const formSchema = z.object({
    firstName: z.string().min(2, 'First name must be at least 2 characters.'),
    lastName: z.string().min(2, 'Last name must be at least 2 characters.'),
    dateOfBirth: z.date(),
    gender: z.string().min(1, 'Gender is required.'),
    passportNumber: z.string().min(6, 'A valid passport number is required.'),
    nationalityCountryId: z.string().min(1, 'Nationality is required.'),
    relationshipToManager: z.string().min(1, 'Relationship is required.'),
    isActive: z.boolean(),
});

type FormValues = z.infer<typeof formSchema>;

export function PassengerSheetForm({
    initialData,
    onSave,
    onClose,
    isLoading,
    isOpen,
    currentUser,
    countries,
}: PassengerSheetFormProps) {


    const isEditMode = !!initialData;
    const title = isEditMode ? 'Edit Passenger' : 'Add New Passenger';
    const description = isEditMode
        ? 'Update the details for this passenger.'
        : 'Fill in the form to add a new passenger.';
    const actionLabel = isEditMode ? 'Save Changes' : 'Create Passenger';


    const defaultValues = {
        firstName: initialData?.firstName ?? '',
        lastName: initialData?.lastName ?? '',
        dateOfBirth: initialData ? new Date(initialData.dateOfBirth) : undefined,
        gender: initialData?.gender ?? '',
        passportNumber: initialData?.passportNumber ?? '',
        nationalityCountryId: initialData?.nationalityCountryId ?? '',
        relationshipToManager: initialData?.relationshipToManager ?? 'Other',
        isActive: initialData?.isActive ?? true,
    };
    const form = useForm<FormValues>({
        resolver: zodResolver(formSchema),
        defaultValues
    });

    const relationship = form.watch('relationshipToManager');
    const isMyself = relationship === 'Myself';

    useEffect(() => {

        form.reset(defaultValues)

        if (!currentUser) return;

        if (isMyself) {
            form.setValue('firstName', currentUser.firstName, { shouldValidate: true });
            form.setValue('lastName', currentUser.lastName, { shouldValidate: true });
        }
    }, [isMyself, currentUser, form, initialData]);

    const onSubmit = async (values: FormValues) => {

        const userId = currentUser?.userId

        if (!userId) return;

        const baseData = {
            ...values,
            dateOfBirth: format(values.dateOfBirth, 'yyyy-MM-dd'),
            managedByUserId: userId,
        };

        if (isEditMode) {
            await onSave({ ...initialData, ...baseData });
        } else {
            await onSave(baseData);
        }
    };

    return (
        <Sheet open={isOpen} onOpenChange={onClose}>
            <SheetContent className="sm:max-w-lg p-4">
                <SheetHeader>
                    <SheetTitle>{title}</SheetTitle>
                    <SheetDescription>{description}</SheetDescription>
                </SheetHeader>
                <Form {...form}>
                    <form
                        onSubmit={form.handleSubmit(onSubmit)}
                        className="space-y-6 pt-6"
                    >
                        <FormField
                            control={form.control}
                            name="relationshipToManager"
                            render={({ field }) => (
                                <FormItem>
                                    <FormLabel>Relationship to You</FormLabel>
                                    <Select onValueChange={field.onChange} defaultValue={field.value} disabled={isMyself}>
                                        <FormControl>
                                            <SelectTrigger>
                                                <SelectValue placeholder="Select a relationship" />
                                            </SelectTrigger>
                                        </FormControl>
                                        <SelectContent>
                                            <SelectItem value="Myself">Myself</SelectItem>
                                            <SelectItem value="Spouse">Spouse</SelectItem>
                                            <SelectItem value="Child">Child</SelectItem>
                                            <SelectItem value="Parent">Parent</SelectItem>
                                            <SelectItem value="Other">Other</SelectItem>
                                        </SelectContent>
                                    </Select>
                                    <FormMessage />
                                </FormItem>
                            )}
                        />

                        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                            <FormField
                                control={form.control}
                                name="firstName"
                                render={({ field }) => (
                                    <FormItem>
                                        <FormLabel>First Name</FormLabel>
                                        <FormControl>
                                            <Input placeholder="John" {...field} disabled={isMyself} />
                                        </FormControl>
                                        <FormMessage />
                                    </FormItem>
                                )}
                            />
                            <FormField
                                control={form.control}
                                name="lastName"
                                render={({ field }) => (
                                    <FormItem>
                                        <FormLabel>Last Name</FormLabel>
                                        <FormControl>
                                            <Input placeholder="Doe" {...field} disabled={isMyself} />
                                        </FormControl>
                                        <FormMessage />
                                    </FormItem>
                                )}
                            />
                        </div>

                        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                            <FormField
                                control={form.control}
                                name="dateOfBirth"
                                render={({ field }) => (
                                    <FormItem className="flex flex-col">
                                        <FormLabel>Date of birth</FormLabel>
                                        <Popover>
                                            <PopoverTrigger asChild>
                                                <FormControl>
                                                    <Button variant='outline' className={cn('pl-3 text-left font-normal', !field.value && 'text-muted-foreground')}>
                                                        {field.value ? format(field.value, 'PPP') : <span>Pick a date</span>}
                                                        <CalendarIcon className="ml-auto h-4 w-4 opacity-50" />
                                                    </Button>
                                                </FormControl>
                                            </PopoverTrigger>
                                            <PopoverContent className="w-auto p-0" align="start">
                                                <Calendar mode="single" selected={field.value} onSelect={field.onChange} disabled={(date) => date > new Date() || date < new Date('1900-01-01')} initialFocus />
                                            </PopoverContent>
                                        </Popover>
                                        <FormMessage />
                                    </FormItem>
                                )}
                            />
                            <FormField
                                control={form.control}
                                name="gender"
                                render={({ field }) => (
                                    <FormItem>
                                        <FormLabel>Gender</FormLabel>
                                        <Select onValueChange={field.onChange} defaultValue={field.value}>
                                            <FormControl>
                                                <SelectTrigger>
                                                    <SelectValue placeholder="Select gender" />
                                                </SelectTrigger>
                                            </FormControl>
                                            <SelectContent>
                                                <SelectItem value="Male">Male</SelectItem>
                                                <SelectItem value="Female">Female</SelectItem>
                                                <SelectItem value="Other">Other</SelectItem>
                                            </SelectContent>
                                        </Select>
                                        <FormMessage />
                                    </FormItem>
                                )}
                            />
                        </div>

                        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                            <FormField
                                control={form.control}
                                name="passportNumber"
                                render={({ field }) => (
                                    <FormItem>
                                        <FormLabel>Passport Number</FormLabel>
                                        <FormControl>
                                            <Input placeholder="A12345678" {...field} />
                                        </FormControl>
                                        <FormMessage />
                                    </FormItem>
                                )}
                            />
                            <FormField
                                control={form.control}
                                name="nationalityCountryId"
                                render={({ field }) => (
                                    <FormItem>
                                        <FormLabel>Nationality</FormLabel>
                                        <Select onValueChange={field.onChange} defaultValue={field.value}>
                                            <FormControl>
                                                <SelectTrigger>
                                                    <SelectValue placeholder="Select a country" />
                                                </SelectTrigger>
                                            </FormControl>
                                            <SelectContent>
                                                {countries.map((country) => (
                                                    <SelectItem key={country.countryID} value={country.countryID}>
                                                        {country.countryName}
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
                                <FormItem className="flex flex-row items-center justify-between rounded-lg border p-4">
                                    <div className="space-y-0.5">
                                        <FormLabel className="text-base">Active Passenger</FormLabel>
                                        <FormDescription>
                                            Inactive passengers cannot be added to future bookings.
                                        </FormDescription>
                                    </div>
                                    <FormControl>
                                        <Checkbox
                                            checked={field.value}
                                            onCheckedChange={field.onChange}
                                        />
                                    </FormControl>
                                </FormItem>
                            )}
                        />

                        <SheetFooter className="pt-4">
                            <SheetClose asChild>
                                <Button type="button" variant="outline">Cancel</Button>
                            </SheetClose>
                            <Button type="submit" disabled={isLoading}>
                                {isLoading && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
                                {actionLabel}
                            </Button>
                        </SheetFooter>
                    </form>
                </Form>
            </SheetContent>
        </Sheet>
    );
}