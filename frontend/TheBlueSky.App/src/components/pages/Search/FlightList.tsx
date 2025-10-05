import { Plane } from 'lucide-react';
import { format, intervalToDuration } from 'date-fns';
import { Badge } from '@/components/ui/badge';
import { Button } from '@/components/ui/button';
import { Card } from '@/components/ui/card';

import type { FlightDetailResponse } from '@/types/search';
import { getFlightStatusVariant } from '@/utils/flight';


interface FlightListProps {
    flights: FlightDetailResponse[];
}

export const FlightList = ({ flights }: FlightListProps) => {

    if (!flights || flights.length === 0) {
        return (
            <div className="text-center py-10">
                <p className="text-muted-foreground">No flights found for your search.</p>
            </div>
        );
    }

    const handleSelectFlight = (flightId: number) => {
        console.log(`Selected Flight ID: ${flightId}`);
    };

    return (
        <div className="space-y-4">
            {flights.map((flight) => {
                const departureTime = new Date(flight.departureDateTime);
                const arrivalTime = new Date(flight.arrivalDateTime);

                const duration = intervalToDuration({ start: departureTime, end: arrivalTime });
                const durationString = `${duration.hours ?? 0}h ${duration.minutes ?? 0}m`;

                return (
                    <Card key={flight.flightId} className="p-4 transition-all hover:shadow-md">
                        <div className="flex flex-col md:flex-row md:items-center md:justify-between gap-4">
                            <div className="flex items-center gap-4 flex-1">
                                <div className="flex flex-col text-center">
                                    <img src={`https://flagcdn.com/h20/${flight.origin.countryId.toLowerCase()}.png`} alt={`${flight.origin.city} flag`} className="w-6 h-4 mx-auto mb-1" />
                                    <span className="font-bold text-xl">{format(departureTime, 'HH:mm')}</span>
                                    <span className="text-muted-foreground">{flight.origin.airportCode}</span>
                                </div>

                                <div className="flex-grow text-center">
                                    <span className="text-sm text-muted-foreground">{durationString}</span>
                                    <div className="relative h-px bg-border my-1">
                                        <Plane className="absolute w-4 h-4 left-1/2 -translate-x-1/2 -top-2 text-muted-foreground bg-card px-1" />
                                    </div>
                                    <span className="text-sm text-green-600 dark:text-green-400">Direct</span>
                                </div>

                                <div className="flex flex-col text-center">

                                    <img src={`https://flagcdn.com/h20/${flight.destination.countryId.toLowerCase()}.png`} alt={`${flight.destination.city} flag`} className="w-6 h-4 mx-auto mb-1" />
                                    <span className="font-bold text-xl">{format(arrivalTime, 'HH:mm')}</span>
                                    <span className="text-muted-foreground">{flight.destination.airportCode}</span>
                                </div>
                            </div>

                            {/* Vertical Separator for Desktop */}
                            <div className="hidden md:block h-16 w-px bg-border" />
                            <div className="block md:hidden h-px w-full bg-border" />

                            <div className="flex flex-row md:flex-col justify-between items-center md:items-end md:min-w-[180px]">

                                <div className="text-right">
                                    <p className="text-sm text-muted-foreground">starts at</p>
                                    <span className="text-2xl font-bold">${flight.baseFare.toLocaleString()}</span>
                                </div>

                                <div className="flex items-center gap-4 mt-2">
                                    <Badge variant={getFlightStatusVariant(flight.flightStatus)} className="font-semibold">
                                        {flight.flightStatus}
                                    </Badge>
                                    <Button onClick={() => handleSelectFlight(flight.flightId)}>
                                        Select
                                    </Button>
                                </div>
                            </div>
                        </div>
                    </Card>
                );
            })}
        </div>
    );
};