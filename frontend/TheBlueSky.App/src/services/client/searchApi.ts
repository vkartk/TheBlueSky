import { FLIGHTS_BASE_URL } from '@/config';
import type { FlightSearchRequest, FlightSearchResponse } from '@/types/search';


export const searchFlights = async (request: FlightSearchRequest): Promise<FlightSearchResponse> => {
    const params = new URLSearchParams({
        routeId: request.routeId.toString(),
        departureDate: request.departureDate,
        tripType: request.tripType,
        adults: request.adults.toString(),
    });

    if (request.tripType === 'RoundTrip' && request.returnDate) {
        params.append('returnDate', request.returnDate);
    }

    const response = await fetch(`${FLIGHTS_BASE_URL}/Flight/Search?${params.toString()}`);

    if (!response.ok) {
        throw new Error('Failed to fetch flight search results.');
    }
    return await response.json();
};