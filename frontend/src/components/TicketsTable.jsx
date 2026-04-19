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

function getStatusLabel(status) {
  switch (status) {
    case 1:
      return "Open";
    case 2:
      return "In Progress";
    case 3:
      return "Completed";
    default:
      return status;
  }
}

function formatDate(dateValue) {
  if (!dateValue) return "-";

  return new Date(dateValue).toLocaleString("pt-BR");
}

export default function TicketsTable({ tickets, loading }) {
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
                    <td>{getStatusLabel(ticket.status)}</td>
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