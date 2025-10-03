import DashboardLayout from "@/components/layouts/dashboard";
import DashboardHome from "@/pages/dashboard";
import { AirportsPage } from "@/pages/dashboard/Airports";
import RoutePage from "@/pages/dashboard/Route";
import SeatClassesPage from "@/pages/dashboard/seatClass";
import AircraftsPage from "@/pages/dashboard/Aircrafts";
import AircraftEditPage from "@/pages/dashboard/Aircrafts/edit";
import FlightSchedulesPage from "@/pages/dashboard/flightSchedule";
import { ManageFlightSchedulePage } from "@/pages/dashboard/flightSchedule/manage";
import FlightsPage from "@/pages/dashboard/flight";

export const dashboardRoutes = {
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
    },
    {
      path: "flights",
      element: <FlightsPage />,
      handle: { crumb: "Flights" },
    },
  ],
};