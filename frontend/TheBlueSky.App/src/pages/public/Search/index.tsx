import { useSearchParams } from 'react-router';
import { useState, useEffect } from 'react';
import { SearchX, Frown, Loader2 } from 'lucide-react';

import { Card, CardContent } from '@/components/ui/card';
import { Separator } from '@/components/ui/separator';
import { FlightList } from '@/components/pages/Search/FlightList';
import { searchFlights } from '@/services/client/searchApi';
import type { FlightSearchRequest, FlightSearchResponse } from '@/types/search';
import { SearchResultsHeader } from '@/components/pages/Search/SearchResultsHeader';



export default function SearchPage() {

    const [searchParams] = useSearchParams();
    const [request, setRequest] = useState<FlightSearchRequest | null>(null);
    const [results, setResults] = useState<FlightSearchResponse | null>(null);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const searchRequest: FlightSearchRequest = {
            routeId: parseInt(searchParams.get('routeId') || '0'),
            departureDate: searchParams.get('departureDate') || '',
            tripType: searchParams.get('tripType') === 'round-trip' ? 'RoundTrip' : 'OneWay',
            adults: parseInt(searchParams.get('adults') || '1'),
            returnDate: searchParams.get('returnDate') || undefined,
        };
        setRequest(searchRequest);

        if (!searchRequest.routeId || !searchRequest.departureDate) {
            setError("Invalid search criteria provided.");
            setIsLoading(false);
            return;
        }

        const fetchResults = async () => {
            try {
                setIsLoading(true);
                const data = await searchFlights(searchRequest);
                setResults(data);
            } catch (err) {
                setError(err instanceof Error ? err.message : 'An unknown error occurred.');
            } finally {
                setIsLoading(false);
            }
        };
        fetchResults();
    }, [searchParams]);

    const renderContent = () => {
        
        if (isLoading) {
            return (
                <div className="flex justify-center items-center py-20">
                    <Loader2 className="h-10 w-10 animate-spin text-muted-foreground" />
                </div>
            );
        }

        if (error) {
            return (
                <Card className="text-center p-10">
                    <CardContent className="flex flex-col items-center gap-4">
                        <Frown className="h-12 w-12 text-destructive" />
                        <h3 className="text-xl font-semibold">An Error Occurred</h3>
                        <p className="text-muted-foreground">{error}</p>
                    </CardContent>
                </Card>
            );
        }

        if (!results || results.outboundFlights.length === 0) {
            return (
                <Card className="text-center p-10">
                    <CardContent className="flex flex-col items-center gap-4">
                        <SearchX className="h-12 w-12 text-muted-foreground" />
                        <h3 className="text-xl font-semibold">No Flights Found</h3>
                        <p className="text-muted-foreground">We couldn't find any flights for your search. Please try a different date or route.</p>
                    </CardContent>
                </Card>
            );
        }

        return (
            <div className="space-y-8">
                <div>
                    <h2 className="text-2xl font-semibold tracking-tight mb-4">Outbound Flights</h2>
                    <FlightList flights={results.outboundFlights} />
                </div>
                {results.returnFlights && results.returnFlights.length > 0 && (
                    <div>
                        <h2 className="text-2xl font-semibold tracking-tight mb-4">Return Flights</h2>
                        <FlightList flights={results.returnFlights} />
                    </div>
                )}
            </div>
        );
    };

    return (
        <main className="container mx-auto p-4 md:p-8">
            <div className="space-y-8">
                {request && results?.outboundFlights && results.outboundFlights.length > 0 && (
                    <SearchResultsHeader flights={results.outboundFlights} request={request} />
                )}
                <Separator />
                {renderContent()}
            </div>
        </main>
    );
}