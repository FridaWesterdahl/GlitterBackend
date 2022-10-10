import React, { useState, useRef, useEffect } from 'react';
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Posts from '../Components/Posts';
import './Home.css';
import img from '../star.png';
import nopic from '../nopic.png';
import axios from 'axios';
import { api } from '../Api.js';

function Home() {
    const [email, setEmail] = useState("");
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [confPassword, setConfPassword] = useState("");
    const [success, setSuccess] = useState(false);
    const [currentUser, setCurrentUser] = useState([]);

    const handleRegister = () => {
        var modal = document.getElementById("myModal");
        var span = document.getElementsByClassName("close")[0];
        modal.style.display = "block";

        span.onclick = function () {
            modal.style.display = "none";
        }

        window.onclick = function (event) {
            if (event.target === modal) {
                modal.style.display = "none";
            }
        }
    }

    const [registerError, setRegisterError] = useState(null);
    const handleSubmit = (event) => {
        event.preventDefault();

        const errormessage = document.querySelector("#passw-error");
        if (password !== confPassword) {
            errormessage.classList.remove("hidden");
        }
        if (password === confPassword) {
            errormessage.classList.add("hidden");
        }

        axios.post(api.API_URL + 'User/createUser', {
            'username': username,
            'email': email,
            'password': password
        }, {
            "headers": {
                'Content-Type': 'application/json'
            }
        })
            .then((response) => {
                console.log("response:", response.data);
                setRegisterError(null);
                const text = document.querySelector("#reg-success");
                text.classList.remove("hidden");
                const button = document.querySelector("#register-sub");
                button.classList.add("hidden");
                const login = document.querySelector("#register-sub2");
                login.classList.remove("hidden");
            })
            .catch((err) => {
                setRegisterError(err.message);
                alert("Username is already taken.");
            })




    }

    const registerUser = () => {
        axios.post(api.API_URL + 'Auth', {
            'username': username,
            'password': password
        }, {
            "headers": {
                'Content-Type': 'application/json',
            }
        })
            .then((response) => {
                console.log("response:", response.data);
                localStorage.setItem("authToken", response.data);
                setSuccess(true);
                setLoginError(null);
            })
            .catch((err) => {
                setLoginError(err.message);
                console.log("error:", err.message);
            })

        axios.get(api.API_URL + `User/getCurrentUser?username=${username}`, {
            'username': username
        })
            .then((fetchedData) => {
                setCurrentUser(fetchedData.data);
                localStorage.setItem("user", fetchedData.data[0].username);
                localStorage.setItem("userId", fetchedData.data[0].id);
                console.log(fetchedData.data);
            })
            .catch((err) => {
                console.log("error:", err.message);
            })


        setUsername("");
        setEmail("");
        setPassword("");
        setConfPassword("");

        var modal = document.getElementById("myModal");
        modal.style.display = "none";
    }

    const loginUsername = useRef();
    const loginPassword = useRef();
    const [loginError, setLoginError] = useState(null);
    const handleLogin = () => {
        axios.post(api.API_URL + 'Auth', {
            'username': loginUsername.current.value,
            'password': loginPassword.current.value
        }, {
            "headers": {
                'Content-Type': 'application/json'
            }
        })
            .then((response) => {
                console.log("response:", response.data);
                localStorage.setItem("authToken", response.data);
                setSuccess(true);
                setLoginError(null);
            })
            .catch((err) => {
                setLoginError(err.message);
                alert("User not found with that username and password.");
                console.log("error:", err.message);
            })

        getCurrentUser();
    }

    const getCurrentUser = () => {
        axios.get(api.API_URL + `User/getCurrentUser?username=${loginUsername.current.value}`, {
            'username': loginUsername.current.value
        })
            .then((fetchedData) => {
                setCurrentUser(fetchedData.data);
                localStorage.setItem("user", fetchedData.data[0].username);
                localStorage.setItem("userId", fetchedData.data[0].id);
                console.log(fetchedData.data);
            })
            .catch((err) => {
                console.log("error:", err.message);
            })
    }

    return (
        <>
            {success ? (
                <>
                    <Routes>
                        <Route path="" element={<Posts />} />
                    </Routes>
                </>
            ) : (
                <div className='home'>
                    <h1>Welcome to Glitter, it's like Twitter but worse.</h1>
                    <div className='login-container'>
                        <img src={img} alt="logo"></img>
                        <h2>Already have an account?</h2>
                        <div className='user-login'>
                            <p>Username:</p>
                            <input ref={loginUsername} type="text"></input>
                            <p>Password:</p>
                            <input ref={loginPassword} type="password"></input>
                        </div>
                        <button onClick={handleLogin}>Login</button>
                        <div className='user-reg'>
                            <p>Don't have an account?</p>
                            <button onClick={handleRegister}>Register</button>

                            <div id="myModal" class="modal">
                                <div class="modal-content">
                                    <span class="close">&times;</span>
                                    <div className='form'>
                                        <h3>Register new user</h3>

                                        <form onSubmit={handleSubmit}>
                                            <p>Email:</p>
                                            <input type="email"
                                                placeholder='example@home.com'
                                                onChange={(e) => setEmail(e.target.value)} required></input>
                                            <p>Username:</p>
                                            <input type="text"
                                                placeholder='3-20 letters'
                                                onChange={(e) => setUsername(e.target.value)} required minLength="3" maxLength="20"></input>
                                            <p>Password:</p>
                                            <input id="reg-passw" type="password"
                                                onChange={(e) => setPassword(e.target.value)} required minLength={1}></input>
                                            <p>Confirm password:</p>
                                            <input id="conf-reg-passw" type="password"
                                                required minLength={1} onChange={(e) => setConfPassword(e.target.value)}></input>
                                            <p id="passw-error" className='hidden'>Passwords does not match</p>

                                            <button id="register-sub" className='' onClick={handleRegister}>Register</button>
                                        </form>
                                        <p id="reg-success" className='hidden'>You are now registred. Click on Login!</p>
                                        <button id="register-sub2" className='hidden' onClick={registerUser}>Login</button>
                                    </div>

                                    <div className='def-preview-user'>
                                        <h4>Your profile:</h4>
                                        <img src={nopic} alt="profile pic"></img>
                                        <p>{username}</p>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            )}
        </>
    )
}

export default Home;