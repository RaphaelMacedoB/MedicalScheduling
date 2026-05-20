-- =============================================================================
-- MedicalScheduling - Database seed script
-- =============================================================================
-- Populates: specialities, doctors, patients, appointments
--
-- Medical specialties source (official, Brazil):
--   Resolução CFM nº 2.380, de 18 de junho de 2024 (homologa Portaria CME nº 1/2024)
--   https://www.in.gov.br/web/dou/-/resolucao-cfm-n-2.380-de-18-de-junho-de-2024-567502141
--
-- Usage (PostgreSQL):
--   psql -h localhost -U postgres -d medical_scheduling -f scripts/seed-database.sql
-- =============================================================================

BEGIN;

-- Uncomment to reset seed data:
-- TRUNCATE TABLE appointments, doctors, patients, specialities RESTART IDENTITY CASCADE;

-- -----------------------------------------------------------------------------
-- Specialities (55 medical specialties recognized by CFM)
-- -----------------------------------------------------------------------------

INSERT INTO specialities ("Id", "Name", "Description", "CreatedAt")
VALUES
    ('10000000-0000-4000-8000-000000000001', 'Acupuntura', 'Especialidade médica reconhecida pelo CFM conforme Portaria CME nº 1/2024, homologada pela Resolução CFM nº 2.380/2024.', NOW()),
    ('10000000-0000-4000-8000-000000000002', 'Alergia e imunologia', 'Especialidade médica reconhecida pelo CFM conforme Portaria CME nº 1/2024, homologada pela Resolução CFM nº 2.380/2024.', NOW()),
    ('10000000-0000-4000-8000-000000000003', 'Anestesiologia', 'Especialidade médica reconhecida pelo CFM conforme Portaria CME nº 1/2024, homologada pela Resolução CFM nº 2.380/2024.', NOW()),
    ('10000000-0000-4000-8000-000000000004', 'Angiologia', 'Especialidade médica reconhecida pelo CFM conforme Portaria CME nº 1/2024, homologada pela Resolução CFM nº 2.380/2024.', NOW()),
    ('10000000-0000-4000-8000-000000000005', 'Cardiologia', 'Especialidade médica reconhecida pelo CFM conforme Portaria CME nº 1/2024, homologada pela Resolução CFM nº 2.380/2024.', NOW()),
    ('10000000-0000-4000-8000-000000000006', 'Cirurgia cardiovascular', 'Especialidade médica reconhecida pelo CFM conforme Portaria CME nº 1/2024, homologada pela Resolução CFM nº 2.380/2024.', NOW()),
    ('10000000-0000-4000-8000-000000000007', 'Cirurgia da mão', 'Especialidade médica reconhecida pelo CFM conforme Portaria CME nº 1/2024, homologada pela Resolução CFM nº 2.380/2024.', NOW()),
    ('10000000-0000-4000-8000-000000000008', 'Cirurgia de cabeça e pescoço', 'Especialidade médica reconhecida pelo CFM conforme Portaria CME nº 1/2024, homologada pela Resolução CFM nº 2.380/2024.', NOW()),
    ('10000000-0000-4000-8000-000000000009', 'Cirurgia do aparelho digestivo', 'Especialidade médica reconhecida pelo CFM conforme Portaria CME nº 1/2024, homologada pela Resolução CFM nº 2.380/2024.', NOW()),
    ('10000000-0000-4000-8000-00000000000a', 'Cirurgia geral', 'Especialidade médica reconhecida pelo CFM conforme Portaria CME nº 1/2024, homologada pela Resolução CFM nº 2.380/2024.', NOW()),
    ('10000000-0000-4000-8000-00000000000b', 'Cirurgia oncológica', 'Especialidade médica reconhecida pelo CFM conforme Portaria CME nº 1/2024, homologada pela Resolução CFM nº 2.380/2024.', NOW()),
    ('10000000-0000-4000-8000-00000000000c', 'Cirurgia pediátrica', 'Especialidade médica reconhecida pelo CFM conforme Portaria CME nº 1/2024, homologada pela Resolução CFM nº 2.380/2024.', NOW()),
    ('10000000-0000-4000-8000-00000000000d', 'Cirurgia plástica', 'Especialidade médica reconhecida pelo CFM conforme Portaria CME nº 1/2024, homologada pela Resolução CFM nº 2.380/2024.', NOW()),
    ('10000000-0000-4000-8000-00000000000e', 'Cirurgia torácica', 'Especialidade médica reconhecida pelo CFM conforme Portaria CME nº 1/2024, homologada pela Resolução CFM nº 2.380/2024.', NOW()),
    ('10000000-0000-4000-8000-00000000000f', 'Cirurgia vascular', 'Especialidade médica reconhecida pelo CFM conforme Portaria CME nº 1/2024, homologada pela Resolução CFM nº 2.380/2024.', NOW()),
    ('10000000-0000-4000-8000-000000000010', 'Clínica médica', 'Especialidade médica reconhecida pelo CFM conforme Portaria CME nº 1/2024, homologada pela Resolução CFM nº 2.380/2024.', NOW()),
    ('10000000-0000-4000-8000-000000000011', 'Coloproctologia', 'Especialidade médica reconhecida pelo CFM conforme Portaria CME nº 1/2024, homologada pela Resolução CFM nº 2.380/2024.', NOW()),
    ('10000000-0000-4000-8000-000000000012', 'Dermatologia', 'Especialidade médica reconhecida pelo CFM conforme Portaria CME nº 1/2024, homologada pela Resolução CFM nº 2.380/2024.', NOW()),
    ('10000000-0000-4000-8000-000000000013', 'Endocrinologia e metabologia', 'Especialidade médica reconhecida pelo CFM conforme Portaria CME nº 1/2024, homologada pela Resolução CFM nº 2.380/2024.', NOW()),
    ('10000000-0000-4000-8000-000000000014', 'Endoscopia', 'Especialidade médica reconhecida pelo CFM conforme Portaria CME nº 1/2024, homologada pela Resolução CFM nº 2.380/2024.', NOW()),
    ('10000000-0000-4000-8000-000000000015', 'Gastroenterologia', 'Especialidade médica reconhecida pelo CFM conforme Portaria CME nº 1/2024, homologada pela Resolução CFM nº 2.380/2024.', NOW()),
    ('10000000-0000-4000-8000-000000000016', 'Genética médica', 'Especialidade médica reconhecida pelo CFM conforme Portaria CME nº 1/2024, homologada pela Resolução CFM nº 2.380/2024.', NOW()),
    ('10000000-0000-4000-8000-000000000017', 'Geriatria', 'Especialidade médica reconhecida pelo CFM conforme Portaria CME nº 1/2024, homologada pela Resolução CFM nº 2.380/2024.', NOW()),
    ('10000000-0000-4000-8000-000000000018', 'Ginecologia e obstetrícia', 'Especialidade médica reconhecida pelo CFM conforme Portaria CME nº 1/2024, homologada pela Resolução CFM nº 2.380/2024.', NOW()),
    ('10000000-0000-4000-8000-000000000019', 'Hematologia e hemoterapia', 'Especialidade médica reconhecida pelo CFM conforme Portaria CME nº 1/2024, homologada pela Resolução CFM nº 2.380/2024.', NOW()),
    ('10000000-0000-4000-8000-00000000001a', 'Homeopatia', 'Especialidade médica reconhecida pelo CFM conforme Portaria CME nº 1/2024, homologada pela Resolução CFM nº 2.380/2024.', NOW()),
    ('10000000-0000-4000-8000-00000000001b', 'Infectologia', 'Especialidade médica reconhecida pelo CFM conforme Portaria CME nº 1/2024, homologada pela Resolução CFM nº 2.380/2024.', NOW()),
    ('10000000-0000-4000-8000-00000000001c', 'Mastologia', 'Especialidade médica reconhecida pelo CFM conforme Portaria CME nº 1/2024, homologada pela Resolução CFM nº 2.380/2024.', NOW()),
    ('10000000-0000-4000-8000-00000000001d', 'Medicina de emergência', 'Especialidade médica reconhecida pelo CFM conforme Portaria CME nº 1/2024, homologada pela Resolução CFM nº 2.380/2024.', NOW()),
    ('10000000-0000-4000-8000-00000000001e', 'Medicina de família e comunidade', 'Especialidade médica reconhecida pelo CFM conforme Portaria CME nº 1/2024, homologada pela Resolução CFM nº 2.380/2024.', NOW()),
    ('10000000-0000-4000-8000-00000000001f', 'Medicina do trabalho', 'Especialidade médica reconhecida pelo CFM conforme Portaria CME nº 1/2024, homologada pela Resolução CFM nº 2.380/2024.', NOW()),
    ('10000000-0000-4000-8000-000000000020', 'Medicina do tráfego', 'Especialidade médica reconhecida pelo CFM conforme Portaria CME nº 1/2024, homologada pela Resolução CFM nº 2.380/2024.', NOW()),
    ('10000000-0000-4000-8000-000000000021', 'Medicina esportiva', 'Especialidade médica reconhecida pelo CFM conforme Portaria CME nº 1/2024, homologada pela Resolução CFM nº 2.380/2024.', NOW()),
    ('10000000-0000-4000-8000-000000000022', 'Medicina física e reabilitação', 'Especialidade médica reconhecida pelo CFM conforme Portaria CME nº 1/2024, homologada pela Resolução CFM nº 2.380/2024.', NOW()),
    ('10000000-0000-4000-8000-000000000023', 'Medicina intensiva', 'Especialidade médica reconhecida pelo CFM conforme Portaria CME nº 1/2024, homologada pela Resolução CFM nº 2.380/2024.', NOW()),
    ('10000000-0000-4000-8000-000000000024', 'Medicina legal e perícia médica', 'Especialidade médica reconhecida pelo CFM conforme Portaria CME nº 1/2024, homologada pela Resolução CFM nº 2.380/2024.', NOW()),
    ('10000000-0000-4000-8000-000000000025', 'Medicina nuclear', 'Especialidade médica reconhecida pelo CFM conforme Portaria CME nº 1/2024, homologada pela Resolução CFM nº 2.380/2024.', NOW()),
    ('10000000-0000-4000-8000-000000000026', 'Medicina preventiva e social', 'Especialidade médica reconhecida pelo CFM conforme Portaria CME nº 1/2024, homologada pela Resolução CFM nº 2.380/2024.', NOW()),
    ('10000000-0000-4000-8000-000000000027', 'Nefrologia', 'Especialidade médica reconhecida pelo CFM conforme Portaria CME nº 1/2024, homologada pela Resolução CFM nº 2.380/2024.', NOW()),
    ('10000000-0000-4000-8000-000000000028', 'Neurocirurgia', 'Especialidade médica reconhecida pelo CFM conforme Portaria CME nº 1/2024, homologada pela Resolução CFM nº 2.380/2024.', NOW()),
    ('10000000-0000-4000-8000-000000000029', 'Neurologia', 'Especialidade médica reconhecida pelo CFM conforme Portaria CME nº 1/2024, homologada pela Resolução CFM nº 2.380/2024.', NOW()),
    ('10000000-0000-4000-8000-00000000002a', 'Nutrologia', 'Especialidade médica reconhecida pelo CFM conforme Portaria CME nº 1/2024, homologada pela Resolução CFM nº 2.380/2024.', NOW()),
    ('10000000-0000-4000-8000-00000000002b', 'Oftalmologia', 'Especialidade médica reconhecida pelo CFM conforme Portaria CME nº 1/2024, homologada pela Resolução CFM nº 2.380/2024.', NOW()),
    ('10000000-0000-4000-8000-00000000002c', 'Oncologia clínica', 'Especialidade médica reconhecida pelo CFM conforme Portaria CME nº 1/2024, homologada pela Resolução CFM nº 2.380/2024.', NOW()),
    ('10000000-0000-4000-8000-00000000002d', 'Ortopedia e traumatologia', 'Especialidade médica reconhecida pelo CFM conforme Portaria CME nº 1/2024, homologada pela Resolução CFM nº 2.380/2024.', NOW()),
    ('10000000-0000-4000-8000-00000000002e', 'Otorrinolaringologia', 'Especialidade médica reconhecida pelo CFM conforme Portaria CME nº 1/2024, homologada pela Resolução CFM nº 2.380/2024.', NOW()),
    ('10000000-0000-4000-8000-00000000002f', 'Patologia', 'Especialidade médica reconhecida pelo CFM conforme Portaria CME nº 1/2024, homologada pela Resolução CFM nº 2.380/2024.', NOW()),
    ('10000000-0000-4000-8000-000000000030', 'Patologia clínica/medicina laboratorial', 'Especialidade médica reconhecida pelo CFM conforme Portaria CME nº 1/2024, homologada pela Resolução CFM nº 2.380/2024.', NOW()),
    ('10000000-0000-4000-8000-000000000031', 'Pediatria', 'Especialidade médica reconhecida pelo CFM conforme Portaria CME nº 1/2024, homologada pela Resolução CFM nº 2.380/2024.', NOW()),
    ('10000000-0000-4000-8000-000000000032', 'Pneumologia', 'Especialidade médica reconhecida pelo CFM conforme Portaria CME nº 1/2024, homologada pela Resolução CFM nº 2.380/2024.', NOW()),
    ('10000000-0000-4000-8000-000000000033', 'Psiquiatria', 'Especialidade médica reconhecida pelo CFM conforme Portaria CME nº 1/2024, homologada pela Resolução CFM nº 2.380/2024.', NOW()),
    ('10000000-0000-4000-8000-000000000034', 'Radiologia e diagnóstico por imagem', 'Especialidade médica reconhecida pelo CFM conforme Portaria CME nº 1/2024, homologada pela Resolução CFM nº 2.380/2024.', NOW()),
    ('10000000-0000-4000-8000-000000000035', 'Radioterapia', 'Especialidade médica reconhecida pelo CFM conforme Portaria CME nº 1/2024, homologada pela Resolução CFM nº 2.380/2024.', NOW()),
    ('10000000-0000-4000-8000-000000000036', 'Reumatologia', 'Especialidade médica reconhecida pelo CFM conforme Portaria CME nº 1/2024, homologada pela Resolução CFM nº 2.380/2024.', NOW()),
    ('10000000-0000-4000-8000-000000000037', 'Urologia', 'Especialidade médica reconhecida pelo CFM conforme Portaria CME nº 1/2024, homologada pela Resolução CFM nº 2.380/2024.', NOW())
ON CONFLICT ("Name") DO NOTHING;

-- -----------------------------------------------------------------------------
-- Doctors
-- -----------------------------------------------------------------------------

INSERT INTO doctors ("Id", "Name", "Crm", email, phone, "SpecialityId", "IsActive", "CreatedAt")
VALUES
    ('20000000-0000-4000-8000-000000000001', 'Dr. Ana Paula Ribeiro', 'CRM-SP100001', 'ana.ribeiro@clinica.med.br', '11987001001', '10000000-0000-4000-8000-000000000005', TRUE, NOW()),
    ('20000000-0000-4000-8000-000000000002', 'Dr. Carlos Eduardo Lima', 'CRM-SP100002', 'carlos.lima@clinica.med.br', '11987001002', '10000000-0000-4000-8000-000000000005', TRUE, NOW()),
    ('20000000-0000-4000-8000-000000000003', 'Dr. Fernanda Souza', 'CRM-RJ100003', 'fernanda.souza@clinica.med.br', '21987001003', '10000000-0000-4000-8000-000000000012', TRUE, NOW()),
    ('20000000-0000-4000-8000-000000000004', 'Dr. Ricardo Mendes', 'CRM-MG100004', 'ricardo.mendes@clinica.med.br', '31987001004', '10000000-0000-4000-8000-000000000018', TRUE, NOW()),
    ('20000000-0000-4000-8000-000000000005', 'Dr. Juliana Costa', 'CRM-SP100005', 'juliana.costa@clinica.med.br', '11987001005', '10000000-0000-4000-8000-000000000018', TRUE, NOW()),
    ('20000000-0000-4000-8000-000000000006', 'Dr. Paulo Henrique Alves', 'CRM-RS100006', 'paulo.alves@clinica.med.br', '51987001006', '10000000-0000-4000-8000-000000000031', TRUE, NOW()),
    ('20000000-0000-4000-8000-000000000007', 'Dr. Mariana Duarte', 'CRM-PR100007', 'mariana.duarte@clinica.med.br', '41987001007', '10000000-0000-4000-8000-000000000031', TRUE, NOW()),
    ('20000000-0000-4000-8000-000000000008', 'Dr. Roberto Nascimento', 'CRM-SP100008', 'roberto.nascimento@clinica.med.br', '11987001008', '10000000-0000-4000-8000-000000000029', TRUE, NOW()),
    ('20000000-0000-4000-8000-000000000009', 'Dr. Lucia Ferreira', 'CRM-BA100009', 'lucia.ferreira@clinica.med.br', '71987001009', '10000000-0000-4000-8000-000000000029', TRUE, NOW()),
    ('20000000-0000-4000-8000-00000000000a', 'Dr. André Martins', 'CRM-SP100010', 'andre.martins@clinica.med.br', '11987001010', '10000000-0000-4000-8000-00000000002d', TRUE, NOW()),
    ('20000000-0000-4000-8000-00000000000b', 'Dr. Camila Rocha', 'CRM-SC100011', 'camila.rocha@clinica.med.br', '48987001011', '10000000-0000-4000-8000-00000000002d', TRUE, NOW()),
    ('20000000-0000-4000-8000-00000000000c', 'Dr. Henrique Barbosa', 'CRM-SP100012', 'henrique.barbosa@clinica.med.br', '11987001012', '10000000-0000-4000-8000-000000000037', TRUE, NOW()),
    ('20000000-0000-4000-8000-00000000000d', 'Dr. Patricia Gomes', 'CRM-PE100013', 'patricia.gomes@clinica.med.br', '81987001013', '10000000-0000-4000-8000-000000000037', TRUE, NOW()),
    ('20000000-0000-4000-8000-00000000000e', 'Dr. Marcos Vieira', 'CRM-SP100014', 'marcos.vieira@clinica.med.br', '11987001014', '10000000-0000-4000-8000-000000000003', TRUE, NOW()),
    ('20000000-0000-4000-8000-00000000000f', 'Dr. Helena Prado', 'CRM-DF100015', 'helena.prado@clinica.med.br', '61987001015', '10000000-0000-4000-8000-000000000033', TRUE, NOW()),
    ('20000000-0000-4000-8000-000000000010', 'Dr. Gustavo Pires', 'CRM-SP100016', 'gustavo.pires@clinica.med.br', '11987001016', '10000000-0000-4000-8000-000000000032', TRUE, NOW()),
    ('20000000-0000-4000-8000-000000000011', 'Dr. Isabela Moura', 'CRM-GO100017', 'isabela.moura@clinica.med.br', '62987001017', '10000000-0000-4000-8000-000000000010', TRUE, NOW()),
    ('20000000-0000-4000-8000-000000000012', 'Dr. Thiago Campos', 'CRM-SP100018', 'thiago.campos@clinica.med.br', '11987001018', '10000000-0000-4000-8000-000000000010', TRUE, NOW()),
    ('20000000-0000-4000-8000-000000000013', 'Dr. Beatriz Lopes', 'CRM-CE100019', 'beatriz.lopes@clinica.med.br', '85987001019', '10000000-0000-4000-8000-00000000002b', TRUE, NOW()),
    ('20000000-0000-4000-8000-000000000014', 'Dr. Daniel Araujo', 'CRM-SP100020', 'daniel.araujo@clinica.med.br', '11987001020', '10000000-0000-4000-8000-00000000002b', TRUE, NOW())
ON CONFLICT ("Crm") DO NOTHING;

-- -----------------------------------------------------------------------------
-- Patients
-- -----------------------------------------------------------------------------

INSERT INTO patients ("Id", "Name", email, cpf, phone, "BirthDate", "IsActive", "CreatedAt")
VALUES
    ('30000000-0000-4000-8000-000000000001', 'Maria Silva Santos', 'maria.silva@email.com', '52998224725', '11987654321', DATE '1990-03-15', TRUE, NOW()),
    ('30000000-0000-4000-8000-000000000002', 'João Pedro Oliveira', 'joao.oliveira@email.com', '39053344705', '11976543210', DATE '1985-07-22', TRUE, NOW()),
    ('30000000-0000-4000-8000-000000000003', 'Ana Carolina Ferreira', 'ana.ferreira@email.com', '11144477735', '21965432109', DATE '1992-11-08', TRUE, NOW()),
    ('30000000-0000-4000-8000-000000000004', 'Lucas Martins Pereira', 'lucas.pereira@email.com', '12345678909', '31954321098', DATE '1988-01-30', TRUE, NOW()),
    ('30000000-0000-4000-8000-000000000005', 'Juliana Rodrigues Lima', 'juliana.lima@email.com', '23456789092', '41943210987', DATE '1995-05-12', TRUE, NOW()),
    ('30000000-0000-4000-8000-000000000006', 'Pedro Henrique Costa', 'pedro.costa@email.com', '34567890175', '51932109876', DATE '1979-09-25', TRUE, NOW()),
    ('30000000-0000-4000-8000-000000000007', 'Fernanda Almeida Souza', 'fernanda.souza.pac@email.com', '45678901249', '61921098765', DATE '2000-12-03', TRUE, NOW()),
    ('30000000-0000-4000-8000-000000000008', 'Rafael Barbosa Nunes', 'rafael.nunes@email.com', '56789012303', '71910987654', DATE '1993-08-17', TRUE, NOW()),
    ('30000000-0000-4000-8000-000000000009', 'Camila Dias Rocha', 'camila.rocha.pac@email.com', '67890123469', '81909876543', DATE '1987-04-09', TRUE, NOW()),
    ('30000000-0000-4000-8000-00000000000a', 'Bruno Teixeira Melo', 'bruno.melo@email.com', '78901234505', '91998765432', DATE '1998-06-28', TRUE, NOW())
ON CONFLICT (cpf) DO NOTHING;

-- -----------------------------------------------------------------------------
-- Appointments
-- -----------------------------------------------------------------------------

INSERT INTO appointments ("Id", "PatientId", "DoctorId", start_at, end_at, "Status", "Notes", "CreatedAt")
VALUES
    ('40000000-0000-4000-8000-000000000001', '30000000-0000-4000-8000-000000000001', '20000000-0000-4000-8000-000000000001', TIMESTAMPTZ '2026-06-02 13:00:00+00', TIMESTAMPTZ '2026-06-02 14:00:00+00', 'Scheduled', NULL, NOW()),
    ('40000000-0000-4000-8000-000000000002', '30000000-0000-4000-8000-000000000002', '20000000-0000-4000-8000-000000000001', TIMESTAMPTZ '2026-06-03 14:00:00+00', TIMESTAMPTZ '2026-06-03 15:00:00+00', 'Confirmed', NULL, NOW()),
    ('40000000-0000-4000-8000-000000000003', '30000000-0000-4000-8000-000000000003', '20000000-0000-4000-8000-000000000002', TIMESTAMPTZ '2026-06-04 13:00:00+00', TIMESTAMPTZ '2026-06-04 14:00:00+00', 'Scheduled', NULL, NOW()),
    ('40000000-0000-4000-8000-000000000004', '30000000-0000-4000-8000-000000000004', '20000000-0000-4000-8000-000000000006', TIMESTAMPTZ '2026-06-05 14:00:00+00', TIMESTAMPTZ '2026-06-05 15:00:00+00', 'Confirmed', NULL, NOW()),
    ('40000000-0000-4000-8000-000000000005', '30000000-0000-4000-8000-000000000005', '20000000-0000-4000-8000-000000000007', TIMESTAMPTZ '2026-06-06 13:00:00+00', TIMESTAMPTZ '2026-06-06 14:00:00+00', 'Scheduled', NULL, NOW()),
    ('40000000-0000-4000-8000-000000000006', '30000000-0000-4000-8000-000000000006', '20000000-0000-4000-8000-000000000009', TIMESTAMPTZ '2026-06-09 15:00:00+00', TIMESTAMPTZ '2026-06-09 16:00:00+00', 'Scheduled', NULL, NOW()),
    ('40000000-0000-4000-8000-000000000007', '30000000-0000-4000-8000-000000000007', '20000000-0000-4000-8000-00000000000a', TIMESTAMPTZ '2026-06-10 13:00:00+00', TIMESTAMPTZ '2026-06-10 14:00:00+00', 'Confirmed', NULL, NOW()),
    ('40000000-0000-4000-8000-000000000008', '30000000-0000-4000-8000-000000000008', '20000000-0000-4000-8000-00000000000c', TIMESTAMPTZ '2026-06-11 14:00:00+00', TIMESTAMPTZ '2026-06-11 15:00:00+00', 'Scheduled', NULL, NOW()),
    ('40000000-0000-4000-8000-000000000009', '30000000-0000-4000-8000-000000000009', '20000000-0000-4000-8000-00000000000e', TIMESTAMPTZ '2026-05-15 13:00:00+00', TIMESTAMPTZ '2026-05-15 14:00:00+00', 'Completed', 'Retorno em 30 dias.', NOW()),
    ('40000000-0000-4000-8000-00000000000a', '30000000-0000-4000-8000-00000000000a', '20000000-0000-4000-8000-00000000000f', TIMESTAMPTZ '2026-05-10 14:00:00+00', TIMESTAMPTZ '2026-05-10 15:00:00+00', 'Completed', 'Exames solicitados.', NOW()),
    ('40000000-0000-4000-8000-00000000000b', '30000000-0000-4000-8000-000000000001', '20000000-0000-4000-8000-000000000011', TIMESTAMPTZ '2026-05-08 13:00:00+00', TIMESTAMPTZ '2026-05-08 14:00:00+00', 'Cancelled', NULL, NOW()),
    ('40000000-0000-4000-8000-00000000000c', '30000000-0000-4000-8000-000000000002', '20000000-0000-4000-8000-000000000012', TIMESTAMPTZ '2026-06-12 13:00:00+00', TIMESTAMPTZ '2026-06-12 14:00:00+00', 'Scheduled', NULL, NOW()),
    ('40000000-0000-4000-8000-00000000000d', '30000000-0000-4000-8000-000000000003', '20000000-0000-4000-8000-000000000013', TIMESTAMPTZ '2026-06-13 14:00:00+00', TIMESTAMPTZ '2026-06-13 15:00:00+00', 'Confirmed', NULL, NOW()),
    ('40000000-0000-4000-8000-00000000000e', '30000000-0000-4000-8000-000000000004', '20000000-0000-4000-8000-000000000014', TIMESTAMPTZ '2026-06-16 13:00:00+00', TIMESTAMPTZ '2026-06-16 14:00:00+00', 'Scheduled', NULL, NOW()),
    ('40000000-0000-4000-8000-00000000000f', '30000000-0000-4000-8000-000000000005', '20000000-0000-4000-8000-000000000003', TIMESTAMPTZ '2026-05-20 14:00:00+00', TIMESTAMPTZ '2026-05-20 15:00:00+00', 'Completed', 'Consulta de rotina.', NOW()),
    ('40000000-0000-4000-8000-000000000010', '30000000-0000-4000-8000-000000000006', '20000000-0000-4000-8000-000000000004', TIMESTAMPTZ '2026-06-17 13:00:00+00', TIMESTAMPTZ '2026-06-17 14:00:00+00', 'Scheduled', NULL, NOW()),
    ('40000000-0000-4000-8000-000000000011', '30000000-0000-4000-8000-000000000007', '20000000-0000-4000-8000-000000000005', TIMESTAMPTZ '2026-06-18 14:00:00+00', TIMESTAMPTZ '2026-06-18 15:00:00+00', 'Confirmed', NULL, NOW()),
    ('40000000-0000-4000-8000-000000000012', '30000000-0000-4000-8000-000000000008', '20000000-0000-4000-8000-000000000008', TIMESTAMPTZ '2026-06-19 13:00:00+00', TIMESTAMPTZ '2026-06-19 14:00:00+00', 'Scheduled', NULL, NOW()),
    ('40000000-0000-4000-8000-000000000013', '30000000-0000-4000-8000-000000000009', '20000000-0000-4000-8000-00000000000b', TIMESTAMPTZ '2026-05-25 14:00:00+00', TIMESTAMPTZ '2026-05-25 15:00:00+00', 'Completed', NULL, NOW()),
    ('40000000-0000-4000-8000-000000000014', '30000000-0000-4000-8000-00000000000a', '20000000-0000-4000-8000-00000000000d', TIMESTAMPTZ '2026-06-20 13:00:00+00', TIMESTAMPTZ '2026-06-20 14:00:00+00', 'Scheduled', NULL, NOW())
ON CONFLICT ("Id") DO NOTHING;

COMMIT;
