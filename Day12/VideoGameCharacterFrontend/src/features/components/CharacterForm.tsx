import { useState, type FormEvent } from 'react';

import { Button } from '../../components/Button/Button';
import type { Character, CreateCharacterRequest, UpdateCharacterRequest } from '../../types/character.types';
import styles from './CharacterForm.module.css';

interface CharacterFormProps {
  initialValue?: Character;
  onSubmit: (data: CreateCharacterRequest | UpdateCharacterRequest) => Promise<void>;
  onCancel: () => void;
}

export function CharacterForm({ initialValue, onSubmit, onCancel }: CharacterFormProps) {
  const [name, setName] = useState(initialValue?.name ?? '');
  const [game, setGame] = useState(initialValue?.game ?? '');
  const [role, setRole] = useState(initialValue?.role ?? '');
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const handleSubmit = async (e:FormEvent) => {

      e.preventDefault();
      
      // Basic client-side validation — mirror this on the API side too (you likely
      // already have [Required] attributes on the DTOs, but never trust the client alone)
      if (!name.trim() || !game.trim() || !role.trim()) {
          setError('All fields are required.');
          return;
        }
        
        setIsSubmitting(true);
        setError(null);
        try {
            const payload= { name: name.trim(), game: game.trim(), role: role.trim() }
            await onSubmit( initialValue? {id:initialValue.id,...payload}:payload);
        } catch {
            setError('Something went wrong while saving. Please try again.');
        } finally {
            setIsSubmitting(false);
        }
    };


  return (
    <form className={styles.form} onSubmit={handleSubmit}>
      <label className={styles.field}>
        <span className={styles.label}>Name</span>
        <input className={styles.input} value={name} onChange={(e) => setName(e.target.value)} placeholder="Aloy" />
      </label>

      <label className={styles.field}>
        <span className={styles.label}>Game</span>
        <input className={styles.input} value={game} onChange={(e) => setGame(e.target.value)} placeholder="Horizon Zero Dawn" />
      </label>

      <label className={styles.field}>
        <span className={styles.label}>Role</span>
        <input className={styles.input} value={role} onChange={(e) => setRole(e.target.value)} placeholder="DPS" />
      </label>

      {error && <p className={styles.error}>{error}</p>}

      <div className={styles.footer}>
        <Button type="button" variant="ghost" onClick={onCancel}>Cancel</Button>
        <Button type="submit" disabled={isSubmitting}>
          {isSubmitting ? 'Saving…' : initialValue ? 'Save changes' : 'Add character'}
        </Button>
      </div>
    </form>
  );

}