import api from "./api";

export async function login(username, password) {
  const response = await api.post("/Auth/login", {
    username,
    password,
  });

  return response.data;
}

export async function loginWithGoogle(idToken) {
  const response = await api.post("/Auth/google-login", {
    idToken,
  });

  return response.data;
}
