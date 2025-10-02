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
import type { Aircraft } from '@/types/aircraft';

interface AircraftComboboxProps {
  aircraftList: Aircraft[];
  value?: number;
  onChange: (value: number) => void;
  placeholder?: string;
}

export const AircraftCombobox = ({
  aircraftList,
  value,
  onChange,
  placeholder = 'Select an aircraft...',
}: AircraftComboboxProps) => {
  const [open, setOpen] = React.useState(false);

  const selectedAircraft = aircraftList.find((aircraft) => aircraft.aircraftId === value);

  return (
    <Popover open={open} onOpenChange={setOpen}>
      <PopoverTrigger asChild>
        <Button
          variant="outline"
          role="combobox"
          aria-expanded={open}
          className="w-full justify-between"
        >
          {selectedAircraft
            ? `${selectedAircraft.aircraftModel} - ${selectedAircraft.aircraftName}`
            : placeholder}
          <ChevronsUpDown className="ml-2 h-4 w-4 shrink-0 opacity-50" />
        </Button>
      </PopoverTrigger>
      <PopoverContent className="w-[--radix-popover-trigger-width] max-h-[--radix-popover-content-available-height] p-0">
        <Command>
          <CommandInput placeholder="Search aircraft..." />
          <CommandList>
            <CommandEmpty>No aircraft found.</CommandEmpty>
            <CommandGroup>
              {aircraftList.map((aircraft) => (
                <CommandItem
                  key={aircraft.aircraftId}
                  value={`${aircraft.aircraftModel} ${aircraft.aircraftName}`}
                  onSelect={() => {
                    onChange(aircraft.aircraftId);
                    setOpen(false);
                  }}
                >
                  <Check
                    className={cn(
                      'mr-2 h-4 w-4',
                      value === aircraft.aircraftId ? 'opacity-100' : 'opacity-0'
                    )}
                  />
                  {aircraft.aircraftModel} - {aircraft.aircraftName}
                </CommandItem>
              ))}
            </CommandGroup>
          </CommandList>
        </Command>
      </PopoverContent>
    </Popover>
  );
};