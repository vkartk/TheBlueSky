import { useState } from 'react';
import { type DateRange } from 'react-day-picker';
import { Calendar as CalendarIcon, Loader2 } from 'lucide-react';
import { cn } from '@/lib/utils';
import { Button } from '@/components/ui/button';
import { Calendar } from '@/components/ui/calendar';
import { Popover, PopoverContent, PopoverTrigger } from '@/components/ui/popover';
import { Card, CardContent, CardHeader, CardTitle, CardFooter } from '@/components/ui/card';

interface FlightGeneratorProps {
  onGenerate: (range: { startDate: string; endDate: string }) => void;
  isLoading: boolean;
}

// format a Date object into 'YYYY-MM-DD' string
const toApiDateString = (date: Date): string => {
  const year = date.getFullYear();
  const month = String(date.getMonth() + 1).padStart(2, '0');
  const day = String(date.getDate()).padStart(2, '0');
  return `${year}-${month}-${day}`;
};

export const FlightGenerator = ({ onGenerate, isLoading }: FlightGeneratorProps) => {
  const [date, setDate] = useState<DateRange | undefined>();

  const handleGenerateClick = () => {
    if (date?.from && date?.to) {
      onGenerate({
        startDate: toApiDateString(date.from),
        endDate: toApiDateString(date.to),
      });
    }
  };

  const displayDateOptions: Intl.DateTimeFormatOptions = {
    year: 'numeric',
    month: 'short',
    day: 'numeric',
  };

  return (
    <Card>
      <CardHeader>
        <CardTitle>Generate Flights</CardTitle>
      </CardHeader>
      <CardContent>
        <Popover>
          <PopoverTrigger asChild>
            <Button
              id="date"
              variant={'outline'}
              className={cn(
                'w-full justify-start text-left font-normal',
                !date && 'text-muted-foreground'
              )}
            >
              <CalendarIcon className="mr-2 h-4 w-4" />
              {date?.from ? (
                date.to ? (
                  <>
                    {date.from.toLocaleDateString(undefined, displayDateOptions)} -{' '}
                    {date.to.toLocaleDateString(undefined, displayDateOptions)}
                  </>
                ) : (
                  date.from.toLocaleDateString(undefined, displayDateOptions)
                )
              ) : (
                <span>Pick a date range</span>
              )}
            </Button>
          </PopoverTrigger>
          <PopoverContent className="w-auto p-0" align="start">
            <Calendar
              initialFocus
              mode="range"
              defaultMonth={date?.from}
              selected={date}
              onSelect={setDate}
              numberOfMonths={2}
            />
          </PopoverContent>
        </Popover>
      </CardContent>
      <CardFooter className="flex justify-end">
        <Button onClick={handleGenerateClick} disabled={!date?.from || !date?.to || isLoading}>
          {isLoading && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
          Generate Flights
        </Button>
      </CardFooter>
    </Card>
  );
};