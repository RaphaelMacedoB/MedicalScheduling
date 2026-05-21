export function Card({ children, className = '', padding = 'md' }) {
  return <div className={`card card--pad-${padding} ${className}`.trim()}>{children}</div>;
}
