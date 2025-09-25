import axios from "axios";
import { AUTH_BASE_URL } from "@/config";
import type { LoginRequest, LoginResponse } from "@/types/auth";


const login = async (credentials: LoginRequest): Promise<LoginResponse> => {

  try {
    const { data } = await axios.post(`${AUTH_BASE_URL}/login`, credentials);

    return {
      success: true,
      message: data?.message ?? "Login successful.",
      accessToken: data?.accessToken,
      refreshToken: data?.refreshToken,
    };

  } catch (err: any) {

    const status = err?.response?.status;
    const data = err?.response?.data;

    let message = "Request failed. Please try again.";

    if (status === 401) {
      message = data?.message || "Invalid credentials.";
    } else if (data) {
      message = data?.message || data?.title || message;
    }

    return { success: false, message };
  }
};

const AuthService = {
    login
}
export default AuthService