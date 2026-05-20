import type { ReactNode } from 'react';
import type { SortDirection } from '../types/api';

interface ListToolbarProps {
  search: string;
  onSearchChange: (value: string) => void;
  onSearchSubmit: () => void;
  sortBy?: string;
  sortDirection?: SortDirection;
  sortOptions: { value: string; label: string }[];
  onSortByChange: (value: string) => void;
  onSortDirectionChange: (value: SortDirection) => void;
  children?: ReactNode;
}

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
}: ListToolbarProps) {
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
        onChange={(e) => onSortDirectionChange(e.target.value as SortDirection)}
      >
        <option value="Asc">Ascendente</option>
        <option value="Desc">Descendente</option>
      </select>
      <button type="submit">Aplicar</button>
      {children}
    </form>
  );
}
