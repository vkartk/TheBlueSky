
export type LoginRequest = {
    email: string;
    password: string;
}

export type LoginResponse = {
    success: boolean;
    message?: string;
    accessToken?: string;
    refreshToken?: string;
};

export type RegisterRequest = {
    firstName: string; 
    lastName?: string;
    email: string;
    password: string;
}
export type RegisterResponse = {
    success: boolean;
    message?: string;
}