import { flightsApiClient } from './flightsApiClient';
import type { ScheduleDay, DayOfWeek } from '@/types/scheduleDay';
import type { FlightScheduleDetails } from '@/types/generatedFlight';
import type { Flight } from '@/types/flight';

interface GenerateFlightsRequest {
  startDate: string;
  endDate: string;
}

interface GenerateFlightsResponse {
  message: string;
  count: number;
}

const getScheduleDetails = async (scheduleId: number): Promise<FlightScheduleDetails> => {
  const response = await flightsApiClient.get<FlightScheduleDetails>(`/flightSchedule/${scheduleId}`);
  return response.data;
};

const updateScheduleDays = async (scheduleId: number, days: DayOfWeek[]): Promise<ScheduleDay[]> => {
  const response = await flightsApiClient.post<ScheduleDay[]>(`/flightSchedule/${scheduleId}/scheduleDays`, days);
  return response.data;
};

const generateFlights = async (scheduleId: number, dateRange: GenerateFlightsRequest): Promise<GenerateFlightsResponse> => {
  const response = await flightsApiClient.post<GenerateFlightsResponse>(`/flightSchedule/${scheduleId}/generate`, dateRange);
  return response.data;
};

const getFlightsForSchedule = async (scheduleId: number): Promise<Flight[]> => {
  const response = await flightsApiClient.get<Flight[]>(`/flightSchedule/${scheduleId}/flights`);
  return response.data;
};

export const flightScheduleManagementService = {
  getScheduleDetails,
  updateScheduleDays,
  generateFlights,
  getFlightsForSchedule,
};