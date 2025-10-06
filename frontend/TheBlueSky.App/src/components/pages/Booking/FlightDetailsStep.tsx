import { Card, CardContent, CardDescription, CardFooter, CardHeader, CardTitle } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Separator } from '@/components/ui/separator';

import type { Flight } from '@/types/flight';
import { FlightInfoDisplay } from './FlightInfoDisplay';



interface FlightDetailsStepProps {
  flight: Flight | null;
  returnFlight?: Flight | null;
  onNext: () => void;
}

export const FlightDetailsStep = ({ flight, returnFlight, onNext }: FlightDetailsStepProps) => {

  if (!flight) {
    return (
        <Card className="w-full max-w-3xl">
            <CardHeader>
                <CardTitle>Error Loading Flight</CardTitle>
                <CardDescription>The requested flight details could not be found. Please try your search again.</CardDescription>
            </CardHeader>
        </Card>
    );
  }

  return (
    <Card className="w-full max-w-3xl">
      <CardHeader>
        <CardTitle>Confirm Your Flight Details</CardTitle>
        <CardDescription>
          Please review the flight information below before proceeding to the next step.
        </CardDescription>
      </CardHeader>
      <CardContent className="space-y-6">
        <FlightInfoDisplay flight={flight} title="Departure Flight" />
        {returnFlight && (
          <>
            <Separator className="my-4" />
            <FlightInfoDisplay flight={returnFlight} title="Return Flight" />
          </>
        )}
      </CardContent>
      <CardFooter className="flex justify-end">
        <Button onClick={onNext} size="lg">Continue to Passenger Selection</Button>
      </CardFooter>
    </Card>
  );
};