import { configureStore } from '@reduxjs/toolkit';
import { type TypedUseSelectorHook, useDispatch, useSelector } from 'react-redux';

import airportsReducer from '@/features/airports/airportsSlice';
import routesReducer from '@/features/routes/routesSlice';
import seatClassReducer from '@/features/seatClass/seatClassSlice';
import aircraftsReducer from '@/features/aircrafts/aircraftsSlice';
import editAircraftReducer from '@/features/aircrafts/edit/editAircraftSlice';
import flightSchedulesReducer from '@/features/flightSchedule/flightScheduleSlice';
import flightScheduleManageReducer from '@/features/flightSchedule/Manage/flightScheduleManageSlice';

export const store = configureStore({
  reducer: {
    airports: airportsReducer,
    routes: routesReducer,
    seatClasses: seatClassReducer,
    aircrafts: aircraftsReducer,
    editAircraft: editAircraftReducer,
    flightSchedules: flightSchedulesReducer,
    flightScheduleManage: flightScheduleManageReducer,
  },
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;

export const useAppDispatch = () => useDispatch<AppDispatch>();
export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector;