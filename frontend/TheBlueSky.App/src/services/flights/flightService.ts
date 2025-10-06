import type { Flight } from '@/types/flight';
import { flightsApiClient } from './flightsApiClient';

const API_ENDPOINT = '/flight';

const getAll = async (): Promise<Flight[]> => {
    const response = await flightsApiClient.get<Flight[]>(API_ENDPOINT);
    return response.data;
};

const getById = async (flightId: number): Promise<Flight> => {
    const response = await flightsApiClient.get<Flight>(`${API_ENDPOINT}/${flightId}`);
    return response.data;
};

const update = async (flightId: number, data: Flight): Promise<void> => {
    const response = await flightsApiClient.put(`${API_ENDPOINT}/${flightId}`, data);
    return response.data;
};

export const flightService = {
    getAll,
    getById,
    update,
};