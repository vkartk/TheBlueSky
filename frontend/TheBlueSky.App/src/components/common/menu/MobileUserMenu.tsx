import { User, Badge, LogOut, Users, Ticket } from "lucide-react";
import { Link } from "react-router";

import { Button } from "@/components/ui/button";
import { useAppSelector } from "@/store";
import { useAuthActions } from "@/hooks/useAuthActions";

export const MobileUserMenu = () => {

    const { isAuthenticated, user } = useAppSelector((state) => state.auth);
    const { handleLogout } = useAuthActions();

    if (!isAuthenticated || !user) {
        return (
            <div className="flex flex-col gap-2">
                <Button asChild variant="outline" className="w-full">
                    <Link to="/login" aria-label="Login">
                        Login
                    </Link>
                </Button>

                <Button asChild className="w-full bg-blue-500 hover:bg-blue-600">
                    <Link to="/register" aria-label="Sign up">
                        Sign Up
                    </Link>
                </Button>

            </div>
        );
    }

    const fullName = [user.firstName, user.lastName].filter(Boolean).join(" ");

    return (
        <div className="space-y-2">
            <div className="flex items-center gap-3 px-3 py-2">
                <div className="h-10 w-10 rounded-full bg-blue-100 flex items-center justify-center">
                    <User className="h-5 w-5 text-blue-600" />
                </div>
                <div>
                    <p className="font-medium text-sm">{fullName}</p>
                    <p className="text-xs text-slate-500">{user.email}</p>
                </div>
            </div>

            <Link to="/account">
                <Button variant="ghost" className="w-full justify-start">
                    <User className="mr-2 h-4 w-4" />
                    Profile
                </Button>
            </Link>
            <Link to="/account/passengers">
                <Button variant="ghost" className="w-full justify-start">
                    <Users className="mr-2 h-4 w-4" />
                    Passengers
                </Button>
            </Link>
            <Link to="/account/bookings">
                <Button variant="ghost" className="w-full justify-start">
                    <Ticket className="mr-2 h-4 w-4" />
                    Bookings
                    <Badge className="ml-auto bg-red-500 text-xs">3</Badge>
                </Button>
            </Link>
            <Button variant="ghost" className="w-full justify-start text-red-600" onClick={handleLogout}>
                <LogOut className="mr-2 h-4 w-4" />
                Logout
            </Button>
        </div>
    );
}
