


import { BrowserRouter, Route, Routes } from "react-router";
import StudentDetail from "./pages/StudentDetail";
import StudentForm from "./pages/StudentForm";
import StudentList from "./pages/StudentList";
const App = () => {
  return (
    <BrowserRouter>
     
     <Routes>
      <Route path='/' element ={<StudentList/>} />
      <Route path='/students/:id' element={<StudentDetail/>} />
      <Route path='/students/:id/edit' element={<StudentForm mode="edit"/>}/>
      <Route path='students/new' element={<StudentForm mode="create"/>}/>
      
    </Routes>
    </BrowserRouter>
  )
}

export default App