export const Logout = () => {
    localStorage.clear();
    window.location.href = "http://localhost:3000/login";
}


export default Logout;