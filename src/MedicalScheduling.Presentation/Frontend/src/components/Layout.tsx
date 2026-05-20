import { NavLink, Outlet } from 'react-router-dom';

const links = [
  { to: '/', label: 'Painel', end: true },
  { to: '/agenda', label: 'Agendas', end: false },
  { to: '/appointments', label: 'Consultas', end: false },
  { to: '/patients', label: 'Pacientes', end: false },
  { to: '/doctors', label: 'Médicos', end: false },
  { to: '/specialities', label: 'Especialidades', end: false },
];

export function Layout() {
  return (
    <div className="app">
      <header className="header">
        <div className="header__brand">
          <span className="header__logo" aria-hidden>
            +
          </span>
          <div>
            <h1>Medical Scheduling</h1>
            <p className="header__tagline">Administração do sistema</p>
          </div>
        </div>
        <nav className="header__nav">
          {links.map((link) => (
            <NavLink
              key={link.to}
              to={link.to}
              end={link.end}
              className={({ isActive }) =>
                isActive ? 'nav-link nav-link--active' : 'nav-link'
              }
            >
              {link.label}
            </NavLink>
          ))}
        </nav>
      </header>
      <main className="main">
        <Outlet />
      </main>
    </div>
  );
}
