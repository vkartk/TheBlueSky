import { useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import * as z from 'zod';
import { useAppDispatch, useAppSelector } from '@/store';
import { selectAllCountries } from '@/features/countries/countriesSlice';

import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Checkbox } from '@/components/ui/checkbox';
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
  SheetDescription,
  SheetFooter,
  SheetClose,
} from '@/components/ui/sheet';
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from '@/components/ui/form';

import type { Airport, NewAirport } from '@/types/airports';
import { fetchCountries } from '@/features/countries/countriesThunks';

const airportSchema = z.object({
    airportCode: z.string()
        .min(3, "Code must be 3 chars")
        .max(3, "Code must be 3 chars")
        .regex(/^[A-Z]+$/, "Code must be uppercase letters"),
    airportName: z.string()
        .min(1, "Name is required"),
    city: z.string()
        .min(1, "City is required"),
    countryId: z.string()
        .min(2, "Country ID must be 2 chars")
        .max(2, "Country ID must be 2 chars")
        .regex(/^[A-Z]+$/, "Must be uppercase letters"),
  isActive: z.boolean(),
});

type AirportFormValues = z.infer<typeof airportSchema>;

interface AirportSheetFormProps {
  initialData?: Airport | null;
  onSave: (data: NewAirport | Airport) => Promise<void>;
  onClose: () => void;
  isLoading: boolean;
}

export const AirportSheetForm = ({ initialData, onSave, onClose, isLoading }: AirportSheetFormProps) => {
  const isEditMode = !!initialData;

  const dispatch = useAppDispatch()
  const countries = useAppSelector(selectAllCountries);

  const form = useForm<AirportFormValues>({
    resolver: zodResolver(airportSchema),
    defaultValues: initialData || {
      airportCode: '',
      airportName: '',
      city: '',
      countryId: '',
      isActive: true,
    },
  });

  console.log(countries)
  useEffect(() => {
    dispatch(fetchCountries())
    
    form.reset(
      initialData || {
        airportCode: '',
        airportName: '',
        city: '',
        countryId: '',
        isActive: true,
      }
    );
  }, [initialData, form]);

  const onSubmit = async (values: AirportFormValues) => {
    const updateData = isEditMode ? { ...initialData, ...values } : values;
    await onSave(updateData);
  };

  return (
    <SheetContent className="sm:max-w-lg p-4">
      <SheetHeader>
        <SheetTitle>{isEditMode ? 'Edit Airport' : 'Add New Airport'}</SheetTitle>
        <SheetDescription>
          {isEditMode ? "Update the airport's details." : 'Enter details for the new airport.'}
        </SheetDescription>
      </SheetHeader>
      <Form {...form}>
        <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-6 py-6">
          <FormField
            control={form.control}
            name="airportCode"
            render={({ field }) => (
              <FormItem>
                <FormLabel>Airport Code</FormLabel>
                <FormControl>
                  <Input placeholder="e.g., MAA" {...field} />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />
          <FormField
            control={form.control}
            name="airportName"
            render={({ field }) => (
              <FormItem>
                <FormLabel>Airport Name</FormLabel>
                <FormControl>
                  <Input placeholder="e.g., Chennai International Airport" {...field} />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />
          <div className="grid grid-cols-2 gap-4">
            <FormField
              control={form.control}
              name="city"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>City</FormLabel>
                  <FormControl>
                    <Input placeholder="e.g., Chennai" {...field} />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
            <FormField
              control={form.control}
              name="countryId"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Country</FormLabel>
                  <Select 
                    onValueChange={field.onChange} 
                    defaultValue={field.value}
                    value={field.value}
                  >
                    <FormControl>
                      <SelectTrigger>
                        <SelectValue placeholder="Select country" />
                      </SelectTrigger>
                    </FormControl>
                    <SelectContent key="country-select-content">
                      {countries && countries.length > 0 ? (
                        countries.map((country) => (
                          <SelectItem 
                            key={`country-${country.countryID}`}
                            value={country.countryID}
                          >
                            {country.countryName} ({country.countryID})
                          </SelectItem>
                        ))
                      ) : (
                        <SelectItem value="no-countries" disabled>
                          No countries available
                        </SelectItem>
                      )}
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
          <SheetFooter className="mt-8"> 
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