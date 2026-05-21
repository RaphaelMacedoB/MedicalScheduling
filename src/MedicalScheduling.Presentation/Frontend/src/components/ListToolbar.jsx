export function ListToolbar({
  search,
  onSearchChange,
  onSearchSubmit,
  sortBy,
  sortDirection,
  sortOptions,
  onSortByChange,
  onSortDirectionChange,
  children,
}) {
  return (
    <form
      className="toolbar"
      onSubmit={(e) => {
        e.preventDefault();
        onSearchSubmit();
      }}
    >
      <input
        type="search"
        placeholder="Buscar..."
        value={search}
        onChange={(e) => onSearchChange(e.target.value)}
      />
      <select value={sortBy ?? ''} onChange={(e) => onSortByChange(e.target.value)}>
        <option value="">Ordenar por</option>
        {sortOptions.map((opt) => (
          <option key={opt.value} value={opt.value}>
            {opt.label}
          </option>
        ))}
      </select>
      <select
        value={sortDirection ?? 'Asc'}
        onChange={(e) => onSortDirectionChange(e.target.value)}
      >
        <option value="Asc">Ascendente</option>
        <option value="Desc">Descendente</option>
      </select>
      <button type="submit">Aplicar</button>
      {children}
    </form>
  );
}
