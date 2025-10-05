import { ArrowRightLeft, PlaneLanding, PlaneTakeoff } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";

interface LocationInputsProps {
    origin: string;
    setOrigin: (value: string) => void;
    destination: string;
    setDestination: (value: string) => void;
}

export const LocationInputs = ({
    origin,
    setOrigin,
    destination,
    setDestination,
}: LocationInputsProps) => {
    
    const handleSwap = () => {
        setOrigin(destination);
        setDestination(origin);
    };

    return (
        <div className="flex flex-col md:flex-row items-center gap-4">
            <div className="relative w-full">
                <PlaneTakeoff className="absolute left-3 top-1/2 -translate-y-1/2 h-5 w-5 text-muted-foreground" />
                <Input
                    type="text"
                    placeholder="From"
                    className="pl-10"
                    value={origin}
                    onChange={(e) => setOrigin(e.target.value)}
                />
            </div>
            <Button
                variant="ghost"
                size="icon"
                className="flex-shrink-0"
                onClick={handleSwap}
                aria-label="Swap origin and destination"
            >
                <ArrowRightLeft className="h-5 w-5" />
            </Button>
            <div className="relative w-full">
                <PlaneLanding className="absolute left-3 top-1/2 -translate-y-1/2 h-5 w-5 text-muted-foreground" />
                <Input
                    type="text"
                    placeholder="To"
                    className="pl-10"
                    value={destination}
                    onChange={(e) => setDestination(e.target.value)}
                />
            </div>
        </div>
    );
}