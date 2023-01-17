import { Button } from '@mui/material';
import { Logout } from '../common/Logout/Logout';
import { useEffect } from 'react';
import { UserAuth } from '../../Context/AuthContext';
import { Message } from '../message/Message';
export const Home = () => {
    const { user } = UserAuth();
    useEffect(() => {
        if (!localStorage.length) {
            window.location.href = "http://localhost:3000/login";
        }
    }, [user])
    return (
        <div>
            <Button variant="contained" size="large" onClick={() => { Logout() }}>
                Salirse
            </Button>
            <br />
            <br />
            <Message />
        </div>


    )
}

export default Home;