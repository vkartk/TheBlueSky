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
  onChange: (value: number) => void;
  placeholder?: string;
}

export const AirportCombobox = ({
  airports,
  value,
  onChange,
  placeholder = 'Select an airport...',
}: AirportComboboxProps) => {
  const [open, setOpen] = React.useState(false);

  const selectedAirport = airports.find((airport) => airport.airportId === value);

  return (
    <Popover open={open} onOpenChange={setOpen}>
      <PopoverTrigger asChild>
        <Button
          variant="outline"
          role="combobox"
          aria-expanded={open}
          className="w-full justify-between"
        >
          {selectedAirport
            ? `${selectedAirport.airportCode} - ${selectedAirport.airportName}`
            : placeholder}
          <ChevronsUpDown className="ml-2 h-4 w-4 shrink-0 opacity-50" />
        </Button>
      </PopoverTrigger>
      <PopoverContent className="w-[--radix-popover-trigger-width] max-h-[--radix-popover-content-available-height] p-0">
        <Command>
          <CommandInput placeholder="Search airport..." />
          <CommandList>
            <CommandEmpty>No airport found.</CommandEmpty>
            <CommandGroup>
              {airports.map((airport) => (
                <CommandItem
                  key={airport.airportId}
                  value={`${airport.airportCode} ${airport.airportName}`}
                  onSelect={() => {
                    onChange(airport.airportId);
                    setOpen(false);
                  }}
                >
                  <Check
                    className={cn(
                      'mr-2 h-4 w-4',
                      value === airport.airportId ? 'opacity-100' : 'opacity-0'
                    )}
                  />
                  {airport.airportCode} - {airport.airportName}
                </CommandItem>
              ))}
            </CommandGroup>
          </CommandList>
        </Command>
      </PopoverContent>
    </Popover>
  );
};