# Database scripts

## Seed data

| File | Description |
|------|-------------|
| [`seed-database.sql`](seed-database.sql) | Populates `specialities`, `doctors`, `patients`, and `appointments` |
| [`generate-seed-database.py`](generate-seed-database.py) | Regenerates `seed-database.sql` from the generator definitions |

### Medical specialties source (Brazil)

The 55 specialties follow the official list homologated by:

- **Resolução CFM nº 2.380, de 18 de junho de 2024** (homologa a Portaria CME nº 1/2024)
- **Official publication (Diário Oficial da União):** https://www.in.gov.br/web/dou/-/resolucao-cfm-n-2.380-de-18-de-junho-de-2024-567502141

### Run seed

```bash
docker compose -f containers/docker-compose.dev.yml up -d
psql -h localhost -U postgres -d medical_scheduling -f scripts/seed-database.sql
```

Password: `postgres` (see `containers/docker-compose.dev.yml`).
