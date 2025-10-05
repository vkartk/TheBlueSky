import { Search } from "lucide-react";
import { useEffect, useState } from "react";
import { toast } from "sonner";
import { format } from "date-fns";
import { useNavigate } from "react-router";

import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardDescription, CardFooter, CardHeader, CardTitle } from "@/components/ui/card";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";
import { Tabs, TabsList, TabsTrigger } from "@/components/ui/tabs";

import { selectAllRoutes } from "@/features/routes/routesSlice";
import { selectAllAirports } from "@/features/airports/airportsSlice";
import { fetchAirports } from "@/features/airports/airportsThunks";
import { fetchRoutes } from "@/features/routes/routesThunks";

import { DatePickers } from "./DatePickers";
import { PassengerSelector, type PassengerCount } from "./PassengerSelector";
import { LocationInputs } from "./LocationInputs";

import { useAppDispatch, useAppSelector } from "@/store";



export function FlightSearchForm() {
  const routes = useAppSelector(selectAllRoutes);
  const airports = useAppSelector(selectAllAirports);

  const dispatch = useAppDispatch();
  const navigate = useNavigate();

  useEffect(() => {
    dispatch(fetchRoutes());
    dispatch(fetchAirports());
  }, [dispatch]);


  const [tripType, setTripType] = useState("Round Trip");
  const [routeId, setRouteId] = useState<number>();
  const [departureDate, setDepartureDate] = useState<Date | undefined>();
  const [returnDate, setReturnDate] = useState<Date | undefined>();
  const [passengers, setPassengers] = useState<PassengerCount>({ adults: 1, children: 0, infants: 0 });
  const [flightClass, setFlightClass] = useState("Economy");

  const handleTripTypeChange = (value: string) => {
    setTripType(value);
    if (value === "One Way") {
      setReturnDate(undefined);
    }
  };

  const handleSearch = () => {

    if (!routeId || !departureDate) {
      toast.error("Please select a route and a departure date.");
      return;
    }

    const searchParams = new URLSearchParams();

    searchParams.append('routeId', routeId.toString());
    searchParams.append('departureDate', format(departureDate, 'yyyy-MM-dd'));
    searchParams.append('adults', passengers.adults.toString());
    searchParams.append('children', passengers.children.toString());
    searchParams.append('infants', passengers.infants.toString());
    searchParams.append('flightClass', flightClass);

    if (tripType === 'Round Trip') {
      if (!returnDate) {
        toast.error("Please select a return date for a round trip.");
        return;
      }
      searchParams.append('tripType', 'round-trip');
      searchParams.append('returnDate', format(returnDate, 'yyyy-MM-dd'));
    } else {
      searchParams.append('tripType', 'one-way');
    }

    navigate(`/search?${searchParams.toString()}`);

  };

  return (
    <Card className="w-full max-w-4xl mx-auto">
      <CardHeader>
        <CardTitle>Search for Flights</CardTitle>
        <CardDescription>Find the best deals for your next adventure.</CardDescription>
      </CardHeader>
      <CardContent>
        <Tabs value={tripType} onValueChange={handleTripTypeChange} className="w-full mb-6">
          <TabsList className="grid w-full grid-cols-3 md:w-[400px]">
            <TabsTrigger value="Round Trip">Round Trip</TabsTrigger>
            <TabsTrigger value="One Way">One Way</TabsTrigger>
            <TabsTrigger value="Multi-City" disabled className="relative">
              Multi-City
              <Badge variant="outline" className="absolute -top-2 -right-4 text-[10px] px-1.5 py-0.5">
                Soon
              </Badge>
            </TabsTrigger>
          </TabsList>
        </Tabs>

        <div className="space-y-6">
          <LocationInputs
            airports={airports}
            routes={routes}
            value={routeId}
            onChange={setRouteId}
          />

          <DatePickers
            tripType={tripType}
            departureDate={departureDate}
            setDepartureDate={setDepartureDate}
            returnDate={returnDate}
            setReturnDate={setReturnDate}
          />

          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <PassengerSelector value={passengers} onChange={setPassengers} />
            <Select onValueChange={setFlightClass} defaultValue={flightClass}>
              <SelectTrigger className="w-full">
                <SelectValue placeholder="Select flight class" />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="Economy">Economy</SelectItem>
                <SelectItem value="Business">Business</SelectItem>
                <SelectItem value="First Class">First Class</SelectItem>
              </SelectContent>
            </Select>
          </div>
        </div>
      </CardContent>
      <CardFooter>
        <Button className="w-full" size="lg" onClick={handleSearch}>
          <Search className="mr-2 h-5 w-5" />
          Search Flights
        </Button>
      </CardFooter>
    </Card>
  );
}