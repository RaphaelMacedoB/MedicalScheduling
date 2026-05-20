import { Navigate, Route, Routes } from 'react-router-dom'
import { Layout } from './components/Layout'
import { AppointmentsPage } from './pages/AppointmentsPage'
import { DashboardPage } from './pages/DashboardPage'
import { DoctorAgendaPage } from './pages/DoctorAgendaPage'
import { DoctorsPage } from './pages/DoctorsPage'
import { PatientsPage } from './pages/PatientsPage'
import { SpecialitiesPage } from './pages/SpecialitiesPage'

export default function App() {
  return (
    <Routes>
      <Route path="/" element={<Layout />}>
        <Route index element={<DashboardPage />} />
        <Route path="patients" element={<PatientsPage />} />
        <Route path="doctors" element={<DoctorsPage />} />
        <Route path="specialities" element={<SpecialitiesPage />} />
        <Route path="appointments" element={<AppointmentsPage />} />
        <Route path="agenda" element={<DoctorAgendaPage />} />
        <Route path="*" element={<Navigate to="/" replace />} />
      </Route>
    </Routes>
  )
}
