import React from 'react';
import { Link } from 'react-router-dom';
import './SignUp.css';

export default function SignUp() {
  return (
    <div className="form-box">
      <form className="form">
        <span className="title">Sign up</span>
        <span className="subtitle">Create a free account with your email.</span>
        <div className="form-container">
          <input type="text" className="input" placeholder="Full Name" />
          <input type="email" className="input" placeholder="Email" />
          <input type="password" className="input" placeholder="Password" />
        </div>
        <button type="submit">Sign up</button>
      </form>
      <div className="form-section">
        <p>Have an account? <Link to="/log-in">Log in</Link></p>
      </div>
    </div>
  );
}