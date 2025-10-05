import { Plane } from "lucide-react";
import type { FlightDetailResponse, FlightSearchRequest } from "@/types/search";

export const SearchResultsHeader = ({ flights, request }: { flights: FlightDetailResponse[], request: FlightSearchRequest }) => {
    
    if (!flights || flights.length === 0) return null;

    const origin = flights[0].origin;
    const destination = flights[0].destination;
    const passengerCount = request.adults;
    const passengerText = `${passengerCount} Adult${passengerCount > 1 ? 's' : ''}`;

    return (
        <div className="space-y-2">
            <div className="flex items-center gap-4">
                <h1 className="text-3xl md:text-4xl font-bold tracking-tight text-gray-900 dark:text-gray-50">
                    {origin.city} to {destination.city}
                </h1>
                <Plane className="h-8 w-8 text-gray-500" />
            </div>
            <p className="text-lg text-muted-foreground">
                {request.departureDate} {request.returnDate ? ` - ${request.returnDate}` : ''}  •  {passengerText}
            </p>
        </div>
    );
};