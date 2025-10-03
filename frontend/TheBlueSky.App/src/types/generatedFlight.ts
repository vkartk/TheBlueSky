import type { ScheduleDay } from "./scheduleDay";

export type FlightScheduleDetails = {
    flightScheduleId: number;
    flightNumber: string;
    aircraftId: number;
    routeId: number;
    departureTime: string; // "HH:mm:ss"
    arrivalTime: string; // "HH:mm:ss"
    scheduleDays: ScheduleDay[];
};