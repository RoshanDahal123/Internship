


import {BrowserRouter, Routes,Route} from "react-router";
import Login from "./pages/Login";
import Display from "./pages/Display";
const App = () => {
  return (
    <BrowserRouter>
     
     <Routes>
      
      <Route path='/' element={<Login/>} />
      <Route path='/display' element={<Display/>}/>
      
    </Routes>
    </BrowserRouter>
  )
}

export default App