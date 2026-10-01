import axios from "axios";

const url = "/api/categories";

export const getCategories = () => axios.get(url);

// category = { name, iconKey, minWeightKg }
// The server works out where each category ends (where the next one starts).
export const addCategory = (category) => axios.post(url, category);

export const updateCategory = (id, category) => axios.put(`${url}/${id}`, category);

export const deleteCategory = (id) => axios.delete(`${url}/${id}`);

export const getCategoryIcons = () => axios.get(url + "/icons");
