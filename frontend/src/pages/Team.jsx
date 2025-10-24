
import "./Team.css";
import TeamDirectory from "../components/teamPage/TeamDirectory";
import MessageBoard from "../components/teamPage/MessageBoard";
import { Link } from "react-router-dom";


export default function Team(){


  return(
    <>
   <div className= "page-container">
    <header className="header-content-top">
    <h1 className="header-title">Team</h1>
    <h3 className="header-subtitle"> <Link to ="/dashboard">PlanIT</Link> / <Link to ="/team">Team</Link></h3>
    </header>


    <TeamDirectory/>

    <MessageBoard/>



    </div>
    </>
  )

}