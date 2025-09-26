"use client"

import * as React from "react"
import {
  AudioWaveform,
  BookOpen,
  Bot,
  Command,
  GalleryVerticalEnd,
  Map,
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
          url: "#",
        },
        {
          title: "Cancellations",
          url: "#",
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
          url: "#",
        },
        {
          title: "Passengers",
          url: "#",
        }
      ],
    },
    {
      title: "Meals",
      url: "#",
      icon: AudioWaveform,
      items: [
        {
          title: "All Meals",
          url: "#",
        }
      ],
    },
    {
      title: "Payments",
      url: "#",
      icon: BookOpen,
      items: [
        {
          title: "All Payments",
          url: "#",
        },
        {
          title: "Refunds",
          url: "#",
        }
      ],
    },
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
          url: "#",
        },
        {
          title: "Routes",
          url: "#",
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
          url: "#",
        },
        {
          title: "Seat Classes",
          url: "#",
        }
      ],
    },
    {
      title: "Operations",
      url: "#",
      icon: AudioWaveform,
      items: [
        {
          title: "Flight Scheduling",
          url: "#",
        },
        {
          title: "Flights",
          url: "#",
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
                  <Command className="size-4" />
                </div>
                <div className="grid flex-1 text-left text-sm leading-tight">
                  <span className="truncate font-medium">The Blue Sky</span>
                  <span className="truncate text-xs">Enterprise</span>
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
