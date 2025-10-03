import './App.css'
import { RouterProvider } from 'react-router'
import { Toaster } from 'sonner'

import router from '@/routes'


function App() {

  return (
    <>
      <RouterProvider router={router} />
      <Toaster />
    </>
  )
}

export default App
