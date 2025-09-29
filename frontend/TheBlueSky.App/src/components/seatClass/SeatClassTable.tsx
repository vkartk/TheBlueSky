import { Pencil } from 'lucide-react';
import { Button } from '@/components/ui/button';
import {
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableHeader,
    TableRow,
} from '@/components/ui/table';
import type { SeatClass } from '@/types/seatClass';

interface SeatClassesTableProps {
    seatClasses: SeatClass[];
    onEdit: (seatClass: SeatClass) => void;
}

export const SeatClassesTable = ({ seatClasses, onEdit }: SeatClassesTableProps) => {

    const sortedSeatClasses = [...seatClasses].sort((a, b) => a.priorityOrder - b.priorityOrder);

    return (
        <Table>
            <TableHeader>
                <TableRow>
                    <TableHead>Class Name</TableHead>
                    <TableHead>Description</TableHead>
                    <TableHead>Priority Order</TableHead>
                    <TableHead className="text-right">Actions</TableHead>
                </TableRow>
            </TableHeader>
            <TableBody>
                {sortedSeatClasses.length > 0 ? (
                    sortedSeatClasses.map((seatClass) => (
                        <TableRow key={seatClass.seatClassId}>
                            <TableCell className="font-medium">{seatClass.className}</TableCell>
                            <TableCell>{seatClass.classDescription || 'N/A'}</TableCell>
                            <TableCell>{seatClass.priorityOrder}</TableCell>
                            <TableCell className="text-right">
                                <Button variant="ghost" size="icon" onClick={() => onEdit(seatClass)}>
                                    <Pencil className="h-4 w-4" />
                                    <span className="sr-only">Edit</span>
                                </Button>
                            </TableCell>
                        </TableRow>
                    ))
                ) : (
                    <TableRow>
                        <TableCell colSpan={4} className="h-24 text-center">
                            No seat classes found.
                        </TableCell>
                    </TableRow>
                )}
            </TableBody>
        </Table>
    );
};