export type Passenger = {
  passengerId: number;
  managedByUserId: string;
  firstName: string;
  lastName: string;
  dateOfBirth: string;
  gender?: string | null;
  passportNumber?: string | null;
  nationalityCountryId?: string | null;
  relationshipToManager?: string | null;
  createdDate: string;
  isActive: boolean;
};

export type NewPassenger = Omit<Passenger, 'passengerId' | 'createdDate'>;