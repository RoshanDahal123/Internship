


import { BrowserRouter, Route, Routes } from "react-router";
import Display from "./pages/Display";
import UserForm from "./pages/UserForm";
const App = () => {
  return (
    <BrowserRouter>
     
     <Routes>
      
      <Route path='/' element={<UserForm/>} />
      <Route path='/display' element={<Display/>}/>
      
    </Routes>
    </BrowserRouter>
  )
}

export default App