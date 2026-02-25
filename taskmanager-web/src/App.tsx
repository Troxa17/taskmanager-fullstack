import { BrowserRouter, Routes, Route } from "react-router-dom";
import  LoginPage  from "./pages/LoginPage";
import TasksPage from "./pages/TasksPage";
import ProtectedRoute from "./components/ProtectedRoute";

export default function App() {
  return (
    <BrowserRouter>
      <Routes>
          <Route path = "/login" element = {<LoginPage />} />

          <Route path="/" element = {<ProtectedRoute> <TasksPage/> </ProtectedRoute>}/>
      </Routes>
    </BrowserRouter>
  )
}