import { Plus } from 'lucide-react';
import { cn } from '@/lib/utils';
import type { AircraftSeat } from '@/types/aircraftSeat';
import { SEAT_CLASS_COLORS } from './constants';

interface SeatProps {
    seatData?: AircraftSeat;
    onClick: () => void;
}

export function Seat({ seatData, onClick }: SeatProps) {
    if (seatData) {
        const colorClass =
            seatData.seatClass ? SEAT_CLASS_COLORS[seatData.seatClass] : SEAT_CLASS_COLORS.default;

        return (
            <div
                className={cn(
                    'flex h-10 w-10 cursor-pointer items-center justify-center rounded border-2 font-semibold transition-colors',
                    colorClass
                )}
                onClick={onClick}
            >
                {seatData.seatNumber}
            </div>
        );
    }

    return (
        <div
            className="flex h-10 w-10 cursor-pointer items-center justify-center rounded border-2 border-dashed border-gray-300 text-gray-400 transition-colors hover:border-gray-500 hover:bg-gray-100"
            onClick={onClick}
        >
            <Plus size={16} />
        </div>
    );
}