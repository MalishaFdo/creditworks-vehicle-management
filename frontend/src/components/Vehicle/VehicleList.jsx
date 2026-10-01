import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { getVehicles } from "../../api/vehicleApi";
import { getErrorMessage } from "../../api/apiErrors";
import CategoryIcon from "../Category/CategoryIcon";
import "../../styles/vehicle.css";

//Columns available for sorting.
const sortableColumns = [
  { field: "ownerName", label: "Owner" },
  { field: "manufacturer", label: "Manufacturer" },
  { field: "yearOfManufacture", label: "Year" },
  { field: "weight", label: "Weight (kg)" },
];

function VehicleList() {
  const [vehicles, setVehicles] = useState([]);
  const [sortBy, setSortBy] = useState("ownerName");
  const [sortDirection, setSortDirection] = useState("asc");
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState("");

  //Load vehicles when the page opens or sorting changes.
  useEffect(() => {
    setIsLoading(true);
    getVehicles(sortBy, sortDirection)
      .then((response) => {
        setVehicles(response.data);
        setError("");
      })
      .catch((err) => setError(getErrorMessage(err)))
      .finally(() => setIsLoading(false));
  }, [sortBy, sortDirection]);

  //Change the sort field or direction
  function handleSort(field) {
    if (field === sortBy) {
      setSortDirection(sortDirection === "asc" ? "desc" : "asc");
    } else {
      setSortBy(field);
      setSortDirection("asc");
    }
  }

// Show the correct arrow for the current sort.
  function sortArrow(field) {
    if (field !== sortBy) return "↕";
    return sortDirection === "asc" ? "▲" : "▼";
  }

  return (
    <div>
      <div className="page-header">
        <h1>Vehicles</h1>
        <Link to="/add-vehicle" className="button button-primary">
          Add Vehicle
        </Link>
      </div>

      {error && <p className="alert alert-error">{error}</p>}

      <div className="table-wrapper">
        <table>
          <thead>
            <tr>
              {sortableColumns.map((column) => (
                <th key={column.field}>
                  <button
                    type="button"
                    className={column.field === sortBy ? "sort-button active" : "sort-button"}
                    onClick={() => handleSort(column.field)}
                  >
                    {column.label} <span className="sort-arrow">{sortArrow(column.field)}</span>
                  </button>
                </th>
              ))}
              <th>Category</th>
            </tr>
          </thead>

          <tbody>
            {vehicles.map((vehicle) => (
              <tr key={vehicle.id}>
                <td>{vehicle.ownerName}</td>
                <td>{vehicle.manufacturerName}</td>
                <td>{vehicle.yearOfManufacture}</td>
                <td>{vehicle.weightKg.toLocaleString()}</td>
                <td>
                  {vehicle.category ? (
                    <span className="category-cell">
                      <CategoryIcon iconKey={vehicle.category.iconKey} />
                      {vehicle.category.name}
                    </span>
                  ) : (
                    "No category"
                  )}
                </td>
              </tr>
            ))}
          </tbody>
        </table>

        {isLoading && <p className="empty-message">Loading...</p>}
        {!isLoading && !error && vehicles.length === 0 && (
          <p className="empty-message">No vehicles yet. Click "Add Vehicle" to add one.</p>
        )}
      </div>
    </div>
  );
}

export default VehicleList;
