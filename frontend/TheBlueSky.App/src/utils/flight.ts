import type { FlightStatus } from "@/types/flight";

export const getFlightStatusVariant = (status: FlightStatus) => {
  switch (status) {

    case 'Boarding':
    case 'Arrived':
      return 'default';

    case 'Scheduled':
      return 'secondary';

    case 'Departed':
      return 'outline'; 

    case 'Delayed':
    case 'Cancelled':
    case 'Diverted':
      return 'destructive';

    default:
      return 'secondary';
  }
};
