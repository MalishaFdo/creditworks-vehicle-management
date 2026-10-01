import axios from "axios";

const url = "/api/manufacturers";

// Get all manufacturers.
export const getManufacturers = () => axios.get(url);
