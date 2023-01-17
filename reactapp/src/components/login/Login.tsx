import Box from '@mui/material/Box';
import TextField from '@mui/material/TextField';
import { Button } from '@mui/material';
import React, { useState, useEffect } from 'react';
import { UserAuth } from '../../Context/AuthContext';
import { Message } from '../message/Message';

interface User {
    username: string;
    password: string;
}
export const Login = () => {
    const { user, updateUser } = UserAuth();
    const [formData, setFormData] = useState<User>({ username: '', password: '' });
    const handleLogin = async (e: React.FormEvent<HTMLInputElement>) => {
        console.log('Entrando por aqui')
        e.preventDefault();
        await fetch("https://localhost:7251/api/authentication/login", {
            method: "POST",
            mode: "cors",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(formData),
        }).then(res => res.json()).then(res => {
            updateUser(res.username, res.jwt);
        }).then(
            () => {
                setFormData({ username: '', password: '' });
            }
        )
    }
    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        console.log("cambiando");
        setFormData((prevformData) => ({ ...prevformData, [e.target.name]: e.target.value }));
    }
    useEffect(() => {
        const localAuthToken = localStorage.getItem('authToken');
        if (
            localAuthToken !== undefined &&
            localAuthToken !== null &&
            localAuthToken !== 'undefined'
        ) {
            window.location.href = "http://localhost:3000/";
        }
    }, [user])
    return (
        <div>
            <div>
                <Box component="form"
                    sx={{
                        '& .MuiTextField-root': { m: 1, width: '70%' },
                    }} noValidate autoComplete="off" onSubmit={handleLogin}>
                    <div>
                        <TextField
                            id="username"
                            label="Username"
                            variant="outlined"
                            name="username"
                            onChange={handleChange} />
                    </div>
                    <div>
                        <TextField
                            id="password"
                            label="Password"
                            type="password"
                            variant="outlined"
                            name="password"
                            onChange={handleChange}
                        />
                    </div>
                    <div>
                        <Button variant="contained" size="large" type="submit">
                            Acceder
                        </Button>
                    </div>
                </Box>
                <br />
                <br />
                <Message />
            </div>
        </div>
    )
}