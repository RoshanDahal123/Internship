// src/App.tsx
import { BrowserRouter, Route, Routes } from "react-router";
import { AuthBootstrap } from "./components/auth/auth-bootstrap";
import { RequireAdmin } from "./components/auth/require-admin";

import { MainLayout } from "./components/layouts/main-layout";
import Login from "./pages/Login";
import Register from "./pages/Register";
import StudentDetail from "./pages/StudentDetail";
import StudentForm from "./pages/StudentForm";
import StudentList from "./pages/StudentList";

const App = () => {
  return (
    <AuthBootstrap>
      <BrowserRouter>
        <Routes>
          <Route element={<MainLayout />}>
            <Route path="/" element={<StudentList />} />
            <Route path="/students/:id" element={<StudentDetail />} />
            <Route path="/login" element={<Login />} />
            <Route path="/register" element={<Register />} />

            {/* Admin-only: RequireAdmin bounces guests to /login */}
            <Route element={<RequireAdmin />}>
              <Route path="/students/new" element={<StudentForm mode="create" />} />
              <Route path="/students/:id/edit" element={<StudentForm mode="edit" />} />
            </Route>
          </Route>
        </Routes>
      </BrowserRouter>
    </AuthBootstrap>
  );
};

export default App;