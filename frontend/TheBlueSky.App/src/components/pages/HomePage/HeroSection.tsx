import { Badge } from "@/components/ui/badge";
import { FlightSearchForm } from "./flight-search/FlightSearchForm";

export const HeroSection: React.FC = () => (
  <section className="relative min-h-[620px] md:h-[700px] flex flex-col justify-center md:justify-center text-white pt-24 md:pt-0">
    <div className="absolute inset-0 bg-gradient-to-b from-black/80 via-black/50 to-black/20 z-10" />
    <img
      src="/images/home/hero-bg.jpg"
      alt="Aircraft wing in the sky"
      className="absolute inset-0 w-full h-full object-cover"
    />
    <div className="relative z-20 container mx-auto px-4 grid grid-cols-1 md:grid-cols-2 gap-8 items-center">
      <div className="space-y-4 text-center md:text-left">
        <Badge className="font-bold uppercase tracking-widest bg-white/90 text-slate-900 hover:bg-white">
          Ready to Fly?
        </Badge>
        <h1 className="text-4xl md:text-6xl font-bold tracking-tight leading-tight">
          Find Your Perfect,<br /> Flight Now
        </h1>
        <p className="text-lg md:text-xl text-slate-200">
          Discover seamless flight booking with unbeatable prices and 24/7 support. Your next adventure is just a click away.
        </p>
      </div>
      <div className="text-slate-900">
        <FlightSearchForm />
      </div>
    </div>
  </section>
);
