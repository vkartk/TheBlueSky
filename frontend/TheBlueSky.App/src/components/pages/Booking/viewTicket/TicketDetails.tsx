import { useNavigate } from 'react-router';
import { Plane, User, Calendar, Clock, MapPin, Printer, Home } from 'lucide-react';
import { Card, CardHeader, CardTitle, CardContent } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Separator } from '@/components/ui/separator';
import { Badge } from '@/components/ui/badge';
import { formatDate, formatTime } from '@/utils/datetime';

import type { Booking } from '@/types/booking';
import type { GetFlight } from '@/types/flight';
import type { Passenger } from '@/types/passenger';
import { getBookingStatusVariant } from '@/utils/booking';

type TicketDetailsProps = {
    booking: Booking;
    flight: GetFlight;
    allPassengers: Passenger[];
};

export function TicketDetails({ booking, flight, allPassengers }: TicketDetailsProps) {
    const navigate = useNavigate();
    const bookingReference = `BK${booking.bookingId}`;

    const seatMap = new Map(flight.seatStatuses.map(seat => [seat.flightSeatStatusId, seat]));
    const passengerMap = new Map(allPassengers.map(p => [p.passengerId, p]));

    const handlePrint = () => window.print();

    return (
        <>
            <style>{`
                @media print {
                    * { -webkit-print-color-adjust: exact !important; print-color-adjust: exact !important; color-adjust: exact !important; }
                    body * { visibility: hidden; }
                    #printable-area, #printable-area * { visibility: visible; }
                    #printable-area { position: absolute; left: 0; top: 0; width: 100%; }
                    .no-print { display: none !important; }
                    .print-border { border: 2px solid #000 !important; }
                }
            `}</style>
            
            <div className="mx-auto max-w-4xl space-y-6">
                <div className="no-print flex flex-wrap gap-3">
                    <Button onClick={handlePrint} variant="outline" size="lg">
                        <Printer className="mr-2 h-4 w-4" /> Print Ticket
                    </Button>
                    <Button onClick={() => navigate('/')} variant="default" size="lg" className="ml-auto">
                        <Home className="mr-2 h-4 w-4" /> Back to Home
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
                                    <span className="font-medium">{formatDate(new Date(booking.bookingDate))}</span>
                                </div>
                                <div className="flex justify-between">
                                    <span className="text-muted-foreground">Status</span>
                                    <Badge variant={getBookingStatusVariant(booking.bookingStatus)}>{booking.bookingStatus}</Badge>
                                </div>
                            </div>
                        </CardContent>
                    </Card>

                    <Card className="print-border">
                         <CardHeader>
                            <CardTitle className="flex items-center gap-2"><Plane className="h-5 w-5" /> Flight Details</CardTitle>
                        </CardHeader>
                        <CardContent className="space-y-4">
                            <div className="flex items-center justify-between">
                                <div>
                                    <div className="text-sm text-muted-foreground">Flight</div>
                                    <div className="text-xl font-bold">{flight.schedule.flightNumber}</div>
                                </div>
                                <div className="text-right">
                                    <div className="text-sm text-muted-foreground">Aircraft</div>
                                    <div className="font-medium">{flight.schedule.aircraft.aircraftModel}</div>
                                </div>
                            </div>
                            <Separator />
                            <div className="grid gap-6 md:grid-cols-3">
                                <div className="space-y-2">
                                    <div className="flex items-start gap-2">
                                        <MapPin className="mt-1 h-4 w-4 text-muted-foreground" />
                                        <div>
                                            <div className="text-xs text-muted-foreground">Departure</div>
                                            <div className="text-lg font-bold">{flight.schedule.route.originAirport.airportCode}</div>
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
                                </div>
                                <div className="space-y-2">
                                    <div className="flex items-start gap-2">
                                        <MapPin className="mt-1 h-4 w-4 text-muted-foreground" />
                                        <div>
                                            <div className="text-xs text-muted-foreground">Arrival</div>
                                            <div className="text-lg font-bold">{flight.schedule.route.destinationAirport.airportCode}</div>
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
                        </CardContent>
                    </Card>

                    <Card className="print-border">
                        <CardHeader>
                            <CardTitle className="flex items-center gap-2"><User className="h-5 w-5" /> Passenger Details</CardTitle>
                        </CardHeader>
                        <CardContent>
                            <div className="space-y-3">
                                {booking.passengers?.map((bookingPassenger, idx) => {
                                    const seatInfo = seatMap.get(bookingPassenger.flightSeatStatusId);
                                    const passengerInfo = passengerMap.get(bookingPassenger.passengerId);

                                    if (!passengerInfo) return null;

                                    return (
                                        <div key={bookingPassenger.bookingPassengerId} className="flex items-center justify-between rounded-lg border-2 p-4">
                                            <div className="flex items-center gap-4">
                                                <div className="flex h-10 w-10 items-center justify-center rounded-full bg-primary/10 font-bold text-primary">{idx + 1}</div>
                                                <div>
                                                    <div className="font-semibold">{passengerInfo.firstName} {passengerInfo.lastName}</div>
                                                    <div className="text-sm text-muted-foreground">Ticket: {bookingPassenger.ticketNumber}</div>
                                                </div>
                                            </div>
                                            {seatInfo && (
                                                <div className="text-right">
                                                    <div className="text-xs text-muted-foreground">Seat</div>
                                                    <div className="text-2xl font-bold text-primary">{seatInfo.aircraftSeat.seatNumber}</div>
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
                                    <span className="text-muted-foreground">Subtotal</span>
                                    <span className="font-medium">₹{booking.subtotalAmount.toLocaleString()}</span>
                                </div>
                                <div className="flex justify-between text-sm">
                                    <span className="text-muted-foreground">Taxes & Fees</span>
                                    <span className="font-medium">₹{booking.taxAmount.toLocaleString()}</span>
                                </div>
                                <Separator className="my-3" />
                                <div className="flex justify-between text-xl font-bold">
                                    <span>Total Paid</span>
                                    <span className="text-green-600">₹{booking.totalAmount.toLocaleString()}</span>
                                </div>
                                <div className="rounded-lg bg-blue-50 p-3 text-sm mt-2">
                                    <div className="font-medium text-blue-900">Payment Status: {booking.paymentStatus}</div>
                                </div>
                            </div>
                        </CardContent>
                    </Card>

                </div>
            </div>
        </>
    );
}