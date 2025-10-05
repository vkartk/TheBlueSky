import { format } from "date-fns";
import { Calendar as CalendarIcon } from "lucide-react";
import { cn } from "@/lib/utils";
import { Button } from "@/components/ui/button";
import { Calendar } from "@/components/ui/calendar";
import {
    Popover,
    PopoverContent,
    PopoverTrigger,
} from "@/components/ui/popover";

interface DatePickersProps {
    tripType: string;
    departureDate?: Date;
    setDepartureDate: (date?: Date) => void;
    returnDate?: Date;
    setReturnDate: (date?: Date) => void;
}

export const DatePickers = ({
    tripType,
    departureDate,
    setDepartureDate,
    returnDate,
    setReturnDate,
}: DatePickersProps) => {

    const handleDepartureSelect = (date?: Date) => {
        setDepartureDate(date);
        // If the new departure date is after the current return date, or if departure is cleared, clear the return date
        if (!date || (returnDate && date && date > returnDate)) {
            setReturnDate(undefined);
        }
    };

    return (
        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <Popover>
                <PopoverTrigger asChild>
                    <Button
                        variant={"outline"}
                        className={cn("w-full justify-start text-left font-normal", !departureDate && "text-muted-foreground")}
                    >
                        <CalendarIcon className="mr-2 h-4 w-4" />
                        {departureDate ? format(departureDate, "PPP") : <span>Departure Date</span>}
                    </Button>
                </PopoverTrigger>
                <PopoverContent className="w-auto p-0">
                    <Calendar mode="single" selected={departureDate} onSelect={handleDepartureSelect} initialFocus />
                </PopoverContent>
            </Popover>
            <Popover>
                <PopoverTrigger asChild>
                    <Button
                        variant={"outline"}
                        disabled={tripType === "One Way"}
                        className={cn("w-full justify-start text-left font-normal", !returnDate && "text-muted-foreground")}
                    >
                        <CalendarIcon className="mr-2 h-4 w-4" />
                        {returnDate ? format(returnDate, "PPP") : <span>Return Date</span>}
                    </Button>
                </PopoverTrigger>
                <PopoverContent className="w-auto p-0">
                    <Calendar
                        mode="single"
                        selected={returnDate}
                        onSelect={setReturnDate}
                        disabled={{ before: departureDate || new Date() }} // Prevent selecting date before departure
                        autoFocus
                    />
                </PopoverContent>
            </Popover>
        </div>
    );
}