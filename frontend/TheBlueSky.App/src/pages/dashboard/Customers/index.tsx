import { useState, useEffect } from 'react';
import { useAppDispatch, useAppSelector } from '@/store';
import { selectAllUsers } from '@/features/users/usersSlice';
import { fetchAllUsers, updateUser } from '@/features/users/usersThunks';

import { Sheet } from '@/components/ui/sheet';
import { UsersTable } from '@/components/users/UsersTable';
import { UserSheetForm } from '@/components/users/UserSheetForm';
import type { User } from '@/types/auth';

const CustomersPage = () => {
  const dispatch = useAppDispatch();
  const customers = useAppSelector(selectAllUsers);
  const { loading } = useAppSelector((state) => state.users);

  const [editingCustomer, setEditingCustomer] = useState<User | null>(null);
  const [isSheetOpen, setIsSheetOpen] = useState(false);

  useEffect(() => {
    dispatch(fetchAllUsers());
  }, [dispatch]);

  const handleEdit = (customer: User) => {
    setEditingCustomer(customer);
    setIsSheetOpen(true);
  };

  const handleCloseSheet = () => {
    setIsSheetOpen(false);
    setEditingCustomer(null);
  };

  const handleSave = async (data: User) => {
    await dispatch(updateUser(data));
    handleCloseSheet();
  };

  return (
    <div className="container mx-auto py-8">
      <div className="flex justify-between items-center mb-6">
        <div>
          <h1 className="text-2xl font-bold">Customers</h1>
          <p className="text-muted-foreground">
            View and edit customer details and roles.
          </p>
        </div>
      </div>

      <Sheet open={isSheetOpen} onOpenChange={setIsSheetOpen}>
        <UsersTable users={customers} onEdit={handleEdit} />
        
        {editingCustomer && (
          <UserSheetForm
            initialData={editingCustomer}
            onSave={handleSave}
            onClose={handleCloseSheet}
            isLoading={loading === 'pending'}
          />
        )}
      </Sheet>
    </div>
  );
};

export default CustomersPage;