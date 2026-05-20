import { useCallback, useEffect, useRef, useState } from 'react';
import type { PagedListParams } from '../api/pagedFetchers';
import type { PagedResponse, SortDirection } from '../types/api';

export type PagedListFetcher<T> = (params: PagedListParams) => Promise<PagedResponse<T>>;

interface UsePagedListOptions<T> {
  fetcher: PagedListFetcher<T>;
  initialPageSize?: number;
}

export function usePagedList<T>({ fetcher, initialPageSize = 10 }: UsePagedListOptions<T>) {
  const fetcherRef = useRef(fetcher);
  fetcherRef.current = fetcher;

  const [data, setData] = useState<PagedResponse<T> | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState(initialPageSize);
  const [search, setSearch] = useState('');
  const [appliedSearch, setAppliedSearch] = useState('');
  const [sortBy, setSortBy] = useState<string | undefined>();
  const [sortDirection, setSortDirection] = useState<SortDirection>('Asc');
  const [filters, setFilters] = useState<
    Record<string, string | number | boolean | undefined | null>
  >({});

  const load = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      const result = await fetcherRef.current({
        page,
        pageSize,
        search: appliedSearch || undefined,
        sortBy,
        sortDirection,
        ...filters,
      });
      setData(result);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to load data');
      setData(null);
    } finally {
      setLoading(false);
    }
  }, [page, pageSize, appliedSearch, sortBy, sortDirection, filters]);

  useEffect(() => {
    void load();
  }, [load]);

  const setFilter = useCallback(
    (key: string, value: string | number | boolean | undefined | null) => {
      setFilters((prev) => ({ ...prev, [key]: value }));
      setPage(1);
    },
    [],
  );

  const setSearchDraft = useCallback((value: string) => {
    setSearch(value);
  }, []);

  const reload = useCallback(() => {
    setAppliedSearch(search);
    setPage(1);
  }, [search]);

  const setSortByField = useCallback((field: string | undefined) => {
    setSortBy(field || undefined);
    setPage(1);
  }, []);

  const setSortDirectionValue = useCallback((direction: SortDirection) => {
    setSortDirection(direction);
    setPage(1);
  }, []);

  const setPageSizeAndReset = useCallback((size: number) => {
    setPageSize(size);
    setPage(1);
  }, []);

  return {
    data,
    loading,
    error,
    page,
    pageSize,
    search,
    sortBy,
    sortDirection,
    filters,
    setPage,
    setPageSize,
    setSearch: setSearchDraft,
    setFilter,
    setSortBy: setSortByField,
    setSortDirection: setSortDirectionValue,
    /** Applies current search draft and resets to page 1. */
    reload,
    /** Re-fetches with current list state (use after create/update/delete). */
    refresh: load,
    setPageSizeAndReset,
  };
}
