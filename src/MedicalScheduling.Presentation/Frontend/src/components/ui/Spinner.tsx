export function Spinner({ label = 'Carregando...' }: { label?: string }) {
  return (
    <div className="spinner" role="status">
      <span className="spinner__ring" aria-hidden />
      <span>{label}</span>
    </div>
  );
}
