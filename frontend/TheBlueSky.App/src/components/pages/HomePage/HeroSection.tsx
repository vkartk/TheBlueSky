import { FlightSearchForm } from "./flight-search/FlightSearchForm";

export const HeroSection: React.FC = () => (
    
  <section className="relative h-[600px] md:h-[700px] flex items-center justify-center text-white">
    <div className="absolute inset-0 bg-black/50 z-10"></div>
    <img src="/images/home/hero-bg.jpg" alt="Aircraft wing in the sky" className="absolute inset-0 w-full h-full object-cover"/>
    <div className="relative z-20 container mx-auto px-4 grid md:grid-cols-2 gap-8 items-center">
      <div className="space-y-4 text-center md:text-left">
        <h1 className="text-4xl md:text-6xl font-bold tracking-tight">
          Your Journey Begins with TheBlueSky
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