import React, { useState, useEffect } from 'react';
import './Posts.css';

function Post({ data, index, removePost, editPost, likes }) {
    const user = localStorage.getItem('user');
    const [isAuthor, setIsAuthor] = useState(false);

    const handleRemove = (i) => {
        removePost(i);
    }

    const handleEdit = (i) => {
        editPost(i);
    }

    useEffect(() => {
        if (data.user === user) {
            setIsAuthor(true);
        }
    }, [isAuthor]);


    return (
        <li key={data.id}>
            <div className='post'>
                <div className='pub'>
                    <p className="user">{data.user}</p>
                    <p className="date">{data.published}</p>
                    {isAuthor &&
                        <>
                            <button className='edit-btn'
                                onClick={() => handleEdit(index)}>✍️</button>
                            <button className="remove-btn" name={data.id}
                                onClick={() => handleRemove(index)}>❌</button>
                        </>
                    }
                </div>
                <textarea id="textContent" className='con' readOnly={true} value={data.content}>
                </textarea>
            </div>
        </li>
    )
}

export default Post;