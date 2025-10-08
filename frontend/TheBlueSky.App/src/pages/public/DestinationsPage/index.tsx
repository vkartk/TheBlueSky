import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";

const destinations = [
  {
    name: "Chennai",
    description: "The vibrant coastal capital of Tamil Nadu.",
    imageUrl: "https://upload.wikimedia.org/wikipedia/commons/c/c1/Marina_Beach%2C_Chennai.jpg",
    tag: "Cultural Hub",
  },
  {
    name: "Bengaluru",
    description: "The bustling Silicon Valley of India.",
    imageUrl: "https://upload.wikimedia.org/wikipedia/commons/f/f5/Vidhana_Soudha_%2C_the_State_Legistlature_of_Karnataka%2C_Bengaluru%2C_India.jpg",
    tag: "Tech Capital",
  },
  {
    name: "Mumbai",
    description: "The energetic financial center and home of Bollywood.",
    imageUrl: "https://upload.wikimedia.org/wikipedia/commons/9/98/Marine_Drive_of_Mumbai.jpg",
    tag: "Metropolis",
  },
  {
    name: "New Delhi",
    description: "A city of rich history and political power.",
    imageUrl: "https://upload.wikimedia.org/wikipedia/commons/0/09/India_Gate_in_New_Delhi_03-2016.jpg",
    tag: "Historic",
  },
  {
    name: "Kolkata",
    description: "The cultural heart of India, known for its art and literature.",
    imageUrl: "https://upload.wikimedia.org/wikipedia/commons/9/9d/The_Howrah_Bridge%2C_Kolkata%2C_India.jpg",
    tag: "City of Joy",
  },
];


export default function DestinationsPage() {
  return (
    <div className="bg-slate-50 dark:bg-black">
      <section className="py-20 text-center bg-white dark:bg-gray-950">
        <div className="container mx-auto px-4">
          <h1 className="text-4xl font-extrabold tracking-tight sm:text-5xl md:text-6xl">
            India's Most Captivating Cities
          </h1>
          <p className="mt-6 max-w-2xl mx-auto text-lg text-muted-foreground">
            From the bustling tech parks of Bengaluru to the historic streets of Delhi, embark on a journey to discover the soul of India.
          </p>
        </div>
      </section>
      
      <main className="container mx-auto py-16 px-4">
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-8">
          {destinations.map((city) => (
            <Card 
              key={city.name} 
              className="overflow-hidden transition-transform duration-300 ease-in-out hover:-translate-y-2 hover:shadow-2xl dark:bg-gray-950 pt-0"
            >
              <CardContent className="p-0">
                <div className="relative">
                  <Badge variant="default" className="absolute top-4 right-4 text-sm">
                    {city.tag}
                  </Badge>
                  <img
                    src={city.imageUrl}
                    alt={`A placeholder image representing ${city.name}`}
                    className="w-full h-56 object-cover"
                  />
                </div>
              </CardContent>
              <CardHeader>
                <CardTitle className="text-2xl font-bold">{city.name}</CardTitle>
                <CardDescription className="pt-2 text-base">
                  {city.description}
                </CardDescription>
              </CardHeader>
            </Card>
          ))}
        </div>
      </main>
    </div>
  );
}