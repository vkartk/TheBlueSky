import { createBrowserRouter } from "react-router";
import { ProtectedRoute } from "@/components/common/ProtectedRoute";

import { publicRoutes } from "./publicRoutes";
import { dashboardRoutes } from "./dashboardRoutes";
import { accountRoutes } from "./accountRoutes";
import  MainLayout  from "@/components/layouts/MainLayout";
import { USER_ROLES } from "@/types/auth";

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
    element: <ProtectedRoute allowedRoles={USER_ROLES} />,
    children: [
      {
        element: < MainLayout />,
        children: [accountRoutes],
      }
    ],
  }
]);

export default router;