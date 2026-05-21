import { useCallback, useEffect, useRef, useState } from 'react';

export function usePagedList({ fetcher, initialPageSize = 10 }) {
  const fetcherRef = useRef(fetcher);
  fetcherRef.current = fetcher;

  const [data, setData] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState(initialPageSize);
  const [search, setSearch] = useState('');
  const [appliedSearch, setAppliedSearch] = useState('');
  const [sortBy, setSortBy] = useState(undefined);
  const [sortDirection, setSortDirection] = useState('Asc');
  const [filters, setFilters] = useState({});

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

  const setFilter = useCallback((key, value) => {
    setFilters((prev) => ({ ...prev, [key]: value }));
    setPage(1);
  }, []);

  const setSearchDraft = useCallback((value) => {
    setSearch(value);
  }, []);

  const reload = useCallback(() => {
    setAppliedSearch(search);
    setPage(1);
  }, [search]);

  const setSortByField = useCallback((field) => {
    setSortBy(field || undefined);
    setPage(1);
  }, []);

  const setSortDirectionValue = useCallback((direction) => {
    setSortDirection(direction);
    setPage(1);
  }, []);

  const setPageSizeAndReset = useCallback((size) => {
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
    reload,
    refresh: load,
    setPageSizeAndReset,
  };
}
