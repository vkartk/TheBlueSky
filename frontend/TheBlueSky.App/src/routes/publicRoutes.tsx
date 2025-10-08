import type { RouteObject } from 'react-router';
import  MainLayout  from '@/components/layouts/MainLayout';

import LoginPage from '@/pages/auth/login';
import RegisterPage from '@/pages/auth/register';

import NotFoundPage from '@/components/pages/errors/not-found';

import HomePage from '@/pages/public/HomePage';
import BookingPage from '@/pages/public/BookingPage';
import SearchPage from '@/pages/public/Search';

import DestinationsPage from '@/pages/public/DestinationsPage';

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
      },{
        path: 'destinations',
        element: <DestinationsPage/>
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