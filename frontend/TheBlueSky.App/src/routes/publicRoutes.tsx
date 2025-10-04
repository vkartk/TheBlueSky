import type { RouteObject } from 'react-router';
import { MainLayout } from '@/components/layouts/MainLayout';
import LoginPage from '@/pages/auth/login';
import RegisterPage from '@/pages/auth/register';

export const publicRoutes: RouteObject[] = [
  {
    path: '/',
    element: <MainLayout />,
    children: [
      {
        index: true,
        element: <h1>Home</h1>,
      },
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
];