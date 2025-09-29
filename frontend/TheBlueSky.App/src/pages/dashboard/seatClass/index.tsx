'use client';

import { useEffect, useMemo, useState } from 'react';
import { Plus } from 'lucide-react';
import { toast } from "sonner";

import { useAppDispatch, useAppSelector } from '@/store';
import { selectAllSeatClasses, selectSeatClassesLoading } from '@/features/seatClass/seatClassSlice';
import { createSeatClass, fetchSeatClasses, updateSeatClass } from '@/features/seatClass/seatClassThunks';

import { Button } from '@/components/ui/button';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Input } from '@/components/ui/input';
import { Sheet, SheetTrigger } from '@/components/ui/sheet';
import { SeatClassesTable } from '@/components/seatClass/SeatClassTable';
import { SeatClassSheetForm } from '@/components/seatClass/SeatClassSheetForm';

import type { SeatClass, NewSeatClass } from '@/types/seatClass';

const SeatClassesPage = () => {
  const dispatch = useAppDispatch();
  const seatClasses = useAppSelector(selectAllSeatClasses);
  const loading = useAppSelector(selectSeatClassesLoading);

  const [searchTerm, setSearchTerm] = useState('');
  const [isSheetOpen, setSheetOpen] = useState(false);
  const [selectedSeatClass, setSelectedSeatClass] = useState<SeatClass | null>(null);

  useEffect(() => {
    dispatch(fetchSeatClasses());
  }, [dispatch]);

  const filteredSeatClasses = useMemo(() => {
    if (!searchTerm) return seatClasses;
    const lowercasedTerm = searchTerm.toLowerCase();
    return seatClasses.filter(
      (sc) =>
        sc.className.toLowerCase().includes(lowercasedTerm) ||
        sc.classDescription?.toLowerCase().includes(lowercasedTerm)
    );
  }, [seatClasses, searchTerm]);

  const handleAddNew = () => {
    setSelectedSeatClass(null);
    setSheetOpen(true);
  };

  const handleEdit = (seatClass: SeatClass) => {
    setSelectedSeatClass(seatClass);
    setSheetOpen(true);
  };

  const handleSave = async (data: NewSeatClass | SeatClass) => {
    
    const dataToSave = 'seatClassId' in data ? data : { ...selectedSeatClass, ...data };
    
    const isEditing = !!(dataToSave as SeatClass).seatClassId;

    const action = isEditing
      ? updateSeatClass(dataToSave as SeatClass)
      : createSeatClass(dataToSave as NewSeatClass);

    try {
      await dispatch(action).unwrap();
      toast.success(`Seat Class ${isEditing ? 'updated' : 'created'} successfully.`);
      setSheetOpen(false);
    } catch (error: any) {
      toast.error(error || `Failed to ${isEditing ? 'update' : 'create'} seat class.`);
    }
  };

  const isSaving = loading === 'pending';

  return (
    <div className="container mx-auto py-8">
      <Card className="p-0">
        <CardHeader className="bg-primary text-primary-foreground p-4 rounded-t-lg">
          <CardTitle>Manage Seat Classes</CardTitle>
        </CardHeader>
        <CardContent className="p-4">
          <div className="flex items-center justify-between mb-4">
            <Input
              placeholder="Search by name or description..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              className="max-w-sm"
            />
            <Sheet open={isSheetOpen} onOpenChange={setSheetOpen}>
              <SheetTrigger asChild>
                <Button onClick={handleAddNew}>
                  <Plus className="mr-2 h-4 w-4" /> Add Seat Class
                </Button>
              </SheetTrigger>
              {isSheetOpen && (
                <SeatClassSheetForm
                  key={selectedSeatClass?.seatClassId ?? 'new'}
                  initialData={selectedSeatClass}
                  onSave={handleSave}
                  onClose={() => setSheetOpen(false)}
                  isLoading={isSaving}
                  seatClasses={seatClasses}
                />
              )}
            </Sheet>
          </div>
          
          <div className="border rounded-md">
            <SeatClassesTable seatClasses={filteredSeatClasses} onEdit={handleEdit} />
          </div>
        </CardContent>
      </Card>
    </div>
  );
};

export default SeatClassesPage;