import { AccountLayout } from "@/components/layouts/AccountLayout";
import { UserProfileForm } from "@/components/pages/account/UserProfileForm";
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
        path: "onboarding",
        element: <OnboardingPage/>
    }
  ],
};