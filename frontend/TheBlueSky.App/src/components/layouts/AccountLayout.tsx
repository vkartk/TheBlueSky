import { Outlet } from 'react-router';
import { UserCog, Users, Ticket } from 'lucide-react';
import { Separator } from '@/components/ui/separator';
import { SidebarNav } from './components/account/sidebar-nav';


const sidebarNavItems = [
    {
        title: 'Profile',
        href: '/account/',
        icon: <UserCog size={18} />,
    },
    {
        title: 'Passengers',
        href: '/account/passengers',
        icon: <Users size={18} />,
    },
    {
        title: 'Bookings',
        href: '/account/bookings',
        icon: <Ticket size={18} />,
    }
]

export const AccountLayout = () => {
    return (
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:p-8" >
            <div className='space-y-0.5'>
                <h1 className='text-2xl font-bold tracking-tight md:text-3xl'>
                    My Account
                </h1>
                <p className='text-muted-foreground'>
                    Manage your account settings and manage your bookings.
                </p>
            </div><Separator className='my-4 lg:my-6' /><div className='flex flex-1 flex-col space-y-2 overflow-hidden md:space-y-2 lg:flex-row lg:space-y-0 lg:space-x-12'>
                <aside className='top-0 lg:sticky lg:w-1/5'>
                    <SidebarNav items={sidebarNavItems} />
                </aside>
                <div className='flex w-full overflow-y-hidden p-1'>
                    <Outlet />
                </div>
            </div>
        </div>
    );
}