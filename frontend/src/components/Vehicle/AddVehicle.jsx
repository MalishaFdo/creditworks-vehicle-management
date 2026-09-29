import { useEffect, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { addVehicle } from "../../api/vehicleApi";
import { getManufacturers } from "../../api/manufacturerApi";
import { getErrorMessage, getFieldErrors } from "../../api/apiErrors";
import FieldError from "../Common/FieldError";
import "../../styles/vehicle.css";

const emptyVehicle = {
  ownerName: "",
  manufacturerId: "",
  yearOfManufacture: "",
  weightKg: "",
};

// Inputs give us text. The API wants numbers, or null when the box is empty.
function toNumber(value) {
  return value === "" ? null : Number(value);
}

function AddVehicle() {
  const navigate = useNavigate();
  const nextYear = new Date().getFullYear() + 1;

  const [vehicle, setVehicle] = useState(emptyVehicle);
  const [manufacturers, setManufacturers] = useState([]);
  const [fieldErrors, setFieldErrors] = useState({}); // errors from the server, per field
  const [error, setError] = useState(""); // any other error
  const [isSaving, setIsSaving] = useState(false);

  // Load the manufacturer list for the drop-down.
  useEffect(() => {
    getManufacturers()
      .then((response) => setManufacturers(response.data))
      .catch((err) => setError(getErrorMessage(err)));
  }, []);

  // One change handler for every input: the input's "name" says which field to update.
  function handleChange(event) {
    setVehicle({ ...vehicle, [event.target.name]: event.target.value });
  }

  async function handleSubmit(event) {
    event.preventDefault(); // stop the browser reloading the page
    setIsSaving(true);
    setError("");
    setFieldErrors({});

    try {
      await addVehicle({
        ownerName: vehicle.ownerName,
        manufacturerId: toNumber(vehicle.manufacturerId),
        yearOfManufacture: toNumber(vehicle.yearOfManufacture),
        weightKg: toNumber(vehicle.weightKg),
      });
      navigate("/"); // back to the vehicle list
    } catch (err) {
      const errors = getFieldErrors(err);
      setFieldErrors(errors);
      if (Object.keys(errors).length === 0) {
        setError(getErrorMessage(err));
      }
    } finally {
      setIsSaving(false);
    }
  }

  return (
    <div className="form-page">
      <h1>Add Vehicle</h1>

      {error && <p className="alert alert-error">{error}</p>}

      {/* The attributes on each input (required, min, max, step) let the browser
          check the values first. The server checks everything again. */}
      <form onSubmit={handleSubmit} className="form">
        <label>
          Owner's name
          <input name="ownerName" value={vehicle.ownerName} onChange={handleChange} required maxLength={100} />
          <FieldError messages={fieldErrors.ownerName} />
        </label>

        <label>
          Manufacturer
          <select name="manufacturerId" value={vehicle.manufacturerId} onChange={handleChange} required>
            <option value="">Select a manufacturer</option>
            {manufacturers.map((manufacturer) => (
              <option key={manufacturer.id} value={manufacturer.id}>
                {manufacturer.name}
              </option>
            ))}
          </select>
          <FieldError messages={fieldErrors.manufacturerId} />
        </label>

        <label>
          Year of manufacture
          <input
            name="yearOfManufacture"
            type="number"
            value={vehicle.yearOfManufacture}
            onChange={handleChange}
            required
            min={1886}
            max={nextYear}
          />
          <FieldError messages={fieldErrors.yearOfManufacture} />
        </label>

        <label>
          Weight (kg)
          <input
            name="weightKg"
            type="number"
            value={vehicle.weightKg}
            onChange={handleChange}
            required
            min={0.01}
            step={0.01}
            placeholder="e.g. 1850.75"
          />
          <FieldError messages={fieldErrors.weightKg} />
        </label>

        <div className="form-buttons">
          <button type="submit" className="button button-primary" disabled={isSaving}>
            {isSaving ? "Saving..." : "Save Vehicle"}
          </button>
          <Link to="/" className="button">
            Cancel
          </Link>
        </div>
      </form>
    </div>
  );
}

export default AddVehicle;
