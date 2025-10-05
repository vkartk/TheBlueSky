import * as React from 'react';
import { ArrowRightLeft } from 'lucide-react';

import { Button } from '@/components/ui/button';
import { AirportCombobox } from '@/components/airports/AirportCombobox';

import type { Airport } from '@/types/airports';
import type { Route } from '@/types/route';

interface LocationInputsProps {
    airports: Airport[];
    routes: Route[];
    value?: number;
    onChange: (routeId?: number) => void;
}

export const LocationInputs = ({ airports, routes, value, onChange }: LocationInputsProps) => {
    const [originId, setOriginId] = React.useState<number>();
    const [destinationId, setDestinationId] = React.useState<number>();

    const routeMap = React.useMemo(() => new Map(routes.map((r) => [r.routeId, r])), [routes]);

    React.useEffect(() => {
        const selectedRoute = value ? routeMap.get(value) : undefined;
        setOriginId(selectedRoute?.originAirportId);
        setDestinationId(selectedRoute?.destinationAirportId);
    }, [value, routeMap]);

    const originAirports = React.useMemo(() => {
        const originIds = new Set(routes.map((r) => r.originAirportId));
        return airports.filter((a) => originIds.has(a.airportId));
    }, [airports, routes]);

    const destinationAirports = React.useMemo(() => {
        if (!originId) return [];
        const destinationIds = new Set(
            routes
                .filter((r) => r.originAirportId === originId)
                .map((r) => r.destinationAirportId)
        );
        return airports.filter((a) => destinationIds.has(a.airportId));
    }, [originId, airports, routes]);

    const handleOriginChange = (newOriginId?: number) => {
        setOriginId(newOriginId);
        setDestinationId(undefined);
        onChange(undefined);
    };

    const handleDestinationChange = (newDestinationId?: number) => {
        setDestinationId(newDestinationId);
        if (originId && newDestinationId) {
            const foundRoute = routes.find(
                (r) => r.originAirportId === originId && r.destinationAirportId === newDestinationId
            );
            onChange(foundRoute?.routeId);
        } else {
            onChange(undefined);
        }
    };

    const handleSwap = () => {
        if (!originId || !destinationId) return;

        const returnRoute = routes.find(
            (r) => r.originAirportId === destinationId && r.destinationAirportId === originId
        );

        onChange(returnRoute?.routeId);
    };

    return (
        <div className="w-full flex flex-col md:flex-row items-center gap-4">
            <div className="w-full flex-1">
                <AirportCombobox
                    airports={originAirports}
                    value={originId}
                    onChange={handleOriginChange}
                    placeholder="Select origin..."
                />
            </div>

            <Button
                variant="ghost"
                size="icon"
                onClick={handleSwap}
                disabled={!destinationId}
                aria-label="Swap origin and destination"
            >
                <ArrowRightLeft className="h-5 w-5" />
            </Button>

            <div className="w-full flex-1">
                <AirportCombobox
                    airports={destinationAirports}
                    value={destinationId}
                    onChange={handleDestinationChange}
                    placeholder="Select destination..."
                    disabled={!originId || destinationAirports.length === 0}
                />
            </div>
        </div>
    );
};