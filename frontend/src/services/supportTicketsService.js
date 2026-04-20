import api from "./api";

export async function getTickets(params = {}) {
  const response = await api.get("/SupportTickets", { params });
  return response.data;
}

export async function createTicket(payload) {
  const response = await api.post("/SupportTickets", payload);
  return response.data;
}
