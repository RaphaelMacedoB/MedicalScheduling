import { fetchDoctors } from '../api/pagedFetchers';
import { ListToolbar } from '../components/ListToolbar';
import { Pagination } from '../components/Pagination';
import { Card } from '../components/ui/Card';
import { PageHeader } from '../components/ui/PageHeader';
import { Select } from '../components/ui/FormField';
import { Spinner } from '../components/ui/Spinner';
import { usePagedList } from '../hooks/usePagedList';

export function DoctorsPage() {
  const list = usePagedList({
    fetcher: fetchDoctors,
  });

  return (
    <section>
      <PageHeader title="Médicos" subtitle="Profissionais e especialidades" />

      <Card padding="md" className="toolbar-card">
        <ListToolbar
          search={list.search}
          onSearchChange={list.setSearch}
          onSearchSubmit={list.reload}
          sortBy={list.sortBy}
          sortDirection={list.sortDirection}
          sortOptions={[
            { value: 'name', label: 'Nome' },
            { value: 'crm', label: 'CRM' },
            { value: 'speciality', label: 'Especialidade' },
          ]}
          onSortByChange={list.setSortBy}
          onSortDirectionChange={list.setSortDirection}
        >
          <Select
            value={String(list.filters.isActive ?? '')}
            onChange={(e) =>
              list.setFilter('isActive', e.target.value === '' ? undefined : e.target.value === 'true')
            }
          >
            <option value="">Todos os status</option>
            <option value="true">Ativos</option>
            <option value="false">Inativos</option>
          </Select>
        </ListToolbar>
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
                  <th>CRM</th>
                  <th>Especialidade</th>
                  <th>E-mail</th>
                  <th>Ativo</th>
                </tr>
              </thead>
              <tbody>
                {list.data.items.map((d) => (
                  <tr key={d.id}>
                    <td>{d.name}</td>
                    <td>{d.crm}</td>
                    <td>{d.specialityName}</td>
                    <td>{d.email}</td>
                    <td>{d.isActive ? 'Sim' : 'Não'}</td>
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
