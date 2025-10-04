import {
    Bell,
    Badge,
    User,
    LayoutDashboard,
    Settings,
    LogOut
} from "lucide-react";
import { Link } from "react-router";

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


export const UserMenu = () => {

    const { isAuthenticated, user } = useAppSelector((state) => state.auth);

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
                    <DropdownMenuItem>
                        <User className="mr-2 h-4 w-4" />
                        Profile
                    </DropdownMenuItem>
                    <DropdownMenuItem>
                        <LayoutDashboard className="mr-2 h-4 w-4" />
                        Dashboard
                    </DropdownMenuItem>
                    <DropdownMenuItem>
                        <Settings className="mr-2 h-4 w-4" />
                        Settings
                    </DropdownMenuItem>
                    <DropdownMenuSeparator />
                    <DropdownMenuItem className="text-red-600">
                        <LogOut className="mr-2 h-4 w-4" />
                        Logout
                    </DropdownMenuItem>
                </DropdownMenuContent>
            </DropdownMenu>
        </div>
    );
}