import axios from "axios";

const url = "/api/manufacturers";

export const getManufacturers = () => axios.get(url);
