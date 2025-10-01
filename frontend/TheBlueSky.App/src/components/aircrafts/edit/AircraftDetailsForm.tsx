import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import * as z from 'zod';
import type { Aircraft } from '@/types/aircraft';
import { Button } from '@/components/ui/button';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
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
import { Switch } from '@/components/ui/switch';
import { getLayoutFromModel } from '@/utils/aircraftLayouts';

interface AircraftDetailsFormProps {
  aircraft: Aircraft;
  onSave: (data: Pick<Aircraft, 'aircraftName' | 'isActive'>) => void;
  isSaving: boolean;
}

const formSchema = z.object({
  aircraftName: z.string().min(2, 'Name must be at least 2 characters.'),
  isActive: z.boolean(),
});

export function AircraftDetailsForm({ aircraft, onSave, isSaving }: AircraftDetailsFormProps) {
  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
    values: {
      aircraftName: aircraft.aircraftName,
      isActive: aircraft.isActive,
    },
  });

  const layoutConfig = getLayoutFromModel(aircraft.aircraftModel);

  const onSubmit = (values: z.infer<typeof formSchema>) => {
    onSave(values);
  };

  return (
    <Card>
      <CardHeader>
        <CardTitle>Edit Aircraft</CardTitle>
        <CardDescription>Update the aircraft's name and status.</CardDescription>
      </CardHeader>
      <CardContent>
        <Form {...form}>
          <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-6">
            <FormField
              control={form.control}
              name="aircraftName"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Aircraft Name</FormLabel>
                  <FormControl>
                    <Input placeholder="e.g., Dreamliner" {...field} />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />

            <div className="space-y-2">
              <FormLabel>Manufacturer</FormLabel>
              <Input readOnly disabled value={aircraft.manufacturer} />
            </div>

            <div className="space-y-2">
              <FormLabel>Aircraft Model</FormLabel>
              <Input readOnly disabled value={aircraft.aircraftModel} />
            </div>

            <FormField
              control={form.control}
              name="isActive"
              render={({ field }) => (
                <FormItem className="flex flex-row items-center justify-between rounded-lg border p-3">
                  <div className="space-y-0.5">
                    <FormLabel>Active Status</FormLabel>
                    <FormDescription>
                      Active aircraft are available for flight scheduling.
                    </FormDescription>
                  </div>
                  <FormControl>
                    <Switch checked={field.value} onCheckedChange={field.onChange} />
                  </FormControl>
                </FormItem>
              )}
            />

            {layoutConfig && (
              <div className="space-y-2 rounded-md border p-4">
                <h4 className="text-sm font-medium text-muted-foreground">Layout & Capacity</h4>
                <div className="text-sm">
                  <strong>Layout:</strong> {layoutConfig.layout} ({layoutConfig.name})
                </div>
              </div>
            )}

            <Button type="submit" disabled={isSaving}>
              {isSaving ? 'Saving...' : 'Save Changes'}
            </Button>
          </form>
        </Form>
      </CardContent>
    </Card>
  );
}