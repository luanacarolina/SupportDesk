import { useState } from "react";
import Login from "./pages/Login";
import Tickets from "./pages/Tickets";

function App() {
  const [token, setToken] = useState(localStorage.getItem("token"));

  if (!token) {
    return <Login onLogin={setToken} />;
  }

  return <Tickets onLogout={() => setToken(null)} />;
}

export default App;
