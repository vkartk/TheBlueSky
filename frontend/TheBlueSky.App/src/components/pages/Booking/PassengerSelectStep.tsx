import { useEffect, useState } from 'react';
import { Link } from 'react-router';
import { Info } from 'lucide-react';

import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Checkbox } from '@/components/ui/checkbox';
import { Alert, AlertDescription, AlertTitle } from '@/components/ui/alert';
import { Separator } from '@/components/ui/separator';

import { useAppDispatch, useAppSelector } from '@/store';
import { fetchPassengerByUser } from '@/features/passenger/passengerThunks';
import { selectAllPassengers } from '@/features/passenger/passengerSlice';
import type { Passenger } from '@/types/passenger';



interface PassengerSelectStepProps {
  onNext: (selectedPassengers: Passenger[]) => void;
  onBack: () => void;
}

export const PassengerSelectStep = ({ onNext, onBack }: PassengerSelectStepProps) => {

  const dispatch = useAppDispatch();
  const user = useAppSelector(state => state.auth.user);
  const passengers = useAppSelector(selectAllPassengers);
  const [selected, setSelected] = useState<number[]>([]);

  useEffect(() => {
    if (user?.userId) {
      dispatch(fetchPassengerByUser(user.userId));
    }
  }, [dispatch, user]);

  const handleSelect = (passengerId: number) => {
    setSelected((prev) =>
      prev.includes(passengerId)
        ? prev.filter((id) => id !== passengerId)
        : [...prev, passengerId]
    );
  };

  const handleContinue = () => {
    const selectedPassengers = passengers.filter(p => selected.includes(p.passengerId));
    onNext(selectedPassengers);
  };

  return (
    <Card className="w-full">
      <CardHeader>
        <CardTitle>Select Passengers</CardTitle>
      </CardHeader>
      <CardContent className="space-y-6">
        <Alert>
          <Info className="h-4 w-4" />
          <AlertTitle>Manage Your Travelers</AlertTitle>
          <AlertDescription>
            To add a new passenger or edit existing details, please visit your account page.
            <Button variant="link" asChild className="p-1">
                <Link to="/account/passengers">Manage Passengers</Link>
            </Button>
          </AlertDescription>
        </Alert>

        <div className="space-y-4">
            <h3 className="text-lg font-medium">Your Saved Passengers</h3>
            <Separator/>
             {passengers.map((passenger) => (
                <div key={passenger.passengerId} className="flex items-center space-x-4 p-2 rounded-md hover:bg-slate-50">
                    <Checkbox
                        id={`passenger-${passenger.passengerId}`}
                        checked={selected.includes(passenger.passengerId)}
                        onCheckedChange={() => handleSelect(passenger.passengerId)}
                    />
                    <label
                        htmlFor={`passenger-${passenger.passengerId}`}
                        className="flex-1 cursor-pointer"
                    >
                        <p className="font-semibold">{`${passenger.firstName} ${passenger.lastName}`}</p>
                        <p className="text-sm text-muted-foreground">{passenger.relationshipToManager || 'Passenger'}</p>
                    </label>
                </div>
            ))}
        </div>
        
        <div className="flex justify-between pt-4">
          <Button variant="outline" onClick={onBack}>Back</Button>
          <Button onClick={handleContinue} disabled={selected.length === 0}>
            Continue
          </Button>
        </div>
      </CardContent>
    </Card>
  );
};