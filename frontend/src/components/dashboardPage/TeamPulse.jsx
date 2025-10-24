import React from "react";
import { FaSmile, FaMeh, FaFrown } from "react-icons/fa";
import "./TeamPulse.css";

const TeamPulse = () => {
  const pulseData = [
    { mood: "happy", icon: <FaSmile />, percent: 60, color: "#4caf50", label: "Happy" },
    { mood: "neutral", icon: <FaMeh />, percent: 25, color: "#ff9800", label: "Neutral" },
    { mood: "sad", icon: <FaFrown />, percent: 15, color: "#f44336", label: "Sad" },
  ];

  return (
    <section className="team-pulse">
      <h2>Team Pulse</h2>
      <div className="pulse-grid">
        {pulseData.map((item) => (
          <div key={item.mood} className="pulse-card" style={{ borderColor: item.color }}>
            <div className="icon" style={{ color: item.color }}>{item.icon}</div>
            <div className="percent">{item.percent}%</div>
            <div className="label">{item.label}</div>
          </div>
        ))}
      </div>
    </section>
  );
};

export default TeamPulse;