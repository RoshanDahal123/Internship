import type { Character } from '../../types/character.types';
import { CharacterCard } from './CharacterCard';

import styles from './CharacterList.module.css';

interface CharacterListProps {
  characters: Character[];
  onEdit: (character: Character) => void;
  onDelete: (character: Character) => void;
}

export function CharacterList({ characters, onEdit, onDelete }: CharacterListProps) {
  if (characters.length === 0) {
    return (
      <div className={styles.empty}>
        <p>No characters in the roster yet.</p>
        <p className={styles.emptySub}>Add your first one to get started.</p>
      </div>
    );
  }

  return (
    <div className={styles.grid}>
      {characters.map((character) => (
        <CharacterCard key={character.id} character={character} onEdit={onEdit} onDelete={onDelete} />
      ))}
    </div>
  );
}