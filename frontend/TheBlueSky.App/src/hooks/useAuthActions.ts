import { useAppDispatch } from "@/store";
import { logoutUser } from "@/features/auth/authThunks";

export const useAuthActions = () => {

  const dispatch = useAppDispatch();
  const handleLogout = () => dispatch(logoutUser());
  
  return { handleLogout };
};