import { useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import * as z from 'zod';

import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Checkbox } from '@/components/ui/checkbox';
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
  FormDescription,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from '@/components/ui/form';

import { USER_ROLES, type User } from '@/types/auth';


const userSchema = z.object({
  firstName: z.string().min(1, 'First name is required'),
  lastName: z.string().min(1, 'Last name is required'),
  email: z.email('Invalid email address'),
  roles: z.array(z.string()).min(1, 'At least one role must be selected'),
});

type UserFormValues = z.infer<typeof userSchema>;

interface UserSheetFormProps {
  initialData: User;
  onSave: (data: User) => Promise<void>;
  onClose: () => void;
  isLoading: boolean;
}

export const UserSheetForm = ({ initialData, onSave, onClose, isLoading }: UserSheetFormProps) => {
  const form = useForm<UserFormValues>({
    resolver: zodResolver(userSchema),
    defaultValues: initialData,
  });

  useEffect(() => {
    form.reset(initialData);
  }, [initialData, form]);

  const onSubmit = async (values: UserFormValues) => {
    const dataToSave: User = { ...initialData, ...values };
    await onSave(dataToSave);
  };

  return (
    <SheetContent className="sm:max-w-lg p-4">
      <SheetHeader>
        <SheetTitle>Edit User</SheetTitle>
        <SheetDescription>Update the user's details and roles.</SheetDescription>
      </SheetHeader>
      <Form {...form}>
        <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-6 py-6">
          <div className="grid grid-cols-2 gap-4">
            <FormField control={form.control} name="firstName" render={({ field }) => (
              <FormItem>
                <FormLabel>First Name</FormLabel>
                <FormControl><Input placeholder="John" {...field} /></FormControl>
                <FormMessage />
              </FormItem>
            )} />
            <FormField control={form.control} name="lastName" render={({ field }) => (
              <FormItem>
                <FormLabel>Last Name</FormLabel>
                <FormControl><Input placeholder="Doe" {...field} /></FormControl>
                <FormMessage />
              </FormItem>
            )} />
          </div>
          <FormField control={form.control} name="email" render={({ field }) => (
            <FormItem>
              <FormLabel>Email</FormLabel>
              <FormControl><Input type="email" placeholder="john.doe@example.com" {...field} /></FormControl>
              <FormMessage />
            </FormItem>
          )} />
          <FormField control={form.control} name="roles" render={() => (
            <FormItem>
              <div className="mb-4">
                <FormLabel>User Roles</FormLabel>
                <FormDescription>Select one or more roles for the user.</FormDescription>
              </div>
              {USER_ROLES.map((role) => (
                <FormField key={role} control={form.control} name="roles" render={({ field }) => (
                  <FormItem key={role} className="flex flex-row items-start space-x-3 space-y-0">
                    <FormControl>
                      <Checkbox
                        checked={field.value?.includes(role)}
                        onCheckedChange={(checked) => {
                          return checked
                            ? field.onChange([...field.value, role])
                            : field.onChange(field.value?.filter((value) => value !== role));
                        }}
                      />
                    </FormControl>
                    <FormLabel className="font-normal">{role}</FormLabel>
                  </FormItem>
                )} />
              ))}
              <FormMessage />
            </FormItem>
          )} />
          <SheetFooter className="mt-8">
            <SheetClose asChild>
              <Button type="button" variant="outline" onClick={onClose}>Cancel</Button>
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