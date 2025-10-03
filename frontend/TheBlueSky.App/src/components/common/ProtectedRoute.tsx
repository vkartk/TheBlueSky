import { Navigate, Outlet, useLocation } from 'react-router';
import { useAppSelector } from '@/store';

type ProtectedRouteProps = {
    allowedRoles?: string[];
};

export const ProtectedRoute = ({ allowedRoles }: ProtectedRouteProps) => {

    const { isAuthenticated, user } = useAppSelector((state) => state.auth);

    const location = useLocation();

    if (!isAuthenticated) {
        return <Navigate to="/login" state={{ from: location }} replace />;
    }

    if (allowedRoles && allowedRoles.length > 0) {

        const userHasRequiredRole = user?.roles?.some(role => allowedRoles?.includes(role));

        if (allowedRoles && !userHasRequiredRole) {
            return <Navigate to="/unauthorized" replace />;
        }
    }

    return <Outlet />;
};