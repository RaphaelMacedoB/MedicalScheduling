"""Generates scripts/seed-database.sql from the Domain schema and CFM specialty list."""

from __future__ import annotations

from pathlib import Path
from textwrap import dedent

SOURCE_URL = (
    "https://www.in.gov.br/web/dou/-/resolucao-cfm-n-2.380-de-18-de-junho-de-2024-567502141"
)
SOURCE_LABEL = "Resolução CFM nº 2.380, de 18 de junho de 2024 (homologa Portaria CME nº 1/2024)"

SPECIALITIES = [
    "Acupuntura",
    "Alergia e imunologia",
    "Anestesiologia",
    "Angiologia",
    "Cardiologia",
    "Cirurgia cardiovascular",
    "Cirurgia da mão",
    "Cirurgia de cabeça e pescoço",
    "Cirurgia do aparelho digestivo",
    "Cirurgia geral",
    "Cirurgia oncológica",
    "Cirurgia pediátrica",
    "Cirurgia plástica",
    "Cirurgia torácica",
    "Cirurgia vascular",
    "Clínica médica",
    "Coloproctologia",
    "Dermatologia",
    "Endocrinologia e metabologia",
    "Endoscopia",
    "Gastroenterologia",
    "Genética médica",
    "Geriatria",
    "Ginecologia e obstetrícia",
    "Hematologia e hemoterapia",
    "Homeopatia",
    "Infectologia",
    "Mastologia",
    "Medicina de emergência",
    "Medicina de família e comunidade",
    "Medicina do trabalho",
    "Medicina do tráfego",
    "Medicina esportiva",
    "Medicina física e reabilitação",
    "Medicina intensiva",
    "Medicina legal e perícia médica",
    "Medicina nuclear",
    "Medicina preventiva e social",
    "Nefrologia",
    "Neurocirurgia",
    "Neurologia",
    "Nutrologia",
    "Oftalmologia",
    "Oncologia clínica",
    "Ortopedia e traumatologia",
    "Otorrinolaringologia",
    "Patologia",
    "Patologia clínica/medicina laboratorial",
    "Pediatria",
    "Pneumologia",
    "Psiquiatria",
    "Radiologia e diagnóstico por imagem",
    "Radioterapia",
    "Reumatologia",
    "Urologia",
]

DOCTORS = [
    ("Dr. Ana Paula Ribeiro", "CRM-SP100001", "ana.ribeiro@clinica.med.br", "11987001001", 5),
    ("Dr. Carlos Eduardo Lima", "CRM-SP100002", "carlos.lima@clinica.med.br", "11987001002", 5),
    ("Dr. Fernanda Souza", "CRM-RJ100003", "fernanda.souza@clinica.med.br", "21987001003", 18),
    ("Dr. Ricardo Mendes", "CRM-MG100004", "ricardo.mendes@clinica.med.br", "31987001004", 24),
    ("Dr. Juliana Costa", "CRM-SP100005", "juliana.costa@clinica.med.br", "11987001005", 24),
    ("Dr. Paulo Henrique Alves", "CRM-RS100006", "paulo.alves@clinica.med.br", "51987001006", 49),
    ("Dr. Mariana Duarte", "CRM-PR100007", "mariana.duarte@clinica.med.br", "41987001007", 49),
    ("Dr. Roberto Nascimento", "CRM-SP100008", "roberto.nascimento@clinica.med.br", "11987001008", 41),
    ("Dr. Lucia Ferreira", "CRM-BA100009", "lucia.ferreira@clinica.med.br", "71987001009", 41),
    ("Dr. André Martins", "CRM-SP100010", "andre.martins@clinica.med.br", "11987001010", 45),
    ("Dr. Camila Rocha", "CRM-SC100011", "camila.rocha@clinica.med.br", "48987001011", 45),
    ("Dr. Henrique Barbosa", "CRM-SP100012", "henrique.barbosa@clinica.med.br", "11987001012", 55),
    ("Dr. Patricia Gomes", "CRM-PE100013", "patricia.gomes@clinica.med.br", "81987001013", 55),
    ("Dr. Marcos Vieira", "CRM-SP100014", "marcos.vieira@clinica.med.br", "11987001014", 3),
    ("Dr. Helena Prado", "CRM-DF100015", "helena.prado@clinica.med.br", "61987001015", 51),
    ("Dr. Gustavo Pires", "CRM-SP100016", "gustavo.pires@clinica.med.br", "11987001016", 50),
    ("Dr. Isabela Moura", "CRM-GO100017", "isabela.moura@clinica.med.br", "62987001017", 16),
    ("Dr. Thiago Campos", "CRM-SP100018", "thiago.campos@clinica.med.br", "11987001018", 16),
    ("Dr. Beatriz Lopes", "CRM-CE100019", "beatriz.lopes@clinica.med.br", "85987001019", 43),
    ("Dr. Daniel Araujo", "CRM-SP100020", "daniel.araujo@clinica.med.br", "11987001020", 43),
]

PATIENTS = [
    ("Maria Silva Santos", "maria.silva@email.com", "52998224725", "11987654321", "1990-03-15"),
    ("João Pedro Oliveira", "joao.oliveira@email.com", "39053344705", "11976543210", "1985-07-22"),
    ("Ana Carolina Ferreira", "ana.ferreira@email.com", "11144477735", "21965432109", "1992-11-08"),
    ("Lucas Martins Pereira", "lucas.pereira@email.com", "12345678909", "31954321098", "1988-01-30"),
    ("Juliana Rodrigues Lima", "juliana.lima@email.com", "23456789092", "41943210987", "1995-05-12"),
    ("Pedro Henrique Costa", "pedro.costa@email.com", "34567890175", "51932109876", "1979-09-25"),
    ("Fernanda Almeida Souza", "fernanda.souza.pac@email.com", "45678901249", "61921098765", "2000-12-03"),
    ("Rafael Barbosa Nunes", "rafael.nunes@email.com", "56789012303", "71910987654", "1993-08-17"),
    ("Camila Dias Rocha", "camila.rocha.pac@email.com", "67890123469", "81909876543", "1987-04-09"),
    ("Bruno Teixeira Melo", "bruno.melo@email.com", "78901234505", "91998765432", "1998-06-28"),
]

APPOINTMENTS = [
    (1, 1, 1, "2026-06-02 13:00:00+00", "2026-06-02 14:00:00+00", "Scheduled", None),
    (2, 2, 1, "2026-06-03 14:00:00+00", "2026-06-03 15:00:00+00", "Confirmed", None),
    (3, 3, 2, "2026-06-04 13:00:00+00", "2026-06-04 14:00:00+00", "Scheduled", None),
    (4, 4, 6, "2026-06-05 14:00:00+00", "2026-06-05 15:00:00+00", "Confirmed", None),
    (5, 5, 7, "2026-06-06 13:00:00+00", "2026-06-06 14:00:00+00", "Scheduled", None),
    (6, 6, 9, "2026-06-09 15:00:00+00", "2026-06-09 16:00:00+00", "Scheduled", None),
    (7, 7, 10, "2026-06-10 13:00:00+00", "2026-06-10 14:00:00+00", "Confirmed", None),
    (8, 8, 12, "2026-06-11 14:00:00+00", "2026-06-11 15:00:00+00", "Scheduled", None),
    (9, 9, 14, "2026-05-15 13:00:00+00", "2026-05-15 14:00:00+00", "Completed", "Retorno em 30 dias."),
    (10, 10, 15, "2026-05-10 14:00:00+00", "2026-05-10 15:00:00+00", "Completed", "Exames solicitados."),
    (11, 1, 17, "2026-05-08 13:00:00+00", "2026-05-08 14:00:00+00", "Cancelled", None),
    (12, 2, 18, "2026-06-12 13:00:00+00", "2026-06-12 14:00:00+00", "Scheduled", None),
    (13, 3, 19, "2026-06-13 14:00:00+00", "2026-06-13 15:00:00+00", "Confirmed", None),
    (14, 4, 20, "2026-06-16 13:00:00+00", "2026-06-16 14:00:00+00", "Scheduled", None),
    (15, 5, 3, "2026-05-20 14:00:00+00", "2026-05-20 15:00:00+00", "Completed", "Consulta de rotina."),
    (16, 6, 4, "2026-06-17 13:00:00+00", "2026-06-17 14:00:00+00", "Scheduled", None),
    (17, 7, 5, "2026-06-18 14:00:00+00", "2026-06-18 15:00:00+00", "Confirmed", None),
    (18, 8, 8, "2026-06-19 13:00:00+00", "2026-06-19 14:00:00+00", "Scheduled", None),
    (19, 9, 11, "2026-05-25 14:00:00+00", "2026-05-25 15:00:00+00", "Completed", None),
    (20, 10, 13, "2026-06-20 13:00:00+00", "2026-06-20 14:00:00+00", "Scheduled", None),
]


def uuid(prefix: str, index: int) -> str:
    return f"{prefix}-0000-4000-8000-{index:012x}"


def sql_escape(value: str) -> str:
    return value.replace("'", "''")


def main() -> None:
    lines: list[str] = [
        "-- =============================================================================",
        "-- MedicalScheduling - Database seed script",
        "-- =============================================================================",
        "-- Populates: specialities, doctors, patients, appointments",
        "--",
        "-- Medical specialties source (official, Brazil):",
        f"--   {SOURCE_LABEL}",
        f"--   {SOURCE_URL}",
        "--",
        "-- Usage (PostgreSQL):",
        "--   psql -h localhost -U postgres -d medical_scheduling -f scripts/seed-database.sql",
        "-- =============================================================================",
        "",
        "BEGIN;",
        "",
        "-- Uncomment to reset seed data:",
        "-- TRUNCATE TABLE appointments, doctors, patients, specialities RESTART IDENTITY CASCADE;",
        "",
        "-- -----------------------------------------------------------------------------",
        "-- Specialities (55 medical specialties recognized by CFM)",
        "-- -----------------------------------------------------------------------------",
        "",
    ]

    speciality_rows = []
    for index, name in enumerate(SPECIALITIES, start=1):
        description = (
            "Especialidade médica reconhecida pelo CFM conforme "
            "Portaria CME nº 1/2024, homologada pela Resolução CFM nº 2.380/2024."
        )
        speciality_rows.append(
            f"    ('{uuid('10000000', index)}', '{sql_escape(name)}', "
            f"'{sql_escape(description)}', NOW())"
        )

    lines.extend(
        [
            "INSERT INTO specialities (\"Id\", \"Name\", \"Description\", \"CreatedAt\")",
            "VALUES",
            ",\n".join(speciality_rows),
            "ON CONFLICT (\"Name\") DO NOTHING;",
            "",
            "-- -----------------------------------------------------------------------------",
            "-- Doctors",
            "-- -----------------------------------------------------------------------------",
            "",
        ]
    )

    doctor_rows = []
    for index, (name, crm, email, phone, speciality_index) in enumerate(DOCTORS, start=1):
        doctor_rows.append(
            f"    ('{uuid('20000000', index)}', '{sql_escape(name)}', '{crm}', "
            f"'{email}', '{phone}', '{uuid('10000000', speciality_index)}', TRUE, NOW())"
        )

    lines.extend(
        [
            "INSERT INTO doctors (\"Id\", \"Name\", \"Crm\", email, phone, \"SpecialityId\", \"IsActive\", \"CreatedAt\")",
            "VALUES",
            ",\n".join(doctor_rows),
            "ON CONFLICT (\"Crm\") DO NOTHING;",
            "",
            "-- -----------------------------------------------------------------------------",
            "-- Patients",
            "-- -----------------------------------------------------------------------------",
            "",
        ]
    )

    patient_rows = []
    for index, (name, email, cpf, phone, birth_date) in enumerate(PATIENTS, start=1):
        patient_rows.append(
            f"    ('{uuid('30000000', index)}', '{sql_escape(name)}', '{email}', "
            f"'{cpf}', '{phone}', DATE '{birth_date}', TRUE, NOW())"
        )

    lines.extend(
        [
            "INSERT INTO patients (\"Id\", \"Name\", email, cpf, phone, \"BirthDate\", \"IsActive\", \"CreatedAt\")",
            "VALUES",
            ",\n".join(patient_rows),
            "ON CONFLICT (cpf) DO NOTHING;",
            "",
            "-- -----------------------------------------------------------------------------",
            "-- Appointments",
            "-- -----------------------------------------------------------------------------",
            "",
        ]
    )

    appointment_rows = []
    for index, (_appt_no, patient_index, doctor_index, start_at, end_at, status, notes) in enumerate(
        APPOINTMENTS, start=1
    ):
        notes_sql = "NULL" if notes is None else f"'{sql_escape(notes)}'"
        appointment_rows.append(
            f"    ('{uuid('40000000', index)}', '{uuid('30000000', patient_index)}', "
            f"'{uuid('20000000', doctor_index)}', TIMESTAMPTZ '{start_at}', "
            f"TIMESTAMPTZ '{end_at}', '{status}', {notes_sql}, NOW())"
        )

    lines.extend(
        [
            "INSERT INTO appointments (\"Id\", \"PatientId\", \"DoctorId\", start_at, end_at, \"Status\", \"Notes\", \"CreatedAt\")",
            "VALUES",
            ",\n".join(appointment_rows),
            "ON CONFLICT (\"Id\") DO NOTHING;",
            "",
            "COMMIT;",
            "",
        ]
    )

    output = Path(__file__).resolve().parent / "seed-database.sql"
    output.write_text("\n".join(lines), encoding="utf-8")
    print(f"Wrote {output} ({len(SPECIALITIES)} specialities, {len(DOCTORS)} doctors, "
          f"{len(PATIENTS)} patients, {len(APPOINTMENTS)} appointments)")


if __name__ == "__main__":
    main()
