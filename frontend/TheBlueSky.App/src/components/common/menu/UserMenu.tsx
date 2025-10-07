import {
    Bell,
    Badge,
    User,
    LayoutDashboard,
    Settings,
    LogOut,
    Users,
    Ticket
} from "lucide-react";
import { Link, useNavigate } from "react-router";

import { Button } from "@/components/ui/button";
import {
    DropdownMenu,
    DropdownMenuTrigger,
    DropdownMenuContent,
    DropdownMenuLabel,
    DropdownMenuSeparator,
    DropdownMenuItem
} from "@/components/ui/dropdown-menu";


import { useAppSelector } from "@/store";
import { useAuthActions } from "@/hooks/useAuthActions";


export const UserMenu = () => {

    const { isAuthenticated, user } = useAppSelector((state) => state.auth);
    const { handleLogout } = useAuthActions();

    const navigate = useNavigate();


    if (!isAuthenticated || !user) {
        return (
            <div className="flex items-center gap-3">
                <Button asChild variant="ghost">
                    <Link to="/login" aria-label="Login">Login</Link>
                </Button>

                <Button asChild className="bg-blue-500 hover:bg-blue-600">
                    <Link to="/register" aria-label="Sign up">Sign Up</Link>
                </Button>

            </div>
        );
    }

    const fullName = [user.firstName, user.lastName].filter(Boolean).join(" ");

    return (
        <div className="flex items-center gap-4">

            <Button variant="ghost" size="icon" className="relative">
                <Bell className="h-5 w-5 text-slate-600" />
                <Badge className="absolute -top-1 -right-1 h-5 w-5 flex items-center justify-center p-0 bg-red-500 text-xs">
                    3
                </Badge>
            </Button>


            <DropdownMenu>
                <DropdownMenuTrigger asChild>
                    <Button variant="ghost" className="flex items-center gap-2">
                        <div className="h-8 w-8 rounded-full bg-blue-100 flex items-center justify-center">
                            <User className="h-5 w-5 text-blue-600" />
                        </div>
                        <span className="text-sm font-medium hidden sm:inline">{fullName}</span>
                    </Button>
                </DropdownMenuTrigger>
                <DropdownMenuContent align="end" className="w-56">
                    <DropdownMenuLabel>
                        <div>
                            <p className="font-medium">{fullName}</p>
                            <p className="text-xs text-slate-500 font-normal">{user.email}</p>
                        </div>
                    </DropdownMenuLabel>
                    <DropdownMenuSeparator />
                    <DropdownMenuItem onClick={ () => navigate("/account")}>
                        <User className="mr-2 h-4 w-4" />
                        Profile
                    </DropdownMenuItem>
                    <DropdownMenuItem onClick={ () => navigate("/account/passengers")}>
                        <Users className="mr-2 h-4 w-4" />
                        Passengers
                    </DropdownMenuItem >
                    <DropdownMenuItem onClick={ () => navigate("/account/bookings")}>
                        <Ticket className="mr-2 h-4 w-4" />
                        Bookings
                    </DropdownMenuItem>
                    <DropdownMenuSeparator />
                    <DropdownMenuItem className="text-red-600" onClick={handleLogout}>
                        <LogOut className="mr-2 h-4 w-4" />
                        Logout
                    </DropdownMenuItem>
                </DropdownMenuContent>
            </DropdownMenu>
        </div>
    );
}