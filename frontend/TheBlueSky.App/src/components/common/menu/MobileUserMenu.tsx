import { User, Bell, Badge, LayoutDashboard, Settings, LogOut } from "lucide-react";
import { Button } from "@/components/ui/button";
import { useAppSelector } from "@/store";

export const MobileUserMenu = () => {

    const { isAuthenticated, user } = useAppSelector((state) => state.auth);

    if (!isAuthenticated || !user) {
        return (
            <div className="flex flex-col gap-2">
                <Button variant="outline" className="w-full">Login</Button>
                <Button className="w-full bg-blue-500 hover:bg-blue-600">Sign Up</Button>
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
            <Button variant="ghost" className="w-full justify-start">
                <Bell className="mr-2 h-4 w-4" />
                Notifications
                <Badge className="ml-auto bg-red-500 text-xs">3</Badge>
            </Button>
            <Button variant="ghost" className="w-full justify-start">
                <User className="mr-2 h-4 w-4" />
                Profile
            </Button>
            <Button variant="ghost" className="w-full justify-start">
                <LayoutDashboard className="mr-2 h-4 w-4" />
                Dashboard
            </Button>
            <Button variant="ghost" className="w-full justify-start">
                <Settings className="mr-2 h-4 w-4" />
                Settings
            </Button>
            <Button variant="ghost" className="w-full justify-start text-red-600">
                <LogOut className="mr-2 h-4 w-4" />
                Logout
            </Button>
        </div>
    );
}
