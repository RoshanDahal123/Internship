
import { useState } from 'react'
import { BrowserRouter, Route, Routes } from 'react-router'
import './App.css'
import Display from './pages/Display'
import Form from './pages/Form'
type FormData={
  name:string
}
function App() {
  const [ formData, setFormData]=useState<FormData>({
    name:""
  })
  return <>
  <BrowserRouter>
  <Routes>
    <Route
     path='/'
     element={<Form formData={formData} setFormData={setFormData}/>}

    />
    <Route
     path='/display'
     element={<Display formData={formData}/>}
    />
  </Routes>
  </BrowserRouter>
  </>
}

export default App
