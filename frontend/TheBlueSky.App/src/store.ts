import { configureStore } from '@reduxjs/toolkit';
import { type TypedUseSelectorHook, useDispatch, useSelector } from 'react-redux';

import airportsReducer from '@/features/airports/airportsSlice';
import routesReducer from '@/features/routes/routesSlice';
import aircraftsReducer from '@/features/aircrafts/aircraftsSlice';
import editAircraftReducer from '@/features/aircrafts/edit/editAircraftSlice';
import flightSchedulesReducer from '@/features/flightSchedule/flightScheduleSlice';
import flightScheduleManageReducer from '@/features/flightSchedule/Manage/flightScheduleManageSlice';
import flightsReducer from '@/features/flight/flightSlice';
import authReducer from '@/features/auth/authSlice';


export const store = configureStore({
  reducer: {
    airports: airportsReducer,
    routes: routesReducer,
    aircrafts: aircraftsReducer,
    editAircraft: editAircraftReducer,
    flightSchedules: flightSchedulesReducer,
    flightScheduleManage: flightScheduleManageReducer,
    flights: flightsReducer,
    auth: authReducer,
  },
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;

export type AppStore = typeof store;

export const useAppDispatch = () => useDispatch<AppDispatch>();
export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector;