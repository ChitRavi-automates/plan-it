import React, { useState } from 'react';
import './PulseDashboard.css';

const PulseDashboard = () => {
  const [mood, setMood] = useState(null);
  const [note, setNote] = useState("");

  const today = new Date().toLocaleDateString('sv-SE', {
    weekday: 'long',
    year: 'numeric',
    month: 'long',
    day: 'numeric'
  });

  const stats = {
    happy: 60,
    neutral: 30,
    sad: 10
  };

  return (
    <div className="dashboard">
      <div className="dashboard-header">
        <div>
          <h1 className="dashboard-title">Dashboard</h1>
          <p className="dashboard-date">{today}</p>
        </div>
      </div>

      <div className="dashboard-grid">
        {/* Daily Check-in */}
        <div className="card feeling-card">
          <h2>Dagens känsla</h2>
          <p className="dashboard-description">Hur känner du dig idag?</p>
          
          {mood === null ? (
            <div className="mood-buttons">
              <button onClick={() => setMood("happy")} className="mood-button">
                <span role="img" aria-label="happy">😊</span>
              </button>
              <button onClick={() => setMood("neutral")} className="mood-button">
                <span role="img" aria-label="neutral">😐</span>
              </button>
              <button onClick={() => setMood("sad")} className="mood-button">
                <span role="img" aria-label="sad">☹️</span>
              </button>
              <div className="anonymous-text">
                Ditt svar är anonymt
              </div>
            </div>
          ) : (
            <div className="mood-selected">
              <div className="selected-icon">
                {mood === "happy" && <span role="img" aria-label="happy">😊</span>}
                {mood === "neutral" && <span role="img" aria-label="neutral">😐</span>}
                {mood === "sad" && <span role="img" aria-label="sad">☹️</span>}
              </div>
              <textarea
                placeholder="Vill du dela mer om din dag? (valfritt)"
                value={note}
                onChange={(e) => setNote(e.target.value)}
                className="mood-textarea"
              />
              <div className="mood-actions">
                <button onClick={() => setMood(null)}>Ändra</button>
                <button>Skicka</button>
              </div>
            </div>
          )}
        </div>

        {/* Team Pulse Stats */}
        <div className="card pulse-card">
          <h2>Team Pulse</h2>
          <p className="dashboard-description">Hur mår teamet idag?</p>
          <div className="pulse-wheels">
            <div className="wheel happy">{stats.happy}%</div>
            <div className="wheel neutral">{stats.neutral}%</div>
            <div className="wheel sad">{stats.sad}%</div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default PulseDashboard;