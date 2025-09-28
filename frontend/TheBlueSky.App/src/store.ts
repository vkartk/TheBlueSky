import { configureStore } from '@reduxjs/toolkit';
import { type TypedUseSelectorHook, useDispatch, useSelector } from 'react-redux';
import airportsReducer from '@/features/airports/airportsSlice';
import routesReducer from '@/features/routes/routesSlice';

export const store = configureStore({
  reducer: {
    airports: airportsReducer,
    routes: routesReducer,
  },
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;

export const useAppDispatch = () => useDispatch<AppDispatch>();
export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector;