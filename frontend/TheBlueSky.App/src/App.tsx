import './App.css'
import { RouterProvider } from 'react-router'
import { Toaster } from 'sonner'

import router from './router'


function App() {

  return (
    <>
      <RouterProvider router={router} />
      <Toaster />
    </>
  )
}

export default App
