import { useEffect, useState } from "react";
import TicketFilters from "../components/TicketFilters";
import TicketsTable from "../components/TicketsTable";
import { getTickets } from "../services/supportTicketsService";

export default function Tickets({ onLogout }) {
  const [tickets, setTickets] = useState([]);
  const [loading, setLoading] = useState(false);

  const [filters, setFilters] = useState({
    clientName: "",
    priority: "",
    status: "",
  });

  const loadTickets = async (customFilters = filters) => {
    try {
      setLoading(true);

      const params = {};

      if (customFilters.clientName) {
        params.clientName = customFilters.clientName;
      }

      if (customFilters.priority) {
        params.priority = customFilters.priority;
      }

      if (customFilters.status) {
        params.status = customFilters.status;
      }

      const data = await getTickets(params);
      setTickets(data);
    } catch (error) {
      console.error("Erro ao buscar chamados:", error);

      if (error.response?.status === 401) {
        localStorage.removeItem("token");
        onLogout();
      }
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadTickets();
  }, []);

  const handleFilterChange = (field, value) => {
    setFilters((prev) => ({
      ...prev,
      [field]: value,
    }));
  };

  const handleFilterSubmit = async (event) => {
    event.preventDefault();
    await loadTickets(filters);
  };

  const handleClearFilters = async () => {
    const clearedFilters = {
      clientName: "",
      priority: "",
      status: "",
    };

    setFilters(clearedFilters);
    await loadTickets(clearedFilters);
  };

  const handleLogout = () => {
    localStorage.removeItem("token");
    onLogout();
  };

  return (
    <div className="container py-4">
      <div className="d-flex justify-content-between align-items-center mb-4">
        <div>
          <h2 className="mb-1">SupportDesk</h2>
          <p className="text-muted mb-0">Gerenciamento de chamados técnicos</p>
        </div>

        <button className="btn btn-outline-danger" onClick={handleLogout}>
          Sair
        </button>
      </div>

      <TicketFilters
        filters={filters}
        onChange={handleFilterChange}
        onSubmit={handleFilterSubmit}
        onClear={handleClearFilters}
      />

      <TicketsTable tickets={tickets} loading={loading} />
    </div>
  );
}