import { createBrowserRouter } from "react-router";

import DashboardLayout from "@/layouts/dashboard";
import LoginPage from "@/pages/auth/login";
import RegisterPage from "@/pages/auth/register";
import DashboardHome from "@/pages/dashboard";
import { AirportsPage } from "@/pages/dashboard/Airports";
import RoutePage from "@/pages/dashboard/Route";
import SeatClassesPage from "@/pages/dashboard/seatClass";

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
      },
      {
        path: "routes",
        element: <RoutePage />,
        handle: { crumb: "Routes" },
      },
      {
        path: "seat-classes",
        element: <SeatClassesPage />,
        handle: { crumb: "Seat Classes" },
      }
    ],
  },
])

export default router;