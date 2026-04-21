import { GoogleLogin } from "@react-oauth/google";
import { loginWithGoogle } from "../services/authService";

export default function GoogleLoginButton({ onLogin }) {
  const handleSuccess = async (credentialResponse) => {
    try {
      const data = await loginWithGoogle(credentialResponse.credential);
      localStorage.setItem("token", data.token);
      onLogin(data.token);
    } catch (error) {
      console.error("Erro no login Google:", error);
    }
  };

  return (
    <GoogleLogin
      onSuccess={handleSuccess}
      onError={() => console.error("Falha no login Google")}
    />
  );
}