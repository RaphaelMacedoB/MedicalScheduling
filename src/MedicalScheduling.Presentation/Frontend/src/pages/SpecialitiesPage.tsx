import { fetchSpecialities } from '../api/pagedFetchers';
import { ListToolbar } from '../components/ListToolbar';
import { Pagination } from '../components/Pagination';
import { Card } from '../components/ui/Card';
import { PageHeader } from '../components/ui/PageHeader';
import { Spinner } from '../components/ui/Spinner';
import { usePagedList } from '../hooks/usePagedList';
import type { SpecialityDto } from '../types/api';

export function SpecialitiesPage() {
  const list = usePagedList<SpecialityDto>({
    fetcher: fetchSpecialities,
  });

  return (
    <section>
      <PageHeader title="Especialidades" subtitle="Áreas de atuação médica" />

      <Card padding="md" className="toolbar-card">
        <ListToolbar
          search={list.search}
          onSearchChange={list.setSearch}
          onSearchSubmit={list.reload}
          sortBy={list.sortBy}
          sortDirection={list.sortDirection}
          sortOptions={[
            { value: 'name', label: 'Nome' },
            { value: 'createdat', label: 'Criado em' },
          ]}
          onSortByChange={list.setSortBy}
          onSortDirectionChange={list.setSortDirection}
        />
      </Card>

      {list.loading && <Spinner />}
      {list.error && <p className="error">{list.error}</p>}
      {list.data && (
        <>
          <div className="table-wrap">
            <table className="data-table">
              <thead>
                <tr>
                  <th>Nome</th>
                  <th>Descrição</th>
                </tr>
              </thead>
              <tbody>
                {list.data.items.map((s) => (
                  <tr key={s.id}>
                    <td>{s.name}</td>
                    <td>{s.description ?? '—'}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
          <Pagination
            currentPage={list.data.currentPage}
            totalPages={list.data.totalPages}
            totalItems={list.data.totalItems}
            pageSize={list.data.pageSize}
            hasNextPage={list.data.hasNextPage}
            hasPreviousPage={list.data.hasPreviousPage}
            onPageChange={list.setPage}
            onPageSizeChange={list.setPageSizeAndReset}
          />
        </>
      )}
    </section>
  );
}
