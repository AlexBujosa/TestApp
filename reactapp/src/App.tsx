import './App.css';
import { Route, Routes } from 'react-router-dom'
import Home from './components/home/Home';
import { Login } from './components/login/Login';
function App() {
  return (
    <div className="App">
      <Routes>
        <Route path='/' />
      </Routes>
      <Routes>
        <Route path='/' element={<Home />} />
        <Route path='/login' element={<Login />} />
        <Route path="*" element={<Home />} />
      </Routes>
    </div>
  );
}

export default App;
