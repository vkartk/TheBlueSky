import { Minus, Plus, Users } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Label } from "@/components/ui/label";
import {
    Popover,
    PopoverContent,
    PopoverTrigger,
} from "@/components/ui/popover";

export type PassengerCount = {
    adults: number;
    children: number;
    infants: number;
};

interface PassengerSelectorProps {
    value: PassengerCount;
    onChange: (newPassengers: PassengerCount) => void;
}

export const PassengerSelector = ({ value, onChange }: PassengerSelectorProps) => {

    const handlePassengerChange = (
        type: keyof PassengerCount,
        change: number
    ) => {
        const newValue = value[type] + change;
        // Enforce minimum 1 adult and no negative passengers
        if (type === "adults" && newValue < 1) return;
        if (newValue < 0) return;

        onChange({ ...value, [type]: newValue });
    };

    const totalPassengers = value.adults + value.children + value.infants;

    return (
        <Popover>
            <PopoverTrigger asChild>
                <Button
                    variant="outline"
                    className="w-full justify-start text-left font-normal"
                >
                    <Users className="mr-2 h-4 w-4" />
                    <span>
                        {totalPassengers} {totalPassengers > 1 ? "Passengers" : "Passenger"}
                    </span>
                </Button>
            </PopoverTrigger>
            <PopoverContent className="w-80 p-4">
                <div className="space-y-4">
                    <div className="flex items-center justify-between">
                        <Label htmlFor="adults">Adults (12+)</Label>
                        <div className="flex items-center gap-2">
                            <Button
                                variant="outline"
                                size="icon"
                                className="h-8 w-8"
                                onClick={() => handlePassengerChange("adults", -1)}
                            >
                                <Minus className="h-4 w-4" />
                            </Button>
                            <span className="w-6 text-center">{value.adults}</span>
                            <Button
                                variant="outline"
                                size="icon"
                                className="h-8 w-8"
                                onClick={() => handlePassengerChange("adults", 1)}
                            >
                                <Plus className="h-4 w-4" />
                            </Button>
                        </div>
                    </div>
                    {/* Repeat for Children and Infants */}
                    <div className="flex items-center justify-between">
                        <Label htmlFor="children">Children (2-11)</Label>
                        <div className="flex items-center gap-2">
                            <Button variant="outline" size="icon" className="h-8 w-8" onClick={() => handlePassengerChange("children", -1)}><Minus className="h-4 w-4" /></Button>
                            <span className="w-6 text-center">{value.children}</span>
                            <Button variant="outline" size="icon" className="h-8 w-8" onClick={() => handlePassengerChange("children", 1)}><Plus className="h-4 w-4" /></Button>
                        </div>
                    </div>
                    <div className="flex items-center justify-between">
                        <Label htmlFor="infants">Infants (under 2)</Label>
                        <div className="flex items-center gap-2">
                            <Button variant="outline" size="icon" className="h-8 w-8" onClick={() => handlePassengerChange("infants", -1)}><Minus className="h-4 w-4" /></Button>
                            <span className="w-6 text-center">{value.infants}</span>
                            <Button variant="outline" size="icon" className="h-8 w-8" onClick={() => handlePassengerChange("infants", 1)}><Plus className="h-4 w-4" /></Button>
                        </div>
                    </div>
                </div>
            </PopoverContent>
        </Popover>
    );
}