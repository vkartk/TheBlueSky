import './App.css'
import { RouterProvider } from 'react-router'
import { Toaster } from 'sonner'

import router from '@/routes'
import { useAppDispatch, useAppSelector } from '@/store';
import { useEffect } from 'react';
import { fetchCurrentUser } from './features/auth/authThunks';


function App() {

  const dispatch = useAppDispatch();

  const token = useAppSelector(state => state.auth.accessToken);
  const user = useAppSelector(state => state.auth.user);

  useEffect(() => {
    if(token && !user) dispatch(fetchCurrentUser());
  },[token,user])
  
  return (
    <>
      <RouterProvider router={router} />
      <Toaster />
    </>
  )
}

export default App
