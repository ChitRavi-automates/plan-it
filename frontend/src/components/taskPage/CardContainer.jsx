import Card from "./Card";
import "./CardContainer.css";
import Modal from "./Modal";


export default function CardContainer({ title, tasks, numberOfTasks }) {


  const handleNewTask = (newTask) => {
    setTasks([...tasks, newTask]);
  };
  

  return (


    <div style={{ flex: 1, margin: "0 10px" }}>

      <header className="card-container-header">
      <h3>{title}</h3>
      <h3 className="number-of-tasks">{numberOfTasks} TASKS</h3>
       <Modal onAddTask={handleNewTask} />
      </header>

      {tasks.map((task, index) => (
        <Card key={index} task={task}/>
      ))}
    </div>
  );
}