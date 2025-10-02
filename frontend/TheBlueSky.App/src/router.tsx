import { createBrowserRouter } from "react-router";

import DashboardLayout from "@/layouts/dashboard";
import LoginPage from "@/pages/auth/login";
import RegisterPage from "@/pages/auth/register";
import DashboardHome from "@/pages/dashboard";
import { AirportsPage } from "@/pages/dashboard/Airports";
import RoutePage from "@/pages/dashboard/Route";
import SeatClassesPage from "@/pages/dashboard/seatClass";
import  AircraftsPage  from "@/pages/dashboard/Aircrafts";
import  AircraftEditPage  from "@/pages/dashboard/Aircrafts/edit";
import  FlightSchedulesPage  from "@/pages/dashboard/flightSchedule";
import { ManageFlightSchedulePage } from "@/pages/dashboard/flightSchedule/manage";

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
        path: "aircrafts",
        element: <AircraftsPage />,
        handle: { crumb: "Aircrafts" },
      },
      {
        path: "aircrafts/edit/:aircraftId",
        element: <AircraftEditPage />,
        handle: { crumb: "Edit Aircraft" },
      },
      {
        path: "seat-classes",
        element: <SeatClassesPage />,
        handle: { crumb: "Seat Classes" },
      },
      {
        path: "flight-schedules",
        element: <FlightSchedulesPage />,
        handle: { crumb: "Flight Schedules" },
      },
      {
        path: "flight-schedules/manage/:scheduleId",
        element: <ManageFlightSchedulePage />,
        handle: { crumb: "Manage Flight Schedule" },
      }
    ],
  },
])

export default router;