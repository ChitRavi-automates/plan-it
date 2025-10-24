import "./ToDo.css";
import { useState } from "react";
import CardContainer from "../components/taskPage/CardContainer";
import Modal from "../components/taskPage/Modal";
import { Link } from "react-router-dom";

export default function ToDo() {
  const [tasks, setTasks] = useState([
    // Boiler plate tasks
    {
      title: "Design UI",
      description: "Create mockups",
      deadline: "2025-05-15",
      teamMember: "Alice",
      project: "Alpha",
      status: "To Do",
    },
    {
      title: "Design UI",
      description: "Create mockups",
      deadline: "2025-05-15",
      teamMember: "Alice",
      project: "Alpha",
      status: "To Do",
    },
    {
      title: "Design UI",
      description: "Create mockups",
      deadline: "2025-05-15",
      teamMember: "Alice",
      project: "Alpha",
      status: "To Do",
    },

    {
      title: "API Integration",
      description: "Connect backend",
      deadline: "2025-05-14",
      teamMember: "Bob",
      project: "Beta",
      status: "Doing",
    },
    {
      title: "Build Frontend",
      description: "React Project",
      deadline: "2025-05-23",
      teamMember: "Bob",
      project: "Beta",
      status: "Doing",
    },
    {
      title: "API Integration",
      description: "Connect backend",
      deadline: "2025-05-14",
      teamMember: "Bob",
      project: "Beta",
      status: "Doing",
    },

    {
      title: "Write Tests",
      description: "Unit tests",
      deadline: "2025-05-10",
      teamMember: "Alice",
      project: "Alpha",
      status: "Done",
    },
  ]);

  const [filter, setFilter] = useState({
    member: "",
    project: "",
    deadline: "",
  });

  const filteredTasks = tasks.filter((task) => {
    return (
      (!filter.member || task.teamMember === filter.member) &&
      (!filter.project || task.project === filter.project) &&
      (!filter.deadline || task.deadline === filter.deadline)
    );
  });

  const handleNewTask = (newTask) => {
    setTasks([...tasks, newTask]);
  };

  const uniqueMembers = [...new Set(tasks.map((t) => t.teamMember))];
  const uniqueProjects = [...new Set(tasks.map((t) => t.project))];

  const uniqueDeadlines = [...new Set(tasks.map((t) => t.deadline))];

  return (
    <div>
      <header className="header-content-top">
        <h1 className="header-title">Task</h1>
        <h3 className="header-subtitle">
          <Link to="/dashboard">PlanIT</Link> / <Link to="/task">Task</Link>
        </h3>

      
      </header>


        {/* Filter UI */}

        <div className="filter-dropdown">
        <label htmlFor="filterType">Filter:</label>
        <select
          id="filterType"
          value={
            filter.member
              ? `member:${filter.member}`
              : filter.project
              ? `project:${filter.project}`
              : filter.deadline
              ? `deadline:${filter.deadline}`
              : ""
          }
          onChange={(e) => {
            const [type, value] = e.target.value.split(":");

            if (!type || !value) {
              // Reset all filters
              setFilter({ member: "", project: "", deadline: "" });
              return;
            }

            // Reset other filters when one is chosen
            setFilter({
              member: type === "member" ? value : "",
              project: type === "project" ? value : "",
              deadline: type === "deadline" ? value : "",
            });
          }}
        >
          <option value="">All Tasks</option>

          <optgroup label="By Member">
            {uniqueMembers.map((m) => (
              <option key={`member-${m}`} value={`member:${m}`}>
                {m}
              </option>
            ))}
          </optgroup>

          <optgroup label="By Project">
            {uniqueProjects.map((p) => (
              <option key={`project-${p}`} value={`project:${p}`}>
                {p}
              </option>
            ))}
          </optgroup>

          <optgroup label="By Deadline">
            {uniqueDeadlines.map((d) => (
              <option key={`deadline-${d}`} value={`deadline:${d}`}>
                {new Date(d).toLocaleDateString()}
              </option>
            ))}
          </optgroup>
        </select>

        {/* Clear Filters Button */}
        <button
          onClick={() => setFilter({ member: "", project: "", deadline: "" })}
          style={{ marginLeft: "1rem" }}
        >
          Clear Filters
        </button>
      </div>
      
      

      {/* Task Columns */}
      <main className="main-content">

        <h2 className="main-title">Task Board</h2>

        <div style={{ display: "flex", marginTop: "20px" }}>
          <CardContainer
            title="To Do"
            numberOfTasks={filteredTasks.filter((t) => t.status === "To Do").length}
            tasks={filteredTasks.filter((t) => t.status === "To Do")}
          />
          <CardContainer
            title="Doing"
            numberOfTasks={filteredTasks.filter((t) => t.status === "Doing").length}
            tasks={filteredTasks.filter((t) => t.status === "Doing")}
          />
          <CardContainer
            title="Done"
            numberOfTasks={filteredTasks.filter((t) => t.status === "Done").length}
            tasks={filteredTasks.filter((t) => t.status === "Done")}
          />
        </div>



      </main>
    </div>
  );
}
