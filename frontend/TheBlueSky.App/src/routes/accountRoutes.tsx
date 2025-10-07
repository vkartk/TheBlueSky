import { AccountLayout } from "@/components/layouts/AccountLayout";
import OnboardingPage from "@/pages/account/Onboarding";


export const accountRoutes = {
  path: "account",
  element: <AccountLayout/>,
  handle: { crumb: "Dashboard" },
  children: [
    {
      index: true,
      element: <h1>Home</h1>,
      handle: { crumb: "Home" },
    },
    {
        path: "onboarding",
        element: <OnboardingPage/>
    }
  ],
};