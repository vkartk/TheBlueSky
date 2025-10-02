import { useState, useEffect } from 'react';
import { Card, CardContent, CardHeader, CardTitle, CardFooter } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Checkbox } from '@/components/ui/checkbox';
import { Label } from '@/components/ui/label';
import { daysOfWeek, type DayOfWeek, type ScheduleDay } from '@/types/scheduleDay';
import { Loader2 } from 'lucide-react';

interface ScheduleDaySelectorProps {
  initialDays: ScheduleDay[];
  onSave: (selectedDays: DayOfWeek[]) => void;
  isLoading: boolean;
}

export const ScheduleDaySelector = ({ initialDays, onSave, isLoading }: ScheduleDaySelectorProps) => {
  const [selectedDays, setSelectedDays] = useState<Set<DayOfWeek>>(new Set());

  useEffect(() => {
    if (initialDays && initialDays.length > 0) {
        const activeDays = new Set(
        initialDays
          .filter(d => d.isActive)
          .map(d => daysOfWeek[d.dayOfWeek])
      );
      setSelectedDays(activeDays);
    }
  }, [initialDays]);

  const handleDayToggle = (day: DayOfWeek, checked: boolean) => {
    const newSelectedDays = new Set(selectedDays);
    if (checked) {
      newSelectedDays.add(day);
    } else {
      newSelectedDays.delete(day);
    }
    setSelectedDays(newSelectedDays);
  };

  const handleSaveClick = () => {
    onSave(Array.from(selectedDays));
  };

  return (
    <Card>
      <CardHeader>
        <CardTitle>Set Weekly Schedule</CardTitle>
      </CardHeader>
      <CardContent className="grid grid-cols-2 sm:grid-cols-4 lg:grid-cols-7 gap-4">
        {daysOfWeek.map((day) => (
          <div key={day} className="flex items-center space-x-2">
            <Checkbox
              id={day}
              checked={selectedDays.has(day)}
              onCheckedChange={(checked) => handleDayToggle(day, !!checked)}
            />
            <Label htmlFor={day} className="font-normal">{day}</Label>
          </div>
        ))}
      </CardContent>
      <CardFooter className="flex justify-end">
        <Button onClick={handleSaveClick} disabled={isLoading}>
          {isLoading && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
          Save Weekly Schedule
        </Button>
      </CardFooter>
    </Card>
  );
};