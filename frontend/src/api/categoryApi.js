import axios from "axios";

const url = "/api/categories";

// Get all categories.
export const getCategories = () => axios.get(url);

// Add a new category.
export const addCategory = (category) => axios.post(url, category);

// Update a category.
export const updateCategory = (id, category) => axios.put(`${url}/${id}`, category);

// Delete a category.
export const deleteCategory = (id) => axios.delete(`${url}/${id}`);

// Get the available category icons.
export const getCategoryIcons = () => axios.get(url + "/icons");
