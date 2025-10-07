import { createBrowserRouter } from "react-router";
import { ProtectedRoute } from "@/components/common/ProtectedRoute";

import { publicRoutes } from "./publicRoutes";
import { dashboardRoutes } from "./dashboardRoutes";
import { accountRoutes } from "./accountRoutes";
import { MainLayout } from "@/components/layouts/MainLayout";

const router = createBrowserRouter([
  // Public Routes
  ...publicRoutes,

  // Protected Routes
  {
    element: <ProtectedRoute allowedRoles={['Admin']} />,
    children: [
      dashboardRoutes,
    ],
  },
  {
    element: <ProtectedRoute allowedRoles={['User', 'Admin']} />,
    children: [
      {
        element: < MainLayout />,
        children: [accountRoutes],
      }
    ],
  }
]);

export default router;