import React from 'react'
import { BrowserRouter, Routes, Route, Link } from 'react-router-dom'
import Footer from './components/footer/Footer'
import Navbar from './components/navbar/Navbar'
import Home from './pages/Home'
import Dashboard from './pages/Dashboard'
import Calendars from './pages/Calendars'
import Settings from './pages/Settings'
import Team from './pages/Team'
import LogOut from './pages/LogOut'
import ToDo from './pages/ToDo'
import LogIn from './pages/LogIn'    
import SignUp from './pages/SignUp'  


export default function  App() {
  return (

    <BrowserRouter>
      <Navbar/>
      <Routes>
        <Route path="/" element={<Home/>}>/</Route>
        <Route path="/dashboard" element={<Dashboard/>}>/Dashboard</Route>
        <Route path="/team" element={<Team/>}>/Team</Route>
        <Route path="/to-do" element={<ToDo/>}>/To Do</Route>
        <Route path="/calendar" element={<Calendars/>}>/Calendar</Route>
        <Route path="/settings" element={<Settings/>}>/Settings</Route>
        <Route path="/log-out" element={<LogOut/>}>/LogOut</Route>
        <Route path="/log-in" element={<LogIn/>}>/Log-In</Route>        
        <Route path="/sign-up" element={<SignUp/>}>/SignUp</Route>    
      </Routes>
      <Footer/>
    </BrowserRouter>
  )
}

