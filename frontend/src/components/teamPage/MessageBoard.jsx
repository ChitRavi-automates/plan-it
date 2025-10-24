import { useState } from "react";
import "./MessageBoard.css";


  
export default function MessageBoard({ messages = [], addMessage }) {
  const [filters, setFilters] = useState({
    user: "",
    team: "",
    project: "",
    task: "",
  });

  const [newMessage, setNewMessage] = useState({
    user: "",
    team: "",
    project: "",
    task: "",
    content: "",
  });

  const users = [...new Set(messages.map((m) => m.user))];
  const teams = [...new Set(messages.map((m) => m.team))];
  const projects = [...new Set(messages.map((m) => m.project))];
  const tasks = [...new Set(messages.map((m) => m.task))];

  const filteredMessages = messages.filter((msg) => {
    return (
      (!filters.user || msg.user === filters.user) &&
      (!filters.team || msg.team === filters.team) &&
      (!filters.project || msg.project === filters.project) &&
      (!filters.task || msg.task === filters.task)
    );
  });

  const handleNewMessageChange = (key) => (e) => {
    setNewMessage({ ...newMessage, [key]: e.target.value });
  };

  const submitMessage = () => {
    if (newMessage.user && newMessage.content) {
      addMessage({ ...newMessage, id: Date.now() });
      setNewMessage({
        user: "",
        team: "",
        project: "",
        task: "",
        content: "",
      });
    }
  };

  return (
    <div className="message-board">
      <h2>Project Message Board</h2>

      <div className="filters">
        <select onChange={(e) => setFilters({ ...filters, team: e.target.value })} value={filters.team}>
          <option value="">All Teams</option>
          {teams.map((t) => (
            <option key={t} value={t}>{t}</option>
          ))}
        </select>
        <select onChange={(e) => setFilters({ ...filters, project: e.target.value })} value={filters.project}>
          <option value="">All Projects</option>
          {projects.map((p) => (
            <option key={p} value={p}>{p}</option>
          ))}
        </select>
        <select onChange={(e) => setFilters({ ...filters, task: e.target.value })} value={filters.task}>
          <option value="">All Tasks</option>
          {tasks.map((t) => (
            <option key={t} value={t}>{t}</option>
          ))}
        </select>
        <select onChange={(e) => setFilters({ ...filters, user: e.target.value })} value={filters.user}>
          <option value="">All Users</option>
          {users.map((u) => (
            <option key={u} value={u}>{u}</option>
          ))}
        </select>
      </div>

      <div className="messages">
        {filteredMessages.map((msg) => (
          <div key={msg.id || msg.content} className="message">
            <p>
              <strong>{msg.user}</strong>{" "}
              [{msg.team} &gt; {msg.project} &gt; {msg.task}]
            </p>
            <p>{msg.content}</p>
          </div>
        ))}
      </div>

      <div className="new-message">
        <h3>Post New Message</h3>
        <input
          placeholder="User"
          value={newMessage.user}
          onChange={handleNewMessageChange("user")}
        />
        <input
          placeholder="Team"
          value={newMessage.team}
          onChange={handleNewMessageChange("team")}
        />
        <input
          placeholder="Project"
          value={newMessage.project}
          onChange={handleNewMessageChange("project")}
        />
        <input
          placeholder="Task"
          value={newMessage.task}
          onChange={handleNewMessageChange("task")}
        />
        <textarea
          placeholder="Message"
          value={newMessage.content}
          onChange={handleNewMessageChange("content")}
        />
        <button onClick={submitMessage}>Post</button>
      </div>
    </div>
  );
}