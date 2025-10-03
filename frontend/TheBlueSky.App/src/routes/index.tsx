import { createBrowserRouter } from "react-router";
import { ProtectedRoute } from "@/components/common/ProtectedRoute";
import LoginPage from "@/pages/auth/login";
import RegisterPage from "@/pages/auth/register";

import { dashboardRoutes } from "./dashboardRoutes";

const router = createBrowserRouter([
  // Public Routes
  {
    path: "/",
    element: <div>Home</div>,
    handle: { crumb: "Home" },
  },
  {
    path: "login",
    element: <LoginPage />,
    handle: { crumb: "Login" },
  },
  {
    path: "register",
    element: <RegisterPage />,
    handle: { crumb: "Register" },
  },

  // Protected Routes
  {
    element: <ProtectedRoute allowedRoles={['Admin']} />,
    children: [
      dashboardRoutes, 
    ],
  },
]);

export default router;