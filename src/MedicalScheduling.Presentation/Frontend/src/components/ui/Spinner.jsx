export function Spinner({ label = 'Carregando...' }) {
  return (
    <div className="spinner" role="status">
      <span className="spinner__ring" aria-hidden />
      <span>{label}</span>
    </div>
  );
}
