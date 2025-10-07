import { CheckCircle2, Plane, User, Calendar, Clock, MapPin, Printer, Download, Home } from 'lucide-react';
import { Card, CardHeader, CardTitle, CardContent } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Separator } from '@/components/ui/separator';
import { Badge } from '@/components/ui/badge';
import { useNavigate } from 'react-router';
import type { GetFlight } from '@/types/flight';
import type { Passenger } from '@/types/passenger';
import type { PassengerSeats } from '@/pages/public/BookingPage';
import { formatDate, formatTime } from '@/utils/datetime';
import type { Booking } from '@/types/booking';

type BookingData = {
    flight: GetFlight | null;
    passengers: Passenger[];
    seats: PassengerSeats[];
};

export function ConfirmationStep({ bookingData, booking }: { bookingData: BookingData, booking: Booking }) {
    const navigate = useNavigate();

    if (!bookingData.flight) {
        return <div>No booking data available</div>;
    }

    const { flight, passengers, seats } = bookingData;

    const bookingReference = `BK${booking.bookingId}`;

    // Calculate pricing
    const baseFareTotal = flight.baseFare * passengers.length;
    const seatCharges = seats.reduce((total, { seat }) => {
        return total + (seat.aircraftSeat.additionalFare || 0);
    }, 0);
    const subtotal = booking.subtotalAmount;
    const taxAmount = booking.taxAmount;
    const totalAmount = booking.totalAmount;

    const handlePrint = () => {
        window.print();
    };

    const handleDownload = () => {
        window.print();
    };

    return (
        <>
            <style>{`
                @media print {
                    * {
                        -webkit-print-color-adjust: exact !important;
                        print-color-adjust: exact !important;
                        color-adjust: exact !important;
                    }
                    body * {
                        visibility: hidden;
                    }
                    #printable-area, #printable-area * {
                        visibility: visible;
                    }
                    #printable-area {
                        position: absolute;
                        left: 0;
                        top: 0;
                        width: 100%;
                    }
                    .no-print {
                        display: none !important;
                    }
                    .print-border {
                        border: 2px solid #000 !important;
                    }
                }
            `}</style>

            <div className="mx-auto max-w-4xl space-y-6">
                
                <div className="no-print">
                    <Card className="border-green-200 bg-green-50">
                        <CardContent className="flex items-center gap-4 p-6">
                            <div className="flex h-12 w-12 items-center justify-center rounded-full bg-green-500">
                                <CheckCircle2 className="h-6 w-6 text-white" />
                            </div>
                            <div>
                                <h2 className="text-2xl font-bold text-green-900">Booking Confirmed!</h2>
                                <p className="text-green-700">
                                    Your ticket has been successfully booked. A confirmation email has been sent.
                                </p>
                            </div>
                        </CardContent>
                    </Card>
                </div>

                <div className="no-print flex flex-wrap gap-3">
                    <Button onClick={handlePrint} variant="outline" size="lg">
                        <Printer className="mr-2 h-4 w-4" />
                        Print Ticket
                    </Button>
                    <Button onClick={handleDownload} variant="outline" size="lg">
                        <Download className="mr-2 h-4 w-4" />
                        Download PDF
                    </Button>
                    <Button onClick={() => navigate('/')} variant="default" size="lg" className="ml-auto">
                        <Home className="mr-2 h-4 w-4" />
                        Back to Home
                    </Button>
                </div>

                <div id="printable-area" className="space-y-6">
                    
                    <Card className="print-border py-4">
                        <CardHeader className="bg-primary text-primary-foreground">
                            <div className="flex items-center justify-between">
                                <CardTitle className="text-2xl">E-Ticket</CardTitle>
                                <div className="text-right">
                                    <div className="text-sm opacity-90">Booking Reference</div>
                                    <div className="text-2xl font-bold">{bookingReference}</div>
                                </div>
                            </div>
                        </CardHeader>
                        <CardContent className="p-6">
                            <div className="grid gap-4 text-sm">
                                <div className="flex justify-between">
                                    <span className="text-muted-foreground">Booking Date</span>
                                    <span className="font-medium">{formatDate(new Date())}</span>
                                </div>
                                <div className="flex justify-between">
                                    <span className="text-muted-foreground">Status</span>
                                    <Badge className="bg-green-500">Confirmed</Badge>
                                </div>
                            </div>
                        </CardContent>
                    </Card>

                    <Card className="print-border">
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
                                    <div className="text-xl font-bold">{flight.schedule.flightNumber}</div>
                                    <div className="text-sm text-muted-foreground">{flight.schedule.flightName}</div>
                                </div>
                                <div className="text-right">
                                    <div className="text-sm text-muted-foreground">Aircraft</div>
                                    <div className="font-medium">{flight.schedule.aircraft.aircraftModel}</div>
                                    <div className="text-sm text-muted-foreground">{flight.schedule.aircraft.manufacturer}</div>
                                </div>
                            </div>

                            <Separator />

                            <div className="grid gap-6 md:grid-cols-3">
                               
                                <div className="space-y-2">
                                    <div className="flex items-start gap-2">
                                        <MapPin className="mt-1 h-4 w-4 text-muted-foreground" />
                                        <div>
                                            <div className="text-xs text-muted-foreground">Departure From</div>
                                            <div className="text-lg font-bold">{flight.schedule.route.originAirport.airportCode}</div>
                                            <div className="text-sm font-medium">{flight.schedule.route.originAirport.city}</div>
                                            <div className="text-xs text-muted-foreground">
                                                {flight.schedule.route.originAirport.airportName}
                                            </div>
                                        </div>
                                    </div>
                                    <div className="flex items-start gap-2">
                                        <Calendar className="mt-1 h-4 w-4 text-muted-foreground" />
                                        <div>
                                            <div className="font-semibold">{formatDate(flight.departureDateTime)}</div>
                                            <div className="text-lg font-bold text-primary">{formatTime(flight.departureDateTime)}</div>
                                        </div>
                                    </div>
                                </div>

                                
                                <div className="flex flex-col items-center justify-center border-x">
                                    <Clock className="mb-2 h-5 w-5 text-muted-foreground" />
                                    <div className="text-sm text-muted-foreground">Duration</div>
                                    <div className="text-lg font-bold">
                                        {Math.floor(flight.schedule.route.estimatedDurationMinutes / 60)}h{' '}
                                        {flight.schedule.route.estimatedDurationMinutes % 60}m
                                    </div>
                                    <div className="mt-2 text-xs text-muted-foreground">
                                        {flight.schedule.route.distanceKm} km
                                    </div>
                                </div>

                                
                                <div className="space-y-2">
                                    <div className="flex items-start gap-2">
                                        <MapPin className="mt-1 h-4 w-4 text-muted-foreground" />
                                        <div>
                                            <div className="text-xs text-muted-foreground">Arrival At</div>
                                            <div className="text-lg font-bold">{flight.schedule.route.destinationAirport.airportCode}</div>
                                            <div className="text-sm font-medium">{flight.schedule.route.destinationAirport.city}</div>
                                            <div className="text-xs text-muted-foreground">
                                                {flight.schedule.route.destinationAirport.airportName}
                                            </div>
                                        </div>
                                    </div>
                                    <div className="flex items-start gap-2">
                                        <Calendar className="mt-1 h-4 w-4 text-muted-foreground" />
                                        <div>
                                            <div className="font-semibold">{formatDate(flight.arrivalDateTime)}</div>
                                            <div className="text-lg font-bold text-primary">{formatTime(flight.arrivalDateTime)}</div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <Separator />

                            
                            <div className="grid gap-4 rounded-lg bg-muted p-4 text-sm md:grid-cols-2">
                                <div>
                                    <div className="font-medium">Check-in Baggage</div>
                                    <div className="text-muted-foreground">{flight.schedule.checkinBaggageWeightKg} kg per passenger</div>
                                </div>
                                <div>
                                    <div className="font-medium">Cabin Baggage</div>
                                    <div className="text-muted-foreground">{flight.schedule.cabinBaggageWeightKg} kg per passenger</div>
                                </div>
                            </div>
                        </CardContent>
                    </Card>

                    
                    <Card className="print-border">
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
                                            className="flex items-center justify-between rounded-lg border-2 p-4"
                                        >
                                            <div className="flex items-center gap-4">
                                                <div className="flex h-10 w-10 items-center justify-center rounded-full bg-primary/10 font-bold text-primary">
                                                    {idx + 1}
                                                </div>
                                                <div>
                                                    <div className="font-semibold">
                                                        {passenger.firstName} {passenger.lastName}
                                                    </div>
                                                    <div className="text-sm text-muted-foreground">
                                                        {passenger.gender} • Age: {new Date().getFullYear() - new Date(passenger.dateOfBirth).getFullYear()}
                                                    </div>
                                                    {passenger.passportNumber && (
                                                        <div className="text-xs text-muted-foreground">
                                                            Passport: {passenger.passportNumber}
                                                        </div>
                                                    )}
                                                </div>
                                            </div>
                                            {seatInfo && (
                                                <div className="text-right">
                                                    <div className="text-xs text-muted-foreground">Seat Number</div>
                                                    <div className="text-2xl font-bold text-primary">
                                                        {seatInfo.seat.aircraftSeat.seatNumber}
                                                    </div>
                                                    <Badge variant="secondary" className="mt-1">
                                                        {seatInfo.seat.aircraftSeat.seatClass}
                                                    </Badge>
                                                </div>
                                            )}
                                        </div>
                                    );
                                })}
                            </div>
                        </CardContent>
                    </Card>

                    
                    <Card className="print-border">
                        <CardHeader>
                            <CardTitle>Payment Summary</CardTitle>
                        </CardHeader>
                        <CardContent>
                            <div className="space-y-3">
                                <div className="flex justify-between text-sm">
                                    <span className="text-muted-foreground">Base Fare × {passengers.length} passenger{passengers.length > 1 ? 's' : ''}</span>
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

                                <Separator className="my-3" />

                                <div className="flex justify-between text-xl font-bold">
                                    <span>Total Paid</span>
                                    <span className="text-green-600">₹{totalAmount.toLocaleString()}</span>
                                </div>

                                <div className="rounded-lg bg-green-50 p-3 text-sm">
                                    <div className="font-medium text-green-900">✓ Payment Successful</div>
                                    <div className="text-xs text-green-700">Transaction ID: TXN{Date.now().toString().slice(-10)}</div>
                                </div>
                            </div>
                        </CardContent>
                    </Card>

                    
                    <Card className="print-border">
                        <CardHeader>
                            <CardTitle className="text-base">Important Information</CardTitle>
                        </CardHeader>
                        <CardContent className="space-y-2 text-sm">
                            <div className="flex gap-2">
                                <span className="font-semibold">•</span>
                                <p>Please arrive at the airport at least 2 hours before departure for domestic flights.</p>
                            </div>
                            <div className="flex gap-2">
                                <span className="font-semibold">•</span>
                                <p>Carry a valid photo ID and this e-ticket (printed or digital) for check-in.</p>
                            </div>
                            <div className="flex gap-2">
                                <span className="font-semibold">•</span>
                                <p>Web check-in opens 48 hours before departure.</p>
                            </div>
                            <div className="flex gap-2">
                                <span className="font-semibold">•</span>
                                <p>For any queries or cancellations, contact customer support with your booking reference.</p>
                            </div>
                        </CardContent>
                    </Card>

                    
                    <div className="border-t pt-4 text-center text-xs text-muted-foreground">
                        <p>This is a computer-generated e-ticket and does not require a signature.</p>
                        <p className="mt-1">For support, visit our website or call customer service.</p>
                        <p className="mt-2 font-medium">Thank you for choosing our airline!</p>
                    </div>
                </div>
            </div>
        </>
    );
}