import { useState } from "react";
import "./TeamDirectory.css";

const sampleData = [
  {
    id: 1,
    name: "Joshua Ng",
    role: "Developer",
    location: "Stockholm",
    description: "Frontend developer with React experience.",
    team: "Alpha",
  },
  {
    id: 2,
    name: "Sami Harun",
    role: "Designer",
    location: "Berlin",
    description: "UX/UI designer with a keen eye for detail.",
    team: "Project Falcon",
  },
  {
    id: 3,
    name: "Hanna ",
    role: "Project Manager",
    location: "Stockholm",
    description: "Drives teams forward with agile methodologies.",
    team: "Project Phoenix",
  },
  {
    id: 4,
    name: "Axel Kim",
    role: "Developer",
    location: "New York",
    description: "Full-stack developer with Node.js and React.",
    team: "Project Falcon",
  },
  {
    id: 5,
    name: "Abdifatah Gomez",
    role: "QA Engineer",
    location: "London",
    description: "Ensures product quality and writes test suites.",
    team: "Project Phoenix",
  },
  {
    id: 6,
    name: "Alfred Zhao",
    role: "Developer",
    location: "Berlin",
    description: "Backend engineer with a passion for databases.",
    team: "Project Falcon",
  },
  {
    id: 7,
    name: "Ida Mendez",
    role: "Developer",
    location: "Berlin",
    description: "Backend engineer with a passion for databases.",
    team: "Project Falcon",
  },
  {
    id: 8,
    name: "Chitra Marie",
    role: "Developer",
    location: "Berlin",
    description: "Backend engineer with a passion for databases.",
    team: "Project Falcon",
  },
  {
    id: 9,
    name: "Tareq Svensson",
    role: "Developer",
    location: "Berlin",
    description: "Backend engineer with a passion for databases.",
    team: "Project Falcon",
  },
  {
    id: 10,
    name: "Aldor Besher",
    role: "Developer",
    location: "Berlin",
    description: "Backend engineer with a passion for databases.",
    team: "Project Falcon",
  }



];
function ProfileCard({ member, onView }) {
  return (
    <div className="card">
      <img className="profile-picture" src= "./src/assets/user.png" alt={member.name} />
      <h2>{member.name}</h2>
      <p><strong>Role:</strong> {member.role}</p>
      <p><strong>Location:</strong> {member.location}</p>
      <p>{member.description}</p>
      <button onClick={() => onView(member)}>View</button>
    </div>
  );
}

function Filters({ filters, setFilters, allMembers }) {
  const roles = [...new Set(allMembers.map((m) => m.role))];
  const locations = [...new Set(allMembers.map((m) => m.location))];
  const teams = [...new Set(allMembers.map((m) => m.team))];

  const handleChange = (key) => (e) => {
    setFilters({ ...filters, [key]: e.target.value });
  };

  return (
    <div className="filters">
      <input
        type="text"
        placeholder="Search by name..."
        value={filters.search}
        onChange={(e) => setFilters({ ...filters, search: e.target.value })}
      />
      <select onChange={handleChange("team")} value={filters.team}>
        <option value="">All Teams</option>
        {teams.map((t) => (
          <option key={t} value={t}>{t}</option>
        ))}
      </select>
      <select onChange={handleChange("role")} value={filters.role}>
        <option value="">All Roles</option>
        {roles.map((r) => (
          <option key={r} value={r}>{r}</option>
        ))}
      </select>
      <select onChange={handleChange("location")} value={filters.location}>
        <option value="">All Locations</option>
        {locations.map((l) => (
          <option key={l} value={l}>{l}</option>
        ))}
      </select>
    </div>
  );
}

function paginate(data, page, perPage) {
  const start = (page - 1) * perPage;
  return data.slice(start, start + perPage);
}

export default function TeamDirectory() {
  const [filters, setFilters] = useState({ team: "", role: "", location: "", search: "" });
  const [page, setPage] = useState(1);
  const [perPage, setPerPage] = useState(5);
  const [sort, setSort] = useState("name");
  const [selectedMember, setSelectedMember] = useState(null);

  const filtered = sampleData.filter((m) => {
    return (
      (!filters.team || m.team === filters.team) &&
      (!filters.role || m.role === filters.role) &&
      (!filters.location || m.location === filters.location) &&
      (!filters.search || m.name.toLowerCase().includes(filters.search.toLowerCase()) ||
        m.description.toLowerCase().includes(filters.search.toLowerCase()))
    );
  });

  const sorted = [...filtered].sort((a, b) => {
    if (a[sort] < b[sort]) return -1;
    if (a[sort] > b[sort]) return 1;
    return 0;
  });

  const totalPages = Math.ceil(sorted.length / perPage);
  const paginated = paginate(sorted, page, perPage);

  return (
    <div className="team-directory">
      
      
      <header className="header-directory">
      <h2>Directory</h2>
      <Filters filters={filters} setFilters={setFilters} allMembers={sampleData} />

      <div className="sort-control">
        <label>Sort by: </label>
        <select value={sort} onChange={(e) => setSort(e.target.value)}>
          <option value="name">Name (A-Z)</option>
          <option value="role">Role</option>
          <option value="location">Location</option>
        </select>
      </div>
      </header>

      <div className="card-grid">
        {paginated.map((member) => (
          <ProfileCard key={member.id} member={member} onView={setSelectedMember} />
        ))}
      </div>

      <div className="pagination">
        <button onClick={() => setPage((p) => Math.max(1, p - 1))} disabled={page === 1}>Prev</button>
        <span>Page {page} of {totalPages}</span>
        <button onClick={() => setPage((p) => Math.min(totalPages, p + 1))} disabled={page === totalPages}>Next</button>
      </div>

      <div className="per-page">
        <label>Cards per page: </label>
        <select value={perPage} onChange={(e) => { setPerPage(Number(e.target.value)); setPage(1); }}>
          {[5, 10, 20].map((n) => (
            <option key={n} value={n}>{n}</option>
          ))}
        </select>
      </div>

      {selectedMember && (
        <div className="modal" onClick={() => setSelectedMember(null)}>
          <div className="modal-content" onClick={(e) => e.stopPropagation()}>

          
          <button className="close-btn" onClick={() => setSelectedMember(null)}>X</button>
          
            <img className="profile-picture" src= "./src/assets/user.png" alt={selectedMember.name} />
            <h2>{selectedMember.name}</h2>
            <p><strong>Role:</strong> {selectedMember.role}</p>
            <p><strong>Location:</strong> {selectedMember.location}</p>
            <p><strong>Team:</strong> {selectedMember.team}</p>
            <p>{selectedMember.description}</p>
            
          </div>
        </div>
      )}
    </div>
  );
}
