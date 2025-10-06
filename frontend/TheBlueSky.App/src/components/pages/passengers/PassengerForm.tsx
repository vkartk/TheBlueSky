import { useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { CalendarIcon } from 'lucide-react';
import { format } from 'date-fns';

import { cn } from '@/lib/utils';
import { Button } from '@/components/ui/button';
import { Calendar } from '@/components/ui/calendar';
import { Form, FormControl, FormField, FormItem, FormLabel, FormMessage } from '@/components/ui/form';
import { Input } from '@/components/ui/input';
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select';
import { Popover, PopoverContent, PopoverTrigger } from '@/components/ui/popover';
import type { Country } from '@/types/country';

export type User = {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  roles: string[];
};

const passengerFormSchema = z.object({
  relationshipToManager: z.string().min(1, 'Relationship is required.'),
  firstName: z.string().min(2, 'First name must be at least 2 characters.'),
  lastName: z.string().min(2, 'Last name must be at least 2 characters.'),
  dateOfBirth: z.date(),
  gender: z.string().min(1, 'Gender is required.'),
  passportNumber: z.string().min(6, 'A valid passport number is required.'),
  nationalityCountryId: z.string().min(1, 'Nationality is required.'),
});

export type PassengerFormData = z.infer<typeof passengerFormSchema>;

interface PassengerFormProps {
  currentUser: User;
  countries: Country[];
  onSubmit: (data: PassengerFormData) => void;
  isLoading?: boolean;
  initialRelationship?: string;
}

export const PassengerForm = ({
  currentUser,
  countries,
  onSubmit,
  isLoading = false,
  initialRelationship = 'Myself',
}: PassengerFormProps) => {
  const form = useForm<PassengerFormData>({
    resolver: zodResolver(passengerFormSchema),
    defaultValues: {
      relationshipToManager: initialRelationship,
      firstName: initialRelationship === 'Myself' ? currentUser.firstName : '',
      lastName: initialRelationship === 'Myself' ? currentUser.lastName : '',
      passportNumber: '',
      gender: '',
      nationalityCountryId: '',
    },
  });

  const relationship = form.watch('relationshipToManager');
  const isMyself = relationship === 'Myself';

  useEffect(() => {
    if (isMyself) {
      form.setValue('firstName', currentUser.firstName, { shouldValidate: true });
      form.setValue('lastName', currentUser.lastName, { shouldValidate: true });
    } else {
      if (form.getValues('firstName') === currentUser.firstName) {
        form.setValue('firstName', '');
        form.setValue('lastName', '');
      }
    }
  }, [isMyself, currentUser, form]);

  return (
    <Form {...form}>
      <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-6">
        <FormField
          control={form.control}
          name="relationshipToManager"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Relationship to You</FormLabel>
              <Select onValueChange={field.onChange} defaultValue={field.value}>
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
                      <Button
                        variant={'outline'}
                        className={cn(
                          'pl-3 text-left font-normal',
                          !field.value && 'text-muted-foreground'
                        )}
                      >
                        {field.value ? format(field.value, 'PPP') : <span>Pick a date</span>}
                        <CalendarIcon className="ml-auto h-4 w-4 opacity-50" />
                      </Button>
                    </FormControl>
                  </PopoverTrigger>
                  <PopoverContent className="w-auto p-0" align="start">
                    <Calendar
                      mode="single"
                      selected={field.value}
                      onSelect={field.onChange}
                      disabled={(date) => date > new Date() || date < new Date('1900-01-01')}
                      initialFocus
                    />
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

        <div className="flex justify-end pt-4">
          <Button type="submit" disabled={isLoading}>
            {isLoading ? 'Saving...' : 'Save Passenger'}
          </Button>
        </div>
      </form>
    </Form>
  );
};