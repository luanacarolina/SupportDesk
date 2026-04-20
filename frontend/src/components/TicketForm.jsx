import { useState } from "react";
import { createTicket } from "../services/supportTicketsService";

export default function TicketForm({ onCreated, onUnauthorized }) {
  const [form, setForm] = useState({
    clientName: "",
    problemDescription: "",
    priority: 1,
  });

  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const [successMessage, setSuccessMessage] = useState("");

  const handleChange = (field, value) => {
    setForm((prev) => ({
      ...prev,
      [field]: value,
    }));
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
    setError("");
    setSuccessMessage("");

    try {
      setLoading(true);

      await createTicket({
        clientName: form.clientName.trim(),
        problemDescription: form.problemDescription.trim(),
        priority: Number(form.priority),
      });

      setSuccessMessage("Chamado criado com sucesso.");
      setForm({
        clientName: "",
        problemDescription: "",
        priority: 1,
      });

      onCreated();
    } catch (error) {
      console.error("Erro ao criar chamado:", error);

      if (error.response?.status === 401) {
        localStorage.removeItem("token");
        onUnauthorized();
        return;
      }

      setError(
        error.response?.data?.message || "Não foi possível criar o chamado."
      );
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="card shadow-sm mb-4">
      <div className="card-body">
        <h5 className="card-title mb-3">Novo Chamado</h5>

        {error && <div className="alert alert-danger">{error}</div>}
        {successMessage && (
          <div className="alert alert-success">{successMessage}</div>
        )}

        <form onSubmit={handleSubmit}>
          <div className="row g-3">
            <div className="col-md-4">
              <label className="form-label">Cliente</label>
              <input
                type="text"
                className="form-control"
                value={form.clientName}
                onChange={(e) => handleChange("clientName", e.target.value)}
                placeholder="Digite o nome do cliente"
                required
              />
            </div>

            <div className="col-md-5">
              <label className="form-label">Descrição do problema</label>
              <input
                type="text"
                className="form-control"
                value={form.problemDescription}
                onChange={(e) =>
                  handleChange("problemDescription", e.target.value)
                }
                placeholder="Descreva o problema"
                required
              />
            </div>

            <div className="col-md-3">
              <label className="form-label">Prioridade</label>
              <select
                className="form-select"
                value={form.priority}
                onChange={(e) => handleChange("priority", e.target.value)}
              >
                <option value={1}>Baixa</option>
                <option value={2}>Média</option>
                <option value={3}>Alta</option>
              </select>
            </div>

            <div className="col-12">
              <button
                type="submit"
                className="btn btn-success"
                disabled={loading}
              >
                {loading ? "Salvando..." : "Cadastrar chamado"}
              </button>
            </div>
          </div>
        </form>
      </div>
    </div>
  );
}