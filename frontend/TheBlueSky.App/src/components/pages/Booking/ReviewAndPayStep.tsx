import { useState } from 'react';
import { Plane, User, CreditCard, Wallet, Building2, ChevronLeft, Calendar, Clock, MapPin } from 'lucide-react';
import { Card, CardHeader, CardTitle, CardContent } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Separator } from '@/components/ui/separator';
import { RadioGroup, RadioGroupItem } from '@/components/ui/radio-group';
import { Label } from '@/components/ui/label';
import { Badge } from '@/components/ui/badge';
import type { GetFlight } from '@/types/flight';
import type { Passenger } from '@/types/passenger';
import type { PassengerSeats } from '@/pages/public/BookingPage';
import { formatDate, formatTime } from '@/utils/datetime';

type BookingData = {
    flight: GetFlight | null;
    passengers: Passenger[];
    seats: PassengerSeats[];
};

type PaymentMethod = 'card' | 'upi' | 'netbanking';

export function ReviewAndPayStep({
    bookingData,
    onConfirm,
    onBack
}: {
    bookingData: BookingData;
    onConfirm: () => void;
    onBack: () => void;
}) {
    const [paymentMethod, setPaymentMethod] = useState<PaymentMethod>('card');
    const [isProcessing, setIsProcessing] = useState(false);

    if (!bookingData.flight) {
        return <div>No flight data available</div>;
    }

    const { flight, passengers, seats } = bookingData;

    // Calculate pricing
    const baseFareTotal = flight.baseFare * passengers.length;
    const seatCharges = seats.reduce((total, { seat }) => {
        return total + (seat.aircraftSeat.additionalFare || 0);
    }, 0);

    const subtotal = baseFareTotal + seatCharges;
    const taxAmount = subtotal * 0.18;
    const totalAmount = subtotal + taxAmount;

    const handlePayment = async () => {
        setIsProcessing(true);
        await new Promise(resolve => setTimeout(resolve, 2000));
        setIsProcessing(false);
        onConfirm();
    };

    return (
        <div className="mx-auto max-w-6xl space-y-6">
            <div className="grid gap-6 lg:grid-cols-[1fr_400px]">
                
                <div className="space-y-6">
                    
                    <Card>
                        <CardHeader>
                            <CardTitle className="flex items-center gap-2">
                                <Plane className="h-5 w-5" />
                                Flight Details
                            </CardTitle>
                        </CardHeader>
                        <CardContent className="space-y-4">
                            <div className="flex items-center justify-between">
                                <div>
                                    <div className="text-sm text-muted-foreground">Flight Number</div>
                                    <div className="text-lg font-semibold">{flight.schedule.flightNumber}</div>
                                </div>
                                <Badge variant="secondary">{flight.flightStatus}</Badge>
                            </div>

                            <Separator />

                            <div className="grid gap-4 md:grid-cols-2">
                                <div className="space-y-3">
                                    <div className="flex items-start gap-2">
                                        <MapPin className="mt-1 h-4 w-4 text-muted-foreground" />
                                        <div>
                                            <div className="text-sm text-muted-foreground">From</div>
                                            <div className="font-medium">{flight.schedule.route.originAirport.city}</div>
                                            <div className="text-xs text-muted-foreground">
                                                {flight.schedule.route.originAirport.airportName}
                                            </div>
                                        </div>
                                    </div>
                                    <div className="flex items-start gap-2">
                                        <Calendar className="mt-1 h-4 w-4 text-muted-foreground" />
                                        <div>
                                            <div className="text-sm text-muted-foreground">Departure</div>
                                            <div className="font-medium">{formatDate(flight.departureDateTime)}</div>
                                            <div className="text-sm">{formatTime(flight.departureDateTime)}</div>
                                        </div>
                                    </div>
                                </div>

                                <div className="space-y-3">
                                    <div className="flex items-start gap-2">
                                        <MapPin className="mt-1 h-4 w-4 text-muted-foreground" />
                                        <div>
                                            <div className="text-sm text-muted-foreground">To</div>
                                            <div className="font-medium">{flight.schedule.route.destinationAirport.city}</div>
                                            <div className="text-xs text-muted-foreground">
                                                {flight.schedule.route.destinationAirport.airportName}
                                            </div>
                                        </div>
                                    </div>
                                    <div className="flex items-start gap-2">
                                        <Clock className="mt-1 h-4 w-4 text-muted-foreground" />
                                        <div>
                                            <div className="text-sm text-muted-foreground">Arrival</div>
                                            <div className="font-medium">{formatDate(flight.arrivalDateTime)}</div>
                                            <div className="text-sm">{formatTime(flight.arrivalDateTime)}</div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <Separator />

                            <div className="grid gap-2 text-sm">
                                <div className="flex justify-between">
                                    <span className="text-muted-foreground">Aircraft</span>
                                    <span className="font-medium">{flight.schedule.aircraft.aircraftModel}</span>
                                </div>
                                <div className="flex justify-between">
                                    <span className="text-muted-foreground">Duration</span>
                                    <span className="font-medium">
                                        {Math.floor(flight.schedule.route.estimatedDurationMinutes / 60)}h{' '}
                                        {flight.schedule.route.estimatedDurationMinutes % 60}m
                                    </span>
                                </div>
                            </div>
                        </CardContent>
                    </Card>

                    
                    <Card>
                        <CardHeader>
                            <CardTitle className="flex items-center gap-2">
                                <User className="h-5 w-5" />
                                Passenger Details
                            </CardTitle>
                        </CardHeader>
                        <CardContent>
                            <div className="space-y-3">
                                {passengers.map((passenger, idx) => {
                                    const seatInfo = seats.find(s => s.passengerId === passenger.passengerId);
                                    return (
                                        <div
                                            key={passenger.passengerId}
                                            className="flex items-center justify-between rounded-lg border p-4"
                                        >
                                            <div className="flex items-center gap-3">
                                                <div className="flex h-10 w-10 items-center justify-center rounded-full bg-primary/10">
                                                    <User className="h-5 w-5 text-primary" />
                                                </div>
                                                <div>
                                                    <div className="font-medium">
                                                        {passenger.firstName} {passenger.lastName}
                                                    </div>
                                                    <div className="text-sm text-muted-foreground">
                                                        {passenger.gender} • {passenger.relationshipToManager || 'Self'}
                                                    </div>
                                                </div>
                                            </div>
                                            {seatInfo && (
                                                <div className="text-right">
                                                    <div className="font-semibold">
                                                        {seatInfo.seat.aircraftSeat.seatNumber}
                                                    </div>
                                                    <div className="text-xs text-muted-foreground">
                                                        {seatInfo.seat.aircraftSeat.seatClass}
                                                    </div>
                                                    {seatInfo.seat.aircraftSeat.additionalFare > 0 && (
                                                        <div className="text-xs text-green-600">
                                                            +₹{seatInfo.seat.aircraftSeat.additionalFare}
                                                        </div>
                                                    )}
                                                </div>
                                            )}
                                        </div>
                                    );
                                })}
                            </div>
                        </CardContent>
                    </Card>

                    
                    <Card>
                        <CardHeader>
                            <CardTitle className="flex items-center gap-2">
                                <CreditCard className="h-5 w-5" />
                                Payment Method
                            </CardTitle>
                        </CardHeader>
                        <CardContent>
                            <RadioGroup value={paymentMethod} onValueChange={(value) => setPaymentMethod(value as PaymentMethod)}>
                                <div className="space-y-3">
                                    <Label
                                        htmlFor="card"
                                        className="flex cursor-pointer items-center justify-between rounded-lg border p-4 hover:bg-muted"
                                    >
                                        <div className="flex items-center gap-3">
                                            <RadioGroupItem value="card" id="card" />
                                            <CreditCard className="h-5 w-5 text-muted-foreground" />
                                            <div>
                                                <div className="font-medium">Credit / Debit Card</div>
                                                <div className="text-sm text-muted-foreground">Visa, Mastercard, RuPay</div>
                                            </div>
                                        </div>
                                    </Label>

                                    <Label
                                        htmlFor="upi"
                                        className="flex cursor-pointer items-center justify-between rounded-lg border p-4 hover:bg-muted"
                                    >
                                        <div className="flex items-center gap-3">
                                            <RadioGroupItem value="upi" id="upi" />
                                            <Wallet className="h-5 w-5 text-muted-foreground" />
                                            <div>
                                                <div className="font-medium">UPI</div>
                                                <div className="text-sm text-muted-foreground">Pay via UPI apps</div>
                                            </div>
                                        </div>
                                    </Label>

                                    <Label
                                        htmlFor="netbanking"
                                        className="flex cursor-pointer items-center justify-between rounded-lg border p-4 hover:bg-muted"
                                    >
                                        <div className="flex items-center gap-3">
                                            <RadioGroupItem value="netbanking" id="netbanking" />
                                            <Building2 className="h-5 w-5 text-muted-foreground" />
                                            <div>
                                                <div className="font-medium">Net Banking</div>
                                                <div className="text-sm text-muted-foreground">All major banks</div>
                                            </div>
                                        </div>
                                    </Label>
                                </div>
                            </RadioGroup>
                        </CardContent>
                    </Card>
                </div>

                
                <div className="space-y-6">
                    <Card className="sticky top-4">
                        <CardHeader>
                            <CardTitle>Price Summary</CardTitle>
                        </CardHeader>
                        <CardContent className="space-y-4">
                            <div className="space-y-3">
                                <div className="flex justify-between text-sm">
                                    <span className="text-muted-foreground">
                                        Base Fare × {passengers.length}
                                    </span>
                                    <span className="font-medium">₹{baseFareTotal.toLocaleString()}</span>
                                </div>

                                {seatCharges > 0 && (
                                    <div className="flex justify-between text-sm">
                                        <span className="text-muted-foreground">Seat Selection Charges</span>
                                        <span className="font-medium">₹{seatCharges.toLocaleString()}</span>
                                    </div>
                                )}

                                <Separator />

                                <div className="flex justify-between text-sm">
                                    <span className="text-muted-foreground">Subtotal</span>
                                    <span className="font-medium">₹{subtotal.toLocaleString()}</span>
                                </div>

                                <div className="flex justify-between text-sm">
                                    <span className="text-muted-foreground">Taxes & Fees (18%)</span>
                                    <span className="font-medium">₹{taxAmount.toLocaleString()}</span>
                                </div>

                                <Separator />

                                <div className="flex justify-between text-lg font-bold">
                                    <span>Total Amount</span>
                                    <span className="text-primary">₹{totalAmount.toLocaleString()}</span>
                                </div>
                            </div>

                            <div className="rounded-lg bg-blue-50 p-3 text-sm text-blue-900">
                                <p className="font-medium">Cancellation Policy</p>
                                <p className="mt-1 text-xs">
                                    Free cancellation up to 24 hours before departure. After that, cancellation charges apply.
                                </p>
                            </div>

                            <Separator />

                            <div className="space-y-3">
                                <Button
                                    onClick={handlePayment}
                                    disabled={isProcessing}
                                    className="w-full"
                                    size="lg"
                                >
                                    {isProcessing ? 'Processing...' : `Pay ₹${totalAmount.toLocaleString()}`}
                                </Button>

                                <Button
                                    variant="outline"
                                    onClick={onBack}
                                    disabled={isProcessing}
                                    className="w-full"
                                >
                                    <ChevronLeft className="mr-2 h-4 w-4" />
                                    Back to Seat Selection
                                </Button>
                            </div>

                            <p className="text-center text-xs text-muted-foreground">
                                By proceeding, you agree to our Terms & Conditions
                            </p>
                        </CardContent>
                    </Card>
                </div>
            </div>
        </div>
    );
}