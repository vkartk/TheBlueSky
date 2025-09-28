import { createBrowserRouter } from "react-router";

import DashboardLayout from "./layouts/dashboard";
import LoginPage from "./pages/auth/login";
import RegisterPage from "./pages/auth/register";
import DashboardHome from "./pages/dashboard";
import { AirportsPage } from "./pages/dashboard/Airports";

const router = createBrowserRouter([
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
  {
    path: "dashboard",
    element: <DashboardLayout />,
    handle: { crumb: "Dashboard" },
    children: [
      {
        index: true,
        element: <DashboardHome />,
        handle: { crumb: "Home" },
      },
      {
        path: "airports",
        element: <AirportsPage />,
        handle: { crumb: "Airports" },
      }
    ],
  },
])

export default router;