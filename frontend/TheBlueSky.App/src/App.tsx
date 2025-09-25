import './App.css'
import { Route, Routes } from 'react-router'
import { Toaster } from 'sonner'
import LoginPage from './pages/auth/login'
import RegisterPage from './pages/auth/register'

function App() {

  return (
    <>
      <Routes>
        <Route path="/" element="Home" />
        <Route path='/login' element={<LoginPage/>} />
        <Route path='/register' element={<RegisterPage />} />
      </Routes>
      <Toaster />
    </>
  )
}

export default App
