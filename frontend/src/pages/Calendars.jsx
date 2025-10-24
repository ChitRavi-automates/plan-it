import React, { useState } from "react";
import { Calendar, momentLocalizer } from "react-big-calendar";
import moment from "moment";
import "react-big-calendar/lib/css/react-big-calendar.css";
import { myEventsList } from "../components/calendarPage/EventsList";
import {Link} from "react-router-dom";

const localizer = momentLocalizer(moment);




export default function Calendars() {
  const [view, setView] = useState("month");
  const [date, setDate] = useState(new Date());

  const handleViewChange = (newView) => {
    setView(newView);
  };

  const handleNavigate = (newDate) => {
    setDate(newDate);
  };

  return (

    <>
    <header className="header-content-top">
      <h1 className="header-title">Calendar</h1>
      <h3 className="header-subtitle"><Link to ="/">PlanIT</Link> / <Link to ="/calendar">Calendar</Link></h3>
    </header>



    <main  style={{ height: 800, padding: "2rem" }}>
      <Calendar
        localizer={localizer}
        events={myEventsList}
        startAccessor="start"
        endAccessor="end"
        style={{ height: "100%" }}
        views={{ month: true, week: true, day: true, agenda: true }}
        view={view}
        onView={handleViewChange}
        date={date}
        onNavigate={handleNavigate}
        popup
        className="calendar"
      />
    </main>

    </>
  );
}