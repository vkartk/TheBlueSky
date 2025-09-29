export type SeatClass = {
  seatClassId: number;
  className: string;
  classDescription: string | null;
  priorityOrder: number;
};

export type NewSeatClass = Omit<SeatClass, 'seatClassId'>;