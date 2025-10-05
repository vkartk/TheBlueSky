import * as React from 'react';
import { Check, ChevronsUpDown } from 'lucide-react';

import { cn } from '@/lib/utils';
import { Button } from '@/components/ui/button';
import {
  Command,
  CommandEmpty,
  CommandGroup,
  CommandInput,
  CommandItem,
  CommandList,
} from '@/components/ui/command';
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from '@/components/ui/popover';
import type { Airport } from '@/types/airports';

interface AirportComboboxProps {
  airports: Airport[];
  value?: number;
  onChange: (airportId?: number) => void;
  placeholder?: string;
  disabled?: boolean;
}

export const AirportCombobox = ({
  airports,
  value,
  onChange,
  placeholder = 'Select an airport...',
  disabled = false,
}: AirportComboboxProps) => {
  const [open, setOpen] = React.useState(false);

  const selectedAirport = React.useMemo(
    () => airports.find((a) => a.airportId === value),
    [airports, value]
  );

  return (
    <Popover open={open} onOpenChange={setOpen}>
      <PopoverTrigger asChild>
        <Button
          variant="outline"
          role="combobox"
          aria-expanded={open}
          className="w-full justify-between text-left font-normal"
          disabled={disabled}
        >
          <span className="truncate">
            {selectedAirport
              ? `${selectedAirport.city} (${selectedAirport.airportCode})`
              : placeholder}
          </span>
          <ChevronsUpDown className="ml-2 h-4 w-4 shrink-0 opacity-50" />
        </Button>
      </PopoverTrigger>
      <PopoverContent className="w-[--radix-popover-trigger-width] max-h-[--radix-popover-content-available-height] p-0">
        <Command>
          <CommandInput placeholder="Search by city, country, or code..." />
          <CommandList>
            <CommandEmpty>No airport found.</CommandEmpty>
            <CommandGroup>
              {airports.map((airport) => (
                <CommandItem
                  key={airport.airportId}
                  value={`${airport.city} ${airport.countryId} ${airport.airportCode} ${airport.airportName}`}
                  onSelect={() => {
                    onChange(airport.airportId === value ? undefined : airport.airportId);
                    setOpen(false);
                  }}
                  className="p-2 cursor-pointer"
                >
                  <Check
                    className={cn(
                      'mr-2 h-4 w-4',
                      value === airport.airportId ? 'opacity-100' : 'opacity-0'
                    )}
                  />
                  <div>
                    <p className="font-medium">
                      {airport.city}, {airport.countryId} ({airport.airportCode})
                    </p>
                    <p className="text-xs text-muted-foreground truncate">
                      {airport.airportName}
                    </p>
                  </div>
                </CommandItem>
              ))}
            </CommandGroup>
          </CommandList>
        </Command>
      </PopoverContent>
    </Popover>
  );
};