import { Fragment } from "react"
import { Link, Outlet, useMatches } from "react-router"

import { AppSidebar } from "@/components/app-sidebar"
import {
  Breadcrumb,
  BreadcrumbItem,
  BreadcrumbLink,
  BreadcrumbList,
  BreadcrumbPage,
  BreadcrumbSeparator,
} from "@/components/ui/breadcrumb"
import { Separator } from "@/components/ui/separator"
import {
  SidebarInset,
  SidebarProvider,
  SidebarTrigger,
} from "@/components/ui/sidebar"


type CrumbMatch = {
  pathname: string
  handle?: { crumb?: string }
}

export default function DashboardLayout() {

  const matches = useMatches() as CrumbMatch[]
  const crumbs = matches.filter(m => m.handle?.crumb)


  return (
    <SidebarProvider>
      <AppSidebar />
      <SidebarInset>
        <header className="flex h-16 shrink-0 items-center gap-2 transition-[width,height] ease-linear group-has-data-[collapsible=icon]/sidebar-wrapper:h-12">
          <div className="flex items-center gap-2 px-4">
            <SidebarTrigger className="-ml-1" />
            <Separator
              orientation="vertical"
              className="mr-2 data-[orientation=vertical]:h-4"
            />
            <Breadcrumb>
              <BreadcrumbList>
                {crumbs.map((m, i) => {
                  const isLast = i === crumbs.length - 1
                  return (
                    <Fragment key={m.pathname}>
                    <BreadcrumbItem>
                      {isLast ? (
                        <BreadcrumbPage>{m.handle?.crumb}</BreadcrumbPage>
                      ) : (
                        <BreadcrumbLink asChild>
                          <Link to={m.pathname}>{m.handle?.crumb}</Link>
                        </BreadcrumbLink>
                      )}
                    </BreadcrumbItem>
                    {!isLast && <BreadcrumbSeparator />}
                    </Fragment>
                  )
                })}
              </BreadcrumbList>

            </Breadcrumb>
          </div>
        </header>
        <Outlet />
      </SidebarInset>
    </SidebarProvider>
  )
}