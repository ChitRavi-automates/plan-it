import { Link } from "react-router-dom"
import "./Navbar.css"


export default function Navbar(){
  return(
   <nav className="navbar">
      <div className="site-title">
        <h1><Link to="/">BuildBoard</Link></h1>
      </div>
      <ul className="nav-links">
        <li><Link to="/dashboard">Dashboard</Link></li>
        <li><Link to="/to-do">To Do</Link></li>
        <li><Link to="/team">Team</Link></li>
        <li><Link to="/calendar">Calendar</Link></li>
        <li><Link to="/settings">Settings</Link></li>
        <li><Link to="/log-out">Log Out</Link></li>
      </ul>
   </nav>
  )
}