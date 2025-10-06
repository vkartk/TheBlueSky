import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import * as z from 'zod';
import { useAppDispatch } from '@/store';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Select, SelectTrigger, SelectContent, SelectItem, SelectValue } from '@/components/ui/select';
import { Form, FormField, FormItem, FormLabel, FormControl, FormMessage } from '@/components/ui/form';
import type { NewPassenger } from '@/types/passenger';
import { createPassenger } from '@/features/passenger/passengerThunks';

const profileSchema = z.object({
  firstName: z.string(),
  lastName: z.string(),
  email: z.string().email(),
  dateOfBirth: z.string().min(1, 'Date of Birth is required'),
  gender: z.string().optional(),
  nationalityCountryId: z.string().optional(),
  passportNumber: z.string().optional(),
});

type ProfileFormValues = z.infer<typeof profileSchema>;

interface ProfileSetupFormProps {
  userId: string;
  userEmail: string;
  firstName: string;
  lastName: string;
  onComplete: () => void;
}

export const ProfileSetupForm = ({
  userId,
  userEmail,
  firstName,
  lastName,
  onComplete,
}: ProfileSetupFormProps) => {
  const dispatch = useAppDispatch();

  const form = useForm<ProfileFormValues>({
    resolver: zodResolver(profileSchema),
    defaultValues: {
      firstName,
      lastName,
      email: userEmail,
      dateOfBirth: '',
      gender: '',
      nationalityCountryId: '',
      passportNumber: '',
    },
  });

  const onSubmit = async (values: ProfileFormValues) => {
    const payload: NewPassenger = {
      managedByUserId: userId,
      firstName: values.firstName,
      lastName: values.lastName,
      dateOfBirth: values.dateOfBirth,
      gender: values.gender || null,
      nationalityCountryId: values.nationalityCountryId || null,
      passportNumber: values.passportNumber || null,
      relationshipToManager: 'Myself',
      isActive: true,
    };

    await dispatch(createPassenger(payload));
    onComplete();
  };

  return (
    <div className="max-w-md mx-auto py-10">
      <h2 className="text-2xl font-semibold mb-2">Complete Your Profile</h2>
      <p className="text-sm text-muted-foreground mb-6">
        Let’s set up your passenger profile so you can start booking flights.
      </p>

      <Form {...form}>
        <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-5">
          
          <FormField
            control={form.control}
            name="firstName"
            render={({ field }) => (
              <FormItem>
                <FormLabel>First Name</FormLabel>
                <FormControl>
                  <Input {...field} disabled />
                </FormControl>
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
                  <Input {...field} disabled />
                </FormControl>
              </FormItem>
            )}
          />
          <FormField
            control={form.control}
            name="email"
            render={({ field }) => (
              <FormItem>
                <FormLabel>Email</FormLabel>
                <FormControl>
                  <Input {...field} disabled />
                </FormControl>
              </FormItem>
            )}
          />

          <div>
            <FormLabel>Relationship</FormLabel>
            <Input value="Myself" disabled />
          </div>

          <FormField
            control={form.control}
            name="dateOfBirth"
            render={({ field }) => (
              <FormItem>
                <FormLabel>Date of Birth</FormLabel>
                <FormControl>
                  <Input type="date" {...field} />
                </FormControl>
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
                <FormControl>
                  <Select value={field.value} onValueChange={field.onChange}>
                    <SelectTrigger>
                      <SelectValue placeholder="Select gender" />
                    </SelectTrigger>
                    <SelectContent>
                      <SelectItem value="Male">Male</SelectItem>
                      <SelectItem value="Female">Female</SelectItem>
                      <SelectItem value="Other">Other</SelectItem>
                    </SelectContent>
                  </Select>
                </FormControl>
              </FormItem>
            )}
          />

          <FormField
            control={form.control}
            name="nationalityCountryId"
            render={({ field }) => (
              <FormItem>
                <FormLabel>Nationality (Country ID)</FormLabel>
                <FormControl>
                  <Input placeholder="e.g., IN" {...field} />
                </FormControl>
              </FormItem>
            )}
          />

          <FormField
            control={form.control}
            name="passportNumber"
            render={({ field }) => (
              <FormItem>
                <FormLabel>Passport Number</FormLabel>
                <FormControl>
                  <Input placeholder="e.g., N1234567" {...field} />
                </FormControl>
              </FormItem>
            )}
          />

          <Button type="submit" className="w-full">
            Save & Continue
          </Button>
        </form>
      </Form>
    </div>
  );
};
