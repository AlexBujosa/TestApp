import React, { useContext, createContext, useState, useEffect } from 'react'

interface User {
    username: string;
    accessToken: string;
}

interface AuthContextProps {
    user: User;
    updateUser: (username: string, accessToken: string) => void;
}
const AuthContext = createContext<AuthContextProps>({
    user: { username: '', accessToken: '' },
    updateUser: (username: string, accessToken: string) => { }
});

function setAuthToken(token: string) {
    localStorage.setItem('authToken', token);
}
function setAuthUsername(username: string) {
    localStorage.setItem('username', username);
}
export const AuthContextProvider = ({ children }: { children: React.ReactNode }) => {
    const [user, setUser] = useState<User>({ username: '', accessToken: '' });

    const updateUser = (username: string, accessToken: string) => {
        setUser({ username, accessToken });
        setAuthToken(accessToken);
        setAuthUsername(username);
    }
    useEffect(() => {
        const accessToken = localStorage.getItem('authToken');
        const username = localStorage.getItem('username');
        if (accessToken && username) {
            setUser({
                username, accessToken
            })
        }
    }, [])

    return (
        <AuthContext.Provider value={{ user, updateUser }}>
            {children}
        </AuthContext.Provider>
    )
}

export const UserAuth = () => {
    return useContext(AuthContext);
};