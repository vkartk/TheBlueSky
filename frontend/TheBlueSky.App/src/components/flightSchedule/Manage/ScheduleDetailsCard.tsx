import { Card, CardHeader, CardTitle, CardContent } from "@/components/ui/card";
import { selectScheduleDetails } from "@/features/flightSchedule/Manage/flightScheduleManageSlice";

export const ScheduleDetailsCard = ({ details }: { details: ReturnType<typeof selectScheduleDetails> }) => {
    if (!details) {
        return (
            <Card>
                <CardHeader><CardTitle>Schedule Details</CardTitle></CardHeader>
                <CardContent>Loading details...</CardContent>
            </Card>
        );
    }
    return (
        <Card>
            <CardHeader>
                <CardTitle>Schedule Details</CardTitle>
            </CardHeader>
            <CardContent className="grid grid-cols-2 md:grid-cols-4 gap-4">
                <div><p className="text-sm text-muted-foreground">Flight Number</p><p>{details.flightNumber}</p></div>
                <div><p className="text-sm text-muted-foreground">Route ID</p><p>{details.routeId}</p></div>
                <div><p className="text-sm text-muted-foreground">Departure</p><p>{details.departureTime}</p></div>
                <div><p className="text-sm text-muted-foreground">Arrival</p><p>{details.arrivalTime}</p></div>
            </CardContent>
        </Card>
    );
};