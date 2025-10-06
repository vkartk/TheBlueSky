
export type LoginRequest = {
    email: string;
    password: string;
}

export type LoginResponse = {
    success: boolean;
    message?: string;
    accessToken?: string;
    refreshToken?: string;
    user: User
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

export type User = {
    userId: string;
    firstName: string,
    lastName: string,
    email: string;
    roles: string[];
}