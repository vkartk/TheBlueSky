import { Plane, Mail, Phone, Facebook, Twitter, Instagram, Linkedin } from 'lucide-react';
import { Input } from '@/components/ui/input';
import { Button } from '@/components/ui/button';

export default function Footer() {
  return (
    <footer className="bg-slate-50 text-slate-700 border-t border-slate-200">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        
        <section className="py-12 grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-8">
          
          <div className="space-y-4">
            <div className="flex items-center gap-2 font-bold">
              <Plane className="h-12 w-12 text-blue-500" />
              <span className="text-3xl text-slate-900">The<span className="text-blue-500">BlueSky</span></span>
            </div>
            <p className="text-sm text-slate-600">
              Your trusted partner for seamless flight bookings and unforgettable travel experiences around the globe.
            </p>
            <div className="flex gap-4">
              <a href="#" className="hover:text-blue-500 transition-colors">
                <Facebook className="h-5 w-5" />
              </a>
              <a href="#" className="hover:text-blue-500 transition-colors">
                <Twitter className="h-5 w-5" />
              </a>
              <a href="#" className="hover:text-blue-500 transition-colors">
                <Instagram className="h-5 w-5" />
              </a>
              <a href="#" className="hover:text-blue-500 transition-colors">
                <Linkedin className="h-5 w-5" />
              </a>
            </div>
          </div>

          <div>
            <h3 className="text-slate-900 font-semibold mb-4">Quick Links</h3>
            <ul className="space-y-2 text-sm">
              <li><a href="#" className="hover:text-blue-500 transition-colors">About Us</a></li>
              <li><a href="#" className="hover:text-blue-500 transition-colors">Our Fleet</a></li>
              <li><a href="#" className="hover:text-blue-500 transition-colors">Destinations</a></li>
              <li><a href="#" className="hover:text-blue-500 transition-colors">Flight Schedule</a></li>
            </ul>
          </div>

          <div>
            <h3 className="text-slate-900 font-semibold mb-4">Customer Support</h3>
            <ul className="space-y-2 text-sm">
              <li><a href="#" className="hover:text-blue-500 transition-colors">Help Center</a></li>
              <li><a href="#" className="hover:text-blue-500 transition-colors">Manage Booking</a></li>
              <li><a href="#" className="hover:text-blue-500 transition-colors">Cancellation Policy</a></li>
              <li><a href="#" className="hover:text-blue-500 transition-colors">Baggage Information</a></li>
              <li><a href="#" className="hover:text-blue-500 transition-colors">FAQ</a></li>
            </ul>
          </div>

          <div>
            <h3 className="text-slate-900 font-semibold mb-4">Stay Updated</h3>
            <p className="text-sm text-slate-600 mb-4">Subscribe to get special offers and travel updates.</p>
            <div className="flex gap-2">
              <Input 
                type="email" 
                placeholder="Your email" 
                className="bg-white border-slate-300"
              />
              <Button className="bg-blue-500 hover:bg-blue-600">
                <Mail className="h-4 w-4" />
              </Button>
            </div>
            <div className="mt-6 space-y-2 text-sm">
              <div className="flex items-center gap-2">
                <Phone className="h-4 w-4 text-blue-500" />
                <span>+1 (800) 123-4567</span>
              </div>
              <div className="flex items-center gap-2">
                <Mail className="h-4 w-4 text-blue-500" />
                <span>support@thebluesky.app</span>
              </div>
            </div>
          </div>
        </section>

        <div className="border-t border-slate-200 py-6">
          <div className="flex flex-col md:flex-row justify-between items-center gap-4">
            <p className="text-sm text-slate-600">
              &copy; 2025 TheBlueSky Airlines. All rights reserved.
            </p>
            <div className="flex gap-6 text-sm">
              <a href="#" className="hover:text-blue-500 transition-colors">Privacy Policy</a>
              <a href="#" className="hover:text-blue-500 transition-colors">Terms of Service</a>
            </div>
          </div>
        </div>
      </div>
    </footer>
  );
}