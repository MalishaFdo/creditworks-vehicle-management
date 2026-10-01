import axios from "axios";

// Vehicle API endpoint.
const url = "/api/vehicles";

// Get vehicles using the selected sorting.
export const getVehicles = (sortBy, sortDirection) =>
  axios.get(url, { params: { sortBy, sortDirection } });

// Add a new vehicle.
export const addVehicle = (vehicle) => axios.post(url, vehicle);
