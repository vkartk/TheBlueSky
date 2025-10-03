import type { FlightStatus } from "@/types/flight";

export const getStatusVariant = (status: FlightStatus) => {
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
