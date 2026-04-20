import { useState } from "react";
import { updateTicketStatus } from "../services/supportTicketsService";
function getPriorityLabel(priority) {
  switch (priority) {
    case 1:
      return "Baixa";
    case 2:
      return "Média";
    case 3:
      return "Alta";
    default:
      return priority;
  }
}

function formatDate(dateValue) {
  if (!dateValue) return "-";

  return new Date(dateValue).toLocaleString("pt-BR");
}

export default function TicketsTable({
  tickets,
  loading,
  onStatusUpdated,
  onUnauthorized,
}) {
  const [updatingId, setUpdatingId] = useState(null);
  const [error, setError] = useState("");

  const handleStatusChange = async (ticketId, newStatus) => {
    setError("");
    setUpdatingId(ticketId);
    try {
      await updateTicketStatus(ticketId, newStatus);
    } catch (error) {
      console.error("Erro ao atualizar status:", error);
      console.error("Response data:", error.response?.data);
      console.error("Response status:", error.response?.status);

      if (error.response?.status === 401) {
        localStorage.removeItem("token");
        onUnauthorized();
        return;
      }

      setError("Não foi possível atualizar o status do chamado.");
      setUpdatingId(null);
      return;
    }
    try {
      await onStatusUpdated();
    } catch (error) {
      console.error("Erro ao recarregar chamados:", error);
      setError("Status atualizado, mas houve erro ao recarregar a lista.");
    } finally {
      setUpdatingId(null);
    }
  };

  if (loading) {
    return (
      <div className="card shadow-sm">
        <div className="card-body text-center py-4">
          <div className="spinner-border" role="status" />
          <p className="mt-3 mb-0">Carregando chamados...</p>
        </div>
      </div>
    );
  }

  return (
    <div className="card shadow-sm">
      <div className="card-body">
        <h5 className="card-title mb-3">Lista de Chamados</h5>

        {error && <div className="alert alert-danger">{error}</div>}

        <div className="table-responsive">
          <table className="table table-striped table-hover align-middle">
            <thead>
              <tr>
                <th>Cliente</th>
                <th>Descrição</th>
                <th>Prioridade</th>
                <th>Status</th>
                <th>Criado em</th>
              </tr>
            </thead>
            <tbody>
              {tickets.length > 0 ? (
                tickets.map((ticket) => (
                  <tr key={ticket.id}>
                    <td>{ticket.clientName}</td>
                    <td>{ticket.problemDescription}</td>
                    <td>{getPriorityLabel(ticket.priority)}</td>
                    <td>
                      <select
                        className="form-select form-select-sm"
                        value={ticket.status}
                        disabled={updatingId === ticket.id}
                        onChange={(e) =>
                          handleStatusChange(ticket.id, e.target.value)
                        }
                      >
                        <option value={1}>Open</option>
                        <option value={2}>In Progress</option>
                        <option value={3}>Completed</option>
                      </select>
                    </td>
                    <td>{formatDate(ticket.createdAt)}</td>
                  </tr>
                ))
              ) : (
                <tr>
                  <td colSpan="5" className="text-center py-4">
                    Nenhum chamado encontrado.
                  </td>
                </tr>
              )}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
}
