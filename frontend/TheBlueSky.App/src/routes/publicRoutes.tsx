import type { RouteObject } from 'react-router';
import { MainLayout } from '@/components/layouts/MainLayout';
import LoginPage from '@/pages/auth/login';
import RegisterPage from '@/pages/auth/register';
import HomePage from '@/pages/public/HomePage';
import SearchPage from '@/pages/public/Search';
import NotFoundPage from '@/components/pages/errors/not-found';
import BookingPage from '@/pages/public/BookingPage';

export const publicRoutes: RouteObject[] = [
  {
    path: '/',
    element: <MainLayout />,
    children: [
      {
        index: true,
        element: <HomePage />,
      },
      {
        path: 'search',
        element: <SearchPage />
      },
      {
        path: 'booking',
        element: <BookingPage />
      }
    ],
  },
  {
    path: 'login',
    element: <LoginPage />,
  },
  {
    path: 'register',
    element: <RegisterPage />,
  },
  {
    path: '*',
    element: <NotFoundPage />
  }
];