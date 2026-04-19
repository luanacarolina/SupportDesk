export default function TicketFilters({ filters, onChange, onSubmit, onClear }) {
  return (
    <div className="card shadow-sm mb-4">
      <div className="card-body">
        <h5 className="card-title mb-3">Filtros</h5>

        <form className="row g-3" onSubmit={onSubmit}>
          <div className="col-md-4">
            <label className="form-label">Cliente</label>
            <input
              type="text"
              className="form-control"
              placeholder="Digite o nome do cliente"
              value={filters.clientName}
              onChange={(e) => onChange("clientName", e.target.value)}
            />
          </div>

          <div className="col-md-3">
            <label className="form-label">Prioridade</label>
            <select
              className="form-select"
              value={filters.priority}
              onChange={(e) => onChange("priority", e.target.value)}
            >
              <option value="">Todas</option>
              <option value="1">Baixa</option>
              <option value="2">Média</option>
              <option value="3">Alta</option>
            </select>
          </div>

          <div className="col-md-3">
            <label className="form-label">Status</label>
            <select
              className="form-select"
              value={filters.status}
              onChange={(e) => onChange("status", e.target.value)}
            >
              <option value="">Todos</option>
              <option value="1">Aberto</option>
              <option value="2">Em Andamento</option>
              <option value="3">Finalizado</option>
            </select>
          </div>

          <div className="col-md-2 d-flex align-items-end gap-2">
            <button type="submit" className="btn btn-primary w-100">
              Filtrar
            </button>
            <button
              type="button"
              className="btn btn-outline-secondary w-100"
              onClick={onClear}
            >
              Limpar
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}