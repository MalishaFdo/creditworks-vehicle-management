import { useEffect, useState } from "react";
import { addCategory, deleteCategory, getCategories, getCategoryIcons, updateCategory } from "../../api/categoryApi";
import { getErrorMessage, getFieldErrors } from "../../api/apiErrors";
import FieldError from "../Common/FieldError";
import CategoryIcon from "./CategoryIcon";
import "../../styles/category.css";

const emptyForm = { name: "", iconKey: "", minWeightKg: "" };

function CategoryManagement() {
  const [categories, setCategories] = useState([]);
  const [icons, setIcons] = useState([]);
  const [form, setForm] = useState(emptyForm);
  const [editingId, setEditingId] = useState(null); // null = adding a new category
  const [fieldErrors, setFieldErrors] = useState({}); // errors from the server, per field
  const [error, setError] = useState("");
  const [successMessage, setSuccessMessage] = useState("");

  function loadCategories() {
    getCategories()
      .then((response) => setCategories(response.data))
      .catch((err) => setError(getErrorMessage(err)));
  }

  // Load the categories and the list of icons when the page opens.
  useEffect(() => {
    loadCategories();
    getCategoryIcons().then((response) => setIcons(response.data));
  }, []);

  function clearMessages() {
    setFieldErrors({});
    setError("");
    setSuccessMessage("");
  }

  function handleChange(event) {
    setForm({ ...form, [event.target.name]: event.target.value });
  }

  // Fill the form with a category so it can be changed.
  function handleEdit(category) {
    clearMessages();
    setEditingId(category.id);
    setForm({ name: category.name, iconKey: category.iconKey, minWeightKg: String(category.minWeightKg) });
  }

  function handleCancel() {
    clearMessages();
    setEditingId(null);
    setForm(emptyForm);
  }

  async function handleSubmit(event) {
    event.preventDefault();
    clearMessages();

    const category = {
      name: form.name,
      iconKey: form.iconKey,
      minWeightKg: form.minWeightKg === "" ? null : Number(form.minWeightKg),
    };

    try {
      if (editingId === null) {
        await addCategory(category);
        setSuccessMessage(`Category "${category.name}" added.`);
      } else {
        await updateCategory(editingId, category);
        setSuccessMessage(`Category "${category.name}" updated.`);
      }
      setEditingId(null);
      setForm(emptyForm);
      loadCategories();
    } catch (err) {
      const errors = getFieldErrors(err);
      setFieldErrors(errors);
      if (Object.keys(errors).length === 0) {
        setError(getErrorMessage(err));
      }
    }
  }

  async function handleDelete(category) {
    if (!window.confirm(`Delete the category "${category.name}"?`)) return;
    clearMessages();

    try {
      await deleteCategory(category.id);
      setSuccessMessage(`Category "${category.name}" deleted. Its weights now belong to the category below it.`);
      if (editingId === category.id) handleCancel();
      loadCategories();
    } catch (err) {
      setError(getErrorMessage(err));
    }
  }

  return (
    <div>
      <h1>Vehicle Categories</h1>

      {successMessage && <p className="alert alert-success">{successMessage}</p>}
      {error && <p className="alert alert-error">{error}</p>}

      <div className="table-wrapper">
        <table>
          <thead>
            <tr>
              <th>Category</th>
              <th>From (kg)</th>
              <th>Up to (kg)</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            {categories.map((category) => (
              <tr key={category.id} className={category.id === editingId ? "editing-row" : ""}>
                <td>
                  <span className="category-name">
                    <CategoryIcon iconKey={category.iconKey} />
                    {category.name}
                  </span>
                </td>
                <td>{category.minWeightKg.toLocaleString()}</td>
                <td>{category.maxWeightKg === null ? "and above" : category.maxWeightKg.toLocaleString()}</td>
                <td className="row-buttons">
                  <button type="button" className="button" onClick={() => handleEdit(category)}>
                    Edit
                  </button>
                  <button type="button" className="button button-danger" onClick={() => handleDelete(category)}>
                    Delete
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      {/* The same form is used to add a new category and to edit an existing one. */}
      <form onSubmit={handleSubmit} className="category-form">
        <h2>{editingId === null ? "Add a category" : "Edit category"}</h2>

        <label>
          Name
          <input name="name" value={form.name} onChange={handleChange} required maxLength={50} />
          <FieldError messages={fieldErrors.name} />
        </label>

        <label>
          Icon
          <span className="icon-picker">
            <select name="iconKey" value={form.iconKey} onChange={handleChange} required>
              <option value="">Select an icon</option>
              {icons.map((icon) => (
                <option key={icon} value={icon}>
                  {icon}
                </option>
              ))}
            </select>
            {form.iconKey && <CategoryIcon iconKey={form.iconKey} />}
          </span>
          <FieldError messages={fieldErrors.iconKey} />
        </label>

        <label>
          From (kg)
          <input
            name="minWeightKg"
            type="number"
            value={form.minWeightKg}
            onChange={handleChange}
            required
            min={0}
            step={0.01}
          />
          <FieldError messages={fieldErrors.minWeightKg} />
        </label>

        <div className="form-buttons">
          <button type="submit" className="button button-primary">
            {editingId === null ? "Add Category" : "Save Changes"}
          </button>
          {editingId !== null && (
            <button type="button" className="button" onClick={handleCancel}>
              Cancel
            </button>
          )}
        </div>
      </form>
    </div>
  );
}

export default CategoryManagement;
