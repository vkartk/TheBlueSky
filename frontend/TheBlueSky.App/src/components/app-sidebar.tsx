"use client"

import * as React from "react"
import {
  AudioWaveform,
  Bot,
  GalleryVerticalEnd,
  Map,
  Plane,
  User,
} from "lucide-react"

import { NavMain } from "@/components/nav-main"
import { NavUser } from "@/components/nav-user"
import {
  Sidebar,
  SidebarContent,
  SidebarFooter,
  SidebarHeader,
  SidebarMenu,
  SidebarMenuButton,
  SidebarMenuItem,
  SidebarRail,
} from "@/components/ui/sidebar"

const data = {
  bookingNavMain: [
    {
      title: "Booking",
      url: "#",
      icon: Bot,
      isActive: true,
      items: [
        {
          title: "Bookings",
          url: "/dashboard/bookings",
        },
        {
          title: "Cancellations",
          url: "/dashboard/bookings/cancellations",
        }
      ],
    },
    {
      title: "Customers",
      url: "#",
      icon: User,
      items: [
        {
          title: "All Customers",
          url: "/dashboard/customers",
        },
        {
          title: "Passengers",
          url: "/dashboard/passengers",
        }
      ],
    }
  ],
  flightNavMain: [
    {
      title: "Air Traffic",
      url: "#",
      icon: Map,
      isActive: true,
      items: [
        {
          title: "Airports",
          url: "/dashboard/airports",
        },
        {
          title: "Routes",
          url: "/dashboard/routes",
        }
      ],
    },
    {
      title: "Fleet",
      url: "#",
      icon: GalleryVerticalEnd,
      items: [
        {
          title: "Aircrafts",
          url: "/dashboard/aircrafts",
        },
      ],
    },
    {
      title: "Operations",
      url: "#",
      icon: AudioWaveform,
      items: [
        {
          title: "Flight Scheduling",
          url: "/dashboard/flight-schedules",
        },
        {
          title: "Flights",
          url: "/dashboard/flights",
        }
      ],
    }
  ],
}

export function AppSidebar({ ...props }: React.ComponentProps<typeof Sidebar>) {
  return (
    <Sidebar collapsible="icon" {...props}>
      <SidebarHeader>
        <SidebarMenu>
          <SidebarMenuItem>
            <SidebarMenuButton size="lg" asChild>
              <a href="#">
                <div className="bg-sidebar-primary text-sidebar-primary-foreground flex aspect-square size-8 items-center justify-center rounded-lg">
                  <Plane className="size-4" />
                </div>
                <div className="grid flex-1 text-left text-lg leading-tight">
                  <span className="truncate font-medium">
                    The<span className="text-blue-500">BlueSky</span>
                  </span>
                  <span className="truncate text-xs">Airlines</span>
                </div>
              </a>
            </SidebarMenuButton>
          </SidebarMenuItem>
        </SidebarMenu>
      </SidebarHeader>
      <SidebarContent>
        <NavMain label="Flight" items={data.flightNavMain} />
        <NavMain label="Booking" items={data.bookingNavMain} />
      </SidebarContent>
      <SidebarFooter>
        <NavUser />
      </SidebarFooter>
      <SidebarRail />
    </Sidebar>
  )
}
