import { authApiClient } from './authApiClient';
import type { 
  LoginRequest, 
  LoginResponse, 
  RegisterRequest,
  RegisterResponse,
} from '@/types/auth';


const login = async (credentials: LoginRequest): Promise<LoginResponse> => {
  const response = await authApiClient.post<LoginResponse>('/login', credentials);
  return response.data;
};

const register = async (data: RegisterRequest): Promise<RegisterResponse> => {
  const response = await authApiClient.post<RegisterResponse>('/register', data);
  return response.data;
};

const logout = async (): Promise<void> => {
  const refreshToken = localStorage.getItem('refreshToken');
  await authApiClient.post('/logout', { refreshToken });
};

export const authService = {
  login,
  register,
  logout,
};

