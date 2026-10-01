import { NavLink } from "react-router-dom";
import "../../styles/navbar.css";

function NavBar() {
  return (
    <header className="navbar">
      <span className="navbar-title">Vehicle Manager</span>

      <nav>
        <NavLink to="/" end>
          Vehicles
        </NavLink>
        <NavLink to="/categories">Categories</NavLink>
      </nav>
    </header>
  );
}

export default NavBar;
