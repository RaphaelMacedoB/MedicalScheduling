import { fetchPatients } from '../api/pagedFetchers';
import { ListToolbar } from '../components/ListToolbar';
import { Pagination } from '../components/Pagination';
import { Card } from '../components/ui/Card';
import { PageHeader } from '../components/ui/PageHeader';
import { Select } from '../components/ui/FormField';
import { Spinner } from '../components/ui/Spinner';
import { usePagedList } from '../hooks/usePagedList';
import { formatDate } from '../utils/datetime';

export function PatientsPage() {
  const list = usePagedList({
    fetcher: fetchPatients,
  });

  return (
    <section>
      <PageHeader title="Pacientes" subtitle="Cadastro e listagem de pacientes" />

      <Card padding="md" className="toolbar-card">
        <ListToolbar
          search={list.search}
          onSearchChange={list.setSearch}
          onSearchSubmit={list.reload}
          sortBy={list.sortBy}
          sortDirection={list.sortDirection}
          sortOptions={[
            { value: 'name', label: 'Nome' },
            { value: 'email', label: 'E-mail' },
            { value: 'birthdate', label: 'Nascimento' },
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
                  <th>E-mail</th>
                  <th>CPF</th>
                  <th>Telefone</th>
                  <th>Nascimento</th>
                  <th>Ativo</th>
                </tr>
              </thead>
              <tbody>
                {list.data.items.map((p) => (
                  <tr key={p.id}>
                    <td>{p.name}</td>
                    <td>{p.email}</td>
                    <td>{p.cpf}</td>
                    <td>{p.phone}</td>
                    <td>{formatDate(p.birthDate)}</td>
                    <td>{p.isActive ? 'Sim' : 'Não'}</td>
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
