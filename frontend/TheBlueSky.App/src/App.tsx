import './App.css'
import { Route, Routes } from 'react-router'
import { Toaster } from 'sonner'
import LoginPage from './pages/auth/login'

function App() {

  return (
    <>
      <Routes>
        <Route path="/" element="Home" />
        <Route path='/login' element={<LoginPage/>} />
      </Routes>
      <Toaster />
    </>
  )
}

export default App
