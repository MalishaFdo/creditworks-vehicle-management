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

//Convert input text to a number or null.
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

  //Load manufacturers when the page opens.
  useEffect(() => {
    getManufacturers()
      .then((response) => setManufacturers(response.data))
      .catch((err) => setError(getErrorMessage(err)));
  }, []);

  //Update the changed form field.
  function handleChange(event) {
    setVehicle({ ...vehicle, [event.target.name]: event.target.value });
  }

  async function handleSubmit(event) {
    // Stop the page from reloading.
    event.preventDefault(); 
    setIsSaving(true);
    setError("");
    setFieldErrors({});

    try {
      // Send the vehicle data to the API.
      await addVehicle({
        ownerName: vehicle.ownerName,
        manufacturerId: toNumber(vehicle.manufacturerId),
        yearOfManufacture: toNumber(vehicle.yearOfManufacture),
        weightKg: toNumber(vehicle.weightKg),
      });
      // Return to the vehicle list after saving.
      navigate("/"); 
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

      {/* Browser validation runs before the form is submitted. */}
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
