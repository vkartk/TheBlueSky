import { createBrowserRouter } from "react-router";
import { ProtectedRoute } from "@/components/common/ProtectedRoute";

import { publicRoutes } from "./publicRoutes";
import { dashboardRoutes } from "./dashboardRoutes";

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
]);

export default router;