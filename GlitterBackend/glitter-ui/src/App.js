import './App.css';
import Home from './Components/Home';
import Posts from './Components/Posts';
import User from './Components/User';
import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";

function App() {
  return (
    <div className="App">
      <Router>
        <Routes>
          <Route exact path="/*" element={<Home />} />
          <Route path="Posts" element={<Posts />} />
          <Route path="Profile" element={<User />} />
        </Routes>
      </Router>
    </div>
  );
}

export default App;
