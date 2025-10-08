import DashboardLayout from "@/components/layouts/dashboard";

import DashboardHome from "@/pages/dashboard";

import  AirportsPage from "@/pages/dashboard/Airports";
import AircraftsPage from "@/pages/dashboard/Aircrafts";
import AircraftEditPage from "@/pages/dashboard/Aircrafts/edit";

import FlightsPage from "@/pages/dashboard/Flight";
import RoutePage from "@/pages/dashboard/Route";

import FlightSchedulesPage from "@/pages/dashboard/FlightSchedule";
import ManageFlightSchedulePage  from "@/pages/dashboard/FlightSchedule/Manage";

import  CustomersPage  from "@/pages/dashboard/Customers";
import PassengersPage from "@/pages/dashboard/Passengers";

import  BookingsPage  from "@/pages/dashboard/Bookings";
import BookingCancellationsPage from "@/pages/dashboard/BookingCancellations";

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
    {
      path: "customers",
      element: <CustomersPage/>,
      handle: { crumb: "Customers" },
    },
    {
      path: "passengers",
      element: <PassengersPage />,
      handle: { crumb: "Passengers" },
    },
    {
      path: "bookings",
      element: <BookingsPage />,
      handle: { crumb: "Bookings" }
    },
    {
      path: "bookings/cancellations",
      element: <BookingCancellationsPage />,
      handle: { crumb: "BookingCancellations" }
    }
  ],
};