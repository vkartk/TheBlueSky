
import { format, parseISO, differenceInMinutes } from 'date-fns';
import { PlaneTakeoff, PlaneLanding, Clock, IndianRupee } from 'lucide-react';

import type { Flight } from '@/types/flight';

export const FlightInfoDisplay = ({ flight, title }: { flight: Flight; title: string }) => {
    const departure = parseISO(flight.departureDateTime);
    const arrival = parseISO(flight.arrivalDateTime);

    const durationMinutes = differenceInMinutes(arrival, departure);
    const hours = Math.floor(durationMinutes / 60);
    const minutes = durationMinutes % 60;
    const durationText = `${hours}h ${minutes}m`;

    return (
        <div>
            <h3 className="text-lg font-semibold mb-4 text-primary">{title}</h3>
            <div className="space-y-4">
                <div className="flex items-center justify-between">
                    <div className="flex items-center space-x-3">
                        <PlaneTakeoff className="h-5 w-5 text-gray-500" />
                        <div>
                            <p className="font-medium">Departure</p>
                            <p className="text-sm text-muted-foreground">{format(departure, 'EEE, dd MMM yyyy - HH:mm')}</p>
                        </div>
                    </div>
                    <div className="flex items-center space-x-3">
                        <Clock className="h-5 w-5 text-gray-500" />
                        <div>
                             <p className="font-medium">Duration</p>
                             <p className="text-sm text-muted-foreground">{durationText}</p>
                        </div>
                    </div>
                    <div className="flex items-center space-x-3 text-right">
                        <div>
                            <p className="font-medium">Arrival</p>
                            <p className="text-sm text-muted-foreground">{format(arrival, 'EEE, dd MMM yyyy - HH:mm')}</p>
                        </div>
                        <PlaneLanding className="h-5 w-5 text-gray-500" />
                    </div>
                </div>
                <div className="flex items-center space-x-3 pt-2">
                     <IndianRupee className="h-5 w-5 text-gray-500" />
                     <div>
                         <p className="font-medium">Base Fare</p>
                         <p className="text-sm text-muted-foreground">₹{flight.baseFare.toLocaleString('en-IN')}</p>
                     </div>
                </div>
            </div>
        </div>
    );
};