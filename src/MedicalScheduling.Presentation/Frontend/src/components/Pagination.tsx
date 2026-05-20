interface PaginationProps {
  currentPage: number;
  totalPages: number;
  totalItems: number;
  pageSize: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
  onPageChange: (page: number) => void;
  onPageSizeChange: (size: number) => void;
}

export function Pagination({
  currentPage,
  totalPages,
  totalItems,
  pageSize,
  hasNextPage,
  hasPreviousPage,
  onPageChange,
  onPageSizeChange,
}: PaginationProps) {
  return (
    <div className="pagination">
      <span>
        {totalItems} registro(s) — página {currentPage} de {totalPages || 1}
      </span>
      <div className="pagination-controls">
        <label>
          Por página
          <select value={pageSize} onChange={(e) => onPageSizeChange(Number(e.target.value))}>
            {[5, 10, 20, 50].map((size) => (
              <option key={size} value={size}>
                {size}
              </option>
            ))}
          </select>
        </label>
        <button type="button" disabled={!hasPreviousPage} onClick={() => onPageChange(currentPage - 1)}>
          Anterior
        </button>
        <button type="button" disabled={!hasNextPage} onClick={() => onPageChange(currentPage + 1)}>
          Próxima
        </button>
      </div>
    </div>
  );
}
