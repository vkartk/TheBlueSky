import { AccountLayout } from "@/components/layouts/AccountLayout";
import { UserProfileForm } from "@/components/pages/account/UserProfileForm";
import MyBookingsPage from "@/pages/account/MyBookings";
import { ViewTicketPage } from "@/pages/account/MyBookings/ViewTicketPage";
import OnboardingPage from "@/pages/account/Onboarding";
import PassengersPage from "@/pages/account/passengers";


export const accountRoutes = {
  path: "account",
  element: <AccountLayout/>,
  handle: { crumb: "Dashboard" },
  children: [
    {
      index: true,
      element: <UserProfileForm/>,
      handle: { crumb: "Home" },
    },
    {
      path: "passengers",
      element: <PassengersPage/>,
      handle: { crumb: "Passengers" },
    },
    {
      path: "bookings",
      element: <MyBookingsPage />,
      handle: { crumb: "My Bookings" },
    },
    {
      path: "ticket/:bookingId/:flightId",
      element: <ViewTicketPage />,
      handle: { crumb: "View Ticket" },
    },
    {
        path: "onboarding",
        element: <OnboardingPage/>
    }
  ],
};