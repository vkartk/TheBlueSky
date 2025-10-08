import { authApiClient } from '../auth/authApiClient';
import type { User } from '@/types/auth';


const getAllUsers = async (): Promise<User[]> => {
  const response = await authApiClient.get<User[]>('/users');
  return response.data;
};

const updateUser = async (user: User): Promise<void> => {
  await authApiClient.put(`/users/${user.userId}`, user);
};

export const userService = {
  getAllUsers,
  updateUser,
};