import { BrowserRouter, Route, Routes } from "react-router-dom";

import NavBar from "./components/NavBar/NavBar";
import VehicleList from "./components/Vehicle/VehicleList";
import AddVehicle from "./components/Vehicle/AddVehicle";
import CategoryManagement from "./components/Category/CategoryManagement";

function App() {
  return (
    <BrowserRouter>
      <NavBar />

      <main className="container">
        <Routes>
          <Route path="/" element={<VehicleList />} />
          <Route path="/add-vehicle" element={<AddVehicle />} />
          <Route path="/categories" element={<CategoryManagement />} />
        </Routes>
      </main>
    </BrowserRouter>
  );
}

export default App;
