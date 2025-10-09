import { Plane, Menu } from "lucide-react";
import { Link } from "react-router";
import { Button } from "@/components/ui/button";
import {
  NavigationMenu,
  NavigationMenuItem,
  NavigationMenuLink,
  NavigationMenuList,
} from "@/components/ui/navigation-menu";
import {
  Sheet,
  SheetContent,
  SheetHeader,
  SheetTitle,
  SheetTrigger,
} from "@/components/ui/sheet";
import { Separator } from "@/components/ui/separator";

import { UserMenu } from "./menu/UserMenu";
import { MobileUserMenu } from "./menu/MobileUserMenu";
import { NAV_LINKS } from "./constants";


const Header = () => {
  return (
    <header className="bg-white shadow-sm sticky top-0 z-50">
      <div className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8">
        <div className="flex h-16 items-center justify-between">
         
          <Link
            to="/"
            className="flex items-center gap-2 font-bold"
            aria-label="TheBlueSky home"
          >
            <Plane className="h-12 w-12 text-blue-500" />
            <span className="text-3xl">
              The<span className="text-blue-500">BlueSky</span>
            </span>
          </Link>

         
          <div className="hidden md:flex md:items-center md:gap-8">
            <NavigationMenu className="!bg-transparent">
              <NavigationMenuList>
                {NAV_LINKS.map((link) => (
                  <NavigationMenuItem key={link.label}>
                    <NavigationMenuLink asChild>
                      <Link
                        to={link.href}
                        className="text-sm font-medium text-slate-700 transition-colors hover:text-blue-500"
                      >
                        {link.label}
                      </Link>
                    </NavigationMenuLink>
                  </NavigationMenuItem>
                ))}
              </NavigationMenuList>
            </NavigationMenu>

            <UserMenu />
          </div>

          
          <div className="md:hidden">
            <Sheet>
              <SheetTrigger asChild>
                <Button variant="ghost" size="icon" aria-label="Open menu">
                  <Menu className="h-6 w-6" />
                </Button>
              </SheetTrigger>

              <SheetContent side="right" className="w-[300px] sm:w-[360px] px-4">
                <SheetHeader className="text-left">
                  <SheetTitle className="flex items-center gap-2">
                    <Plane className="h-5 w-5 text-blue-500" />
                    <span>
                      The<span className="text-blue-500">BlueSky</span>
                    </span>
                  </SheetTitle>
                </SheetHeader>

                <nav className="grid gap-1">
                  {NAV_LINKS.map((link) => (
                    <Link
                      key={link.label}
                      to={link.href}
                      className="rounded-md px-3 py-2 text-sm font-medium text-slate-700 transition-colors hover:bg-slate-50 hover:text-blue-600"
                    >
                      {link.label}
                    </Link>
                  ))}
                </nav>

                <Separator className="my-4" />

                <div className="pt-2">
                  <MobileUserMenu />
                </div>
              </SheetContent>
            </Sheet>
          </div>
        </div>
      </div>
    </header>
  );
};

export default Header;