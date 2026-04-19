import api from "./api";

export async function getTickets(params = {}) {
  const response = await api.get("/SupportTickets", { params });
  return response.data;
}