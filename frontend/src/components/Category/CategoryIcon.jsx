import { Bike, Bus, BusFront, Car, CarFront, Caravan, CircleHelp, Motorbike, Scooter, Tractor, Truck, Van } from "lucide-react";

// The database stores only an icon name (e.g. "truck").
// This list turns that name into a picture.
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
  const Icon = icons[iconKey] || CircleHelp; // question mark if the name is unknown
  return <Icon size={22} className="category-icon" aria-hidden="true" />;
}

export default CategoryIcon;
