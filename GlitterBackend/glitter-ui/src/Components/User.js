import React, { useState, useEffect, useRef } from 'react';
import './User.css';
import axios from 'axios';
import { api } from '../Api.js';
import nopic from '../nopic.png';

function User() {
    const userId = localStorage.getItem('userId');

    const [email, setEmail] = useState("");
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [confPassword, setConfPassword] = useState("");
    const [user, setUser] = useState([]);
    const [error, setError] = useState(null);
    const [editError, setEditError] = useState(null);

    useEffect(() => {
        axios.get(api.API_URL + `User/getUserById/${userId}`, {
            "headers": {
                'Content-Type': 'application/json',
                'Authorization': `bearer ${localStorage.getItem('authToken')}`
            }
        })
            .then((fetchedData) => {
                setUser(fetchedData.data);
                setError(null);
            })
            .catch((err) => {
                console.log("error:", err.message);
                setError(err.message);
            })
    }, [user]);

    const [editEmailClick, setEmailClick] = useState(false)
    const handleEmailClick = () => setEmailClick(!editEmailClick);
    const emailInput = useRef(null);

    const editEmail = () => {
        console.log("editEmailSubmit")
        axios.put(api.API_URL + `User/editUser/${userId}`, {
            'id': user.id,
            'username': user.username,
            'email': email,
            'password': user.password
        }, {
            "headers": {
                'Content-Type': 'application/json',
                'Authorization': `bearer ${localStorage.getItem('authToken')}`
            }
        })
            .then((response) => {
                console.log("response:", response.data);
                setEditError(null);
            })
            .catch((err) => {
                setEditError(err.message);
                console.log("error:", err.message);
            })

        emailInput.current.value = null;
    }

    const [editUsernameClick, setUsernameClick] = useState(false)
    const handleUsernameClick = () => setUsernameClick(!editUsernameClick);
    const usernameInput = useRef(null);

    const editUsername = () => {
        console.log("editUsernameSubmit")
        axios.put(api.API_URL + `User/editUsername/${userId}`, {
            'id': user.id,
            'username': username,
            'email': user.email,
            'password': user.password
        }, {
            "headers": {
                'Content-Type': 'application/json',
                'Authorization': `bearer ${localStorage.getItem('authToken')}`
            }
        })
            .then((response) => {
                console.log("response:", response.data);
                localStorage.setItem("user", username);
                setEditError(null);
            })
            .catch((err) => {
                setEditError(err.message);
                console.log("error:", err.message);
                alert("Username is already taken.")
            })

        usernameInput.current.value = null;
    }

    const [editPasswordClick, setPasswordClick] = useState(false)
    const handlePasswordClick = () => setPasswordClick(!editPasswordClick);
    const passwordInput = useRef(null);

    const editPassword = () => {
        console.log("editPasswordSubmit")
        axios.put(api.API_URL + `User/editUser/${userId}`, {
            'id': user.id,
            'username': user.username,
            'email': user.email,
            'password': password
        }, {
            "headers": {
                'Content-Type': 'application/json',
                'Authorization': `bearer ${localStorage.getItem('authToken')}`
            }
        })
            .then((response) => {
                console.log("response:", response.data);
                alert("Your password has been changed.")
                setEditError(null);
            })
            .catch((err) => {
                setEditError(err.message);
                console.log("error:", err.message);
            })

        passwordInput.current.value = null;
    }

    const handleLogOut = () => {
        localStorage.removeItem('authToken');
        localStorage.removeItem('user');
        localStorage.removeItem('userId');
    }

    const [deleteClick, setDeleteClick] = useState(false);

    const deleteAccount = () => {
        axios.delete(api.API_URL + `User/delete/${userId}`, {
            "headers": {
                'Content-Type': 'application/json',
                'Authorization': `bearer ${localStorage.getItem('authToken')}`
            }
        })
            .then((response) => {
                console.log("response:", response.data);
                localStorage.removeItem('authToken');
                localStorage.removeItem('user');
                localStorage.removeItem('userId');
                alert("Your account has been deleted.")
            })
            .catch((err) => {
                console.log("error:", err.message);
            })

        axios.delete(api.API_URL + 'User/deleteUsersPosts', {
            'userId': userId
        }, {
            "headers": {
                'Content-Type': 'application/json',
                'Authorization': `bearer ${localStorage.getItem('authToken')}`
            }
        })
            .then((response) => {
                console.log("response:", response.data);
                localStorage.removeItem('authToken');
                localStorage.removeItem('user');
                localStorage.removeItem('userId');
            })
            .catch((err) => {
                console.log("error:", err.message);
            })
    }

    return (
        <>
            <h1>Profile</h1>
            <div className='main-profile'>
                <div className='navbar'>
                    <h2>Menu</h2>
                    <ul>
                        <li id="posts-link"><a href="Posts">Posts</a></li>
                        <li id="user-link"><a href="Profile">Profile</a></li>

                    </ul>
                </div>

                <div className='profile'>
                    <div className='value'>
                        <p>Username:<br></br>{user.username}</p>
                        <p>Email:<br></br>{user.email}</p>
                        <p><br></br>Password:</p>
                    </div>

                    <div className='buttons'>
                        <div className='edit'>
                            <button className='edit-btn' onClick={handleUsernameClick}>Edit</button>
                            <input className={editUsernameClick ? "" : "hidden"} type="text" ref={usernameInput}
                                placeholder='3-20 letters'
                                onChange={(e) => setUsername(e.target.value)} minLength="3" maxLength="20" required></input>
                            <button className={editUsernameClick ? "submit" : "hidden"} onClick={editUsername}>Submit</button>
                        </div>

                        <div className='edit'>
                            <button className='edit-btn' onClick={handleEmailClick}>Edit</button>
                            <input className={editEmailClick ? "" : "hidden"} type="email" ref={emailInput}
                                placeholder='example@home.com'
                                onChange={(e) => setEmail(e.target.value)} required></input>
                            <button className={editEmailClick ? "submit" : "hidden"} onClick={editEmail}>Submit</button>
                        </div>

                        <div className='edit'>
                            <button className='edit-btn' onClick={handlePasswordClick}>Edit</button>
                            <input className={editPasswordClick ? "" : "hidden"} id="reg-passw" type="password" ref={passwordInput}
                                onChange={(e) => setPassword(e.target.value)} minLength={1} required></input>
                            <button className={editPasswordClick ? "submit" : "hidden"} onClick={editPassword}>Submit</button>
                        </div>
                    </div>

                    <a href="http://localhost:3000"><button onClick={handleLogOut}>Log out</button></a>
                    <button className='delete-btn' onClick={() => setDeleteClick(!deleteClick)}>Delete account</button>
                    <div className={deleteClick ? 'delete-account' : 'hidden'}>
                        <p>Are you sure you want do delete your account?</p>
                        <a href='http://localhost:3000'><button onClick={deleteAccount}>Yes</button></a>
                        <button onClick={() => setDeleteClick(!deleteClick)}>No</button>
                    </div>
                </div>

            </div>
        </>
    )
}

export default User;