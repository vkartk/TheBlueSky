'use client';

import { useEffect, useMemo, useState } from 'react';
import { Plus } from 'lucide-react';
import { toast } from "sonner";

import { useAppDispatch, useAppSelector } from '@/store';
import {
  selectAllRoutes,
  selectRoutesLoading,
} from '@/features/routes/routesSlice';
import {
  createRoute,
  fetchRoutes,
  updateRoute,
} from '@/features/routes/routesThunks';

import {
  selectAllAirports,
} from '@/features/airports/airportsSlice';

import { Button } from '@/components/ui/button';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Input } from '@/components/ui/input';
import { Sheet, SheetTrigger } from '@/components/ui/sheet';
import { RouteTable } from '@/components/route/RouteTable';
import { RouteSheetForm } from '@/components/route/RouteSheetForm';

import type { Route, NewRoute } from '@/types/route';
import { fetchAirports } from '@/features/airports/airportsThunks';

const RoutesPage = () => {

  const dispatch = useAppDispatch();
  const routes = useAppSelector(selectAllRoutes);
  const airports = useAppSelector(selectAllAirports);
  const routesLoading = useAppSelector(selectRoutesLoading);

  const [searchTerm, setSearchTerm] = useState('');
  const [isSheetOpen, setSheetOpen] = useState(false);
  const [selectedRoute, setSelectedRoute] = useState<Route | null>(null);

  useEffect(() => {
    dispatch(fetchRoutes());
    dispatch(fetchAirports());
  }, [dispatch]);

  const airportMap = useMemo(() => 
    new Map(airports.map(airport => [airport.airportId, airport])),
    [airports]
  );

  const filteredRoutes = useMemo(() => {
    if (!searchTerm) return routes;
    
    const lowercasedSearchTerm = searchTerm.toLowerCase();

    return routes.filter(route => {
      const origin = airportMap.get(route.originAirportId);
      const destination = airportMap.get(route.destinationAirportId);
      
      const originMatch = origin?.airportCode.toLowerCase().includes(lowercasedSearchTerm) ||
                          origin?.airportName.toLowerCase().includes(lowercasedSearchTerm);
                          
      const destinationMatch = destination?.airportCode.toLowerCase().includes(lowercasedSearchTerm) ||
                               destination?.airportName.toLowerCase().includes(lowercasedSearchTerm);

      return originMatch || destinationMatch;
    });
  }, [routes, searchTerm, airportMap]);

  const handleAddNew = () => {
    setSelectedRoute(null);
    setSheetOpen(true);
  };

  const handleEdit = (route: Route) => {
    setSelectedRoute(route);
    setSheetOpen(true);
  };

  const handleSave = async (data: NewRoute | Route) => {
    const isEditing = 'routeId' in data;
    const action = isEditing ? updateRoute(data as Route) : createRoute(data as NewRoute);

    try {
      await dispatch(action).unwrap();
      toast.success(`Route ${isEditing ? 'updated' : 'created'} successfully.`);
      setSheetOpen(false);
    } catch (error: any) {
      toast.error(error || `Failed to ${isEditing ? 'update' : 'create'} route.`);
    }
  };

  const isSaving = routesLoading === 'pending';

  return (
    <div className="container mx-auto py-8">
      <Card className="p-0">
        <CardHeader className="bg-primary text-primary-foreground p-4 rounded-t-lg">
          <CardTitle>Manage Routes</CardTitle>
        </CardHeader>
        <CardContent className="p-4">
          <div className="flex items-center justify-between mb-4">
            <Input
              placeholder="Search by origin or destination airport..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              className="max-w-sm"
            />
            <Sheet open={isSheetOpen} onOpenChange={setSheetOpen}>
              <SheetTrigger asChild>
                <Button onClick={handleAddNew}>
                  <Plus className="mr-2 h-4 w-4" /> Add Route
                </Button>
              </SheetTrigger>
              <RouteSheetForm
                key={selectedRoute?.routeId ?? 'new'}
                initialData={selectedRoute}
                onSave={handleSave}
                onClose={() => setSheetOpen(false)}
                airports={airports}
                isLoading={isSaving}
              />
            </Sheet>
          </div>

          <div className="border rounded-md">
            <RouteTable 
              routes={filteredRoutes} 
              onEdit={handleEdit} 
              airports={airports}
            />
          </div>
        </CardContent>
      </Card>
    </div>
  );
};

export default RoutesPage;