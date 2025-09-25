const env = import.meta.env;

export const API_BASE_URL =  env.VITE_API_BASE_URL;

export const AUTH_BASE_URL = `${API_BASE_URL}/gateway/Auth`

export const ACCESS_KEY = "auth.accessToken";
export const REFRESH_KEY = "auth.refreshToken";
