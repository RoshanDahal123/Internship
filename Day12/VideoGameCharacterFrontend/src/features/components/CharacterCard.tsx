
import { RoleBadge } from './RoleBadge';

import styles from './CharacterCard.module.css';
import type { Character } from '../../types/character.types';
import { Button } from '../../components/Button/Button';

interface CharacterCardProps {
  character: Character;
  onEdit: (character: Character) => void;
  onDelete: (character: Character) => void;
}

export function CharacterCard({ character, onEdit, onDelete }: CharacterCardProps) {
  return (
    <article className={styles.card}>
      <div className={styles.stripe} />
      <div className={styles.content}>
        <header className={styles.header}>
          <h3 className={styles.name}>{character.name}</h3>
          <span className={styles.id}>#{String(character.id).padStart(3, '0')}</span>
        </header>
        <p className={styles.game}>{character.game}</p>
        <RoleBadge role={character.role} />
        <div className={styles.actions}>
          <Button variant="ghost" onClick={() => onEdit(character)}>Edit</Button>
          <Button variant="danger" onClick={() => onDelete(character)}>Delete</Button>
        </div>
      </div>
    </article>
  );
}