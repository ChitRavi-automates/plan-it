import "./Card.css";



function showUser(teamMember) {
  const name = new String(teamMember);
  return name.charAt(0).toUpperCase() + name.slice(1);
}


function formatDate(dateString) {
  const date = new Date(dateString);
  return date.toLocaleDateString("en-US", {
    month: "short",
    day: "numeric",
  });
}


export default function Card({ task }) {

  const getStatusColor = (status) => {
    switch (status) {
      case "To Do":
        return "lightcoral"; 
      case "Doing":
        return "khaki"; 
      case "Done":
        return "lightgreen"; 
      default:
        return "white";
    }
  };

  return (
    <div className="card-style" style={{
      backgroundColor: "white",
      border: "1px solid #ccc",
      borderLeftColor: getStatusColor(task.status), 
      borderLeftWidth: "15px",
      padding: "40px",
      marginBottom: "10px",
    }}>


      <header className="card-header">      
      <h4>{task.title}</h4>
      <button className="edit-btn"><img src="./src/assets/edit.svg" alt="edit-icon"></img> </button>
      </header>

      <main className="card-main">
      <small className="task-deadline">{formatDate(task.deadline)}</small>
      <small>  •  </small>
      <small> <img src="./src/assets/user.png" className="userProfilePicture"/> {showUser(task.teamMember)}</small><br />
      </main>


      <footer className="card-footer">
        <small>Project: {task.project}</small>
        <button className="add-label"> + Add Label </button>

      </footer>
    </div>
  );
}
