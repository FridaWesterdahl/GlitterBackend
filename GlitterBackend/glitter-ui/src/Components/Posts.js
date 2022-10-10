import React, { useState, useEffect, useRef } from 'react';
import './Posts.css';
import axios from 'axios';
import { api } from '../Api.js';
import Post from './Post';

let editId;
let id;

export default function Posts() {
    const user = localStorage.getItem('user');
    const userId = localStorage.getItem('userId');

    const [click, setClick] = useState(false)
    const handleClick = () => setClick(!click);

    const [data, setData] = useState([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);

    useEffect(() => {
        axios.get(api.API_URL + 'Post/getPosts', {
            "headers": {
                'Content-Type': 'application/json',
                'Authorization': `bearer ${localStorage.getItem('authToken')}`
            }

        })
            .then((fetchedData) => {
                setLoading(true);
                setData(fetchedData.data);
                setError(null);
            })
            .catch((err) => {
                console.log("error:", error);
                setError(err.message);
            })
            .finally(() => {
                setLoading(false);
            })
    }, [data]);

    data.sort((a, b) => {
        return new Date(b.published) - new Date(a.published);
    })

    const [errorPost, setErrorPost] = useState(null);
    const postContent = useRef(null);

    const handleNewPost = () => {
        const date = new Date();
        axios.post(api.API_URL + 'Post/createPost', {
            'content': postContent.current.value,
            'published': date,
            'userId': userId
        }, {
            "headers": {
                'Content-Type': 'application/json',
                'Authorization': `bearer ${localStorage.getItem('authToken')}`
            }
        })
            .then((response) => {
                console.log("response:", response.data);
                setErrorPost(null);
            })
            .catch((err) => {
                setErrorPost(err.message);
                alert(err.message)
                console.log("error:", errorPost);
            })

        postContent.current.value = null;
    }

    const [errorDelete, setErrorDelete] = useState(null);

    const removePost = (i) => {
        let removeId = data.filter((value, index) => index === i);
        console.log("removeId:", removeId)
        let id = removeId[0].postId;

        axios.delete(api.API_URL + `Post/delete/${id}`, {
            "headers": {
                'Content-Type': 'application/json',
                'Authorization': `bearer ${localStorage.getItem('authToken')}`
            }
        })
            .then((response) => {
                console.log("response:", response.data);
                setErrorDelete(null);
            })
            .catch((err) => {
                setErrorDelete(err.message);
                console.log("error:", errorDelete);
            })
    }

    const editText = useRef(null);
    const editPost = (i) => {
        editId = data.filter((value, index) => index === i);
        console.log("editId:", editId)
        id = editId[0].postId;
        console.log("Id:", id)

        var modal = document.getElementById("edit-window");
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

    const putRequest = () => {
        axios.put(api.API_URL + `Post/editPost/${id}`, {
            'content': editText.current.value
        }, {
            "headers": {
                'Content-Type': 'application/json',
                'Authorization': `bearer ${localStorage.getItem('authToken')}`
            }
        })
            .then((response) => {
                console.log("response:", response.data);
                editText.current.value = null;
            })
            .catch((err) => {
                console.log(err.message);
            })

        var modal = document.getElementById("edit-window");
        modal.style.display = "none";
    }

    return (
        <div className='posts'>
            <h1>Welcome {user}!</h1>
            <div className='main'>
                <div className='navbar'>
                    <h2>Menu</h2>
                    <ul>
                        <li id="posts-link"><a href="Posts">Posts</a></li>
                        <li id="user-link"><a href="Profile">Profile</a></li>

                    </ul>
                </div>

                <div className='new-post'>
                    <button id="new-post-btn" onClick={handleClick}>New post...</button>
                    <form className={click ? 'show' : 'hidden'}>
                        <textarea type="text" rows="10" cols="250"
                            placeholder='What are you thinking about?'
                            ref={postContent}></textarea>

                    </form>
                    <button id="post-btn" className={click ? 'show' : 'hidden'}
                        type="submit"
                        onClick={() => handleNewPost()}>publish</button>
                </div>

                <div id="edit-window" class="modal">
                    <div class="modal-content">
                        <span class="close">&times;</span>
                        <div className='form'>
                            <h3>Edit post</h3>
                            <textarea ref={editText}></textarea>
                            <button type="submit" onClick={(e) => putRequest(e.preventDefault())}>Edit</button>
                        </div>
                    </div>
                </div>

                <div className='posts-container'>
                    <h3 id="latest-posts-title">Latest posts</h3>
                    <ul>
                        {
                            data.map((value, index) => {
                                return <Post
                                    index={index}
                                    key={index}
                                    data={value}
                                    removePost={removePost}
                                    editPost={editPost}
                                />
                            })}
                    </ul>
                </div>

            </div >
        </div>
    )
}
