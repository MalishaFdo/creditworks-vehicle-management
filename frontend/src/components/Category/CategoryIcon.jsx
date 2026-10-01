import { Bike, Bus, BusFront, Car, CarFront, Caravan, CircleHelp, Motorbike, Scooter, Tractor, Truck, Van } from "lucide-react";

// Match each icon name with its React icon.
const icons = {
  bicycle: Bike,
  scooter: Scooter,
  motorcycle: Motorbike,
  car: Car,
  SUV: CarFront,
  caravan: Caravan,
  van: Van,
  Minibus: BusFront,
  bus: Bus,
  truck: Truck,
  tractor: Tractor,
};

function CategoryIcon({ iconKey }) {
  const Icon = icons[iconKey] || CircleHelp; 
  return <Icon size={22} className="category-icon" aria-hidden="true" />;
}

export default CategoryIcon;
