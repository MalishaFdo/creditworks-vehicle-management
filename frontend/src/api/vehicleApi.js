import axios from "axios";

// "/api" is forwarded to the backend by vite.config.js.
const url = "/api/vehicles";

// sortBy: "ownerName" | "manufacturer" | "yearOfManufacture" | "weight"
// sortDirection: "asc" | "desc"
export const getVehicles = (sortBy, sortDirection) =>
  axios.get(url, { params: { sortBy, sortDirection } });

export const addVehicle = (vehicle) => axios.post(url, vehicle);
