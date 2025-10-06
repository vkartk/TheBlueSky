import OnboardingPage from "@/pages/account/Onboarding";


export const accountRoutes = {
  path: "account",
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