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
import type { Route } from '@/types/route';
import type { Airport } from '@/types/airports'; 

interface RouteComboboxProps {
  routes: Route[];
  airports: Airport[];
  value?: number;
  onChange: (value: number) => void;
  placeholder?: string;
}

export const RouteCombobox = ({
  routes,
  airports,
  value,
  onChange,
  placeholder = 'Select a route...',
}: RouteComboboxProps) => {
  const [open, setOpen] = React.useState(false);

  const airportMap = React.useMemo(
    () => new Map(airports.map((a) => [a.airportId, a])),
    [airports]
  );

  const getRouteName = (route: Route) => {
    const origin = airportMap.get(route.originAirportId);
    const destination = airportMap.get(route.destinationAirportId);

    const originText = origin ? `${origin.airportCode} - ${origin.city}` : `ID: ${route.originAirportId}`;
    const destinationText = destination ? `${destination.airportCode} - ${destination.city}` : `ID: ${route.destinationAirportId}`;
    
    return `(${originText}) → (${destinationText})`;
  };

  const selectedRoute = routes.find((route) => route.routeId === value);

  return (
    <Popover open={open} onOpenChange={setOpen}>
      <PopoverTrigger asChild>
        <Button
          variant="outline"
          role="combobox"
          aria-expanded={open}
          className="w-full justify-between text-left font-normal"
        >
          <span className="truncate">
            {selectedRoute ? getRouteName(selectedRoute) : placeholder}
          </span>
          <ChevronsUpDown className="ml-2 h-4 w-4 shrink-0 opacity-50" />
        </Button>
      </PopoverTrigger>
      <PopoverContent className="w-[--radix-popover-trigger-width] max-h-[--radix-popover-content-available-height] p-0">
        <Command>
          <CommandInput placeholder="Search by airport code or city..." />
          <CommandList>
            <CommandEmpty>No route found.</CommandEmpty>
            <CommandGroup>
              {routes.map((route) => (
                <CommandItem
                  key={route.routeId}
                  value={getRouteName(route)}
                  onSelect={() => {
                    onChange(route.routeId);
                    setOpen(false);
                  }}
                >
                  <Check
                    className={cn(
                      'mr-2 h-4 w-4',
                      value === route.routeId ? 'opacity-100' : 'opacity-0'
                    )}
                  />
                  <span className="truncate">{getRouteName(route)}</span>
                </CommandItem>
              ))}
            </CommandGroup>
          </CommandList>
        </Command>
      </PopoverContent>
    </Popover>
  );
};