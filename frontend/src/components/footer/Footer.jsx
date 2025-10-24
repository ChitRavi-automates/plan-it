import { Link } from "react-router-dom"
import "./Footer.css"

export default function Footer() {
  return (
    <footer className = "footer-container">
      <div className="footer-content">
        <h3>BuildBoard AB</h3>
        <p>Lorem ipsum dolor sit amet, consectetur adipisicing elit. Quod, doloremque.</p>
        <p>&copy; 2025 BuildBoard. All rights reserved.</p>
      </div>

      <div> <ul className="footer-socials">
          <li><Link to="#">Facebook</Link></li>
          <li><Link to="#">LinkedIn</Link></li>
          <li><Link to="#">Instagram</Link></li>
        </ul>
      </div>
     
    </footer>
  )
}