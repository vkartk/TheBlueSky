import './App.css'
import { Route, Routes } from 'react-router'
import LoginPage from './pages/auth/login'

function App() {

  return (
    <>
      <Routes>
        <Route path="/" element="Home" />
        <Route path='/login' element={<LoginPage/>} />
      </Routes>
    </>
  )
}

export default App
