import { Button } from '@mui/material';
import { useState } from 'react';
import { UserAuth } from '../../Context/AuthContext';
export const Message = () => {
    const { user } = UserAuth();
    const [message, setMessage] = useState<string>("");
    const handleTestApp = async () => {
        await fetch("https://localhost:7178/api/TestApp", {
            method: "GET",
            mode: "cors",
            headers: {
                "Content-Type": "application/json",
                "Authorization": 'Bearer ' + user.accessToken
            }
        }).then(res => res.text()).then(data => setMessage(data))
    }
    return (
        <div>
            <Button variant="contained" size="large" onClick={handleTestApp}>
                Give Message
            </Button>
            <br />
            <br />
            <div className="show-message">
                {message}
            </div>
        </div>

    )
}