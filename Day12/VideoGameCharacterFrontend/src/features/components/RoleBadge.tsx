import styles from './RoleBadge.module.css';

// Deterministic color per role string, so "Tank" always renders the same
// color everywhere without needing a hardcoded enum (Role is free text in the API).
const PALETTE = ['#4c9eff', '#3ecf8e', '#ff5470', '#b98cff', '#f5b942', '#34e4ea'];

function colorForRole(role: string): string {
  let hash = 0;
  for (let i = 0; i < role.length; i++) {
    hash = role.charCodeAt(i) + ((hash << 5) - hash);
  }
  return PALETTE[Math.abs(hash) % PALETTE.length];
}

export function RoleBadge({ role }: { role: string }) {
  const color = colorForRole(role || 'Unknown');
  return (
    <span className={styles.badge} style={{ '--badge-color': color } as React.CSSProperties}>
      {role}
    </span>
  );
}