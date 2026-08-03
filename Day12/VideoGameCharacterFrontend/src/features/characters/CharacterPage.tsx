import { useState } from "react";
import { Button } from "../../components/Button/Button";
import { Modal } from "../../components/Modal/Modal";
import { Spinner } from "../../components/Spinner/Spinner";
import { useCharacters } from "../../hooks/useCharacters";
import type { Character } from "../../types/character.types";
import { CharacterForm } from "../components/CharacterForm";
import { CharacterList } from "../components/CharacterList";
import styles from './CharacterPage.module.css';

type ModalState =
  | { mode: 'closed' }
  | { mode: 'create' }
  | { mode: 'edit'; character: Character }
  | { mode: 'delete'; character: Character };


  export function CharacterPage(){
     const { characters, isLoading, error, createCharacter, updateCharacter, deleteCharacter } = useCharacters();
     const [modal,setModal]=useState<ModalState>({mode:'closed'});
     const closeModal=()=>setModal({mode:"closed"});

     return (
     <div className={styles.page}>
        <header className={styles.header}>
            <div>
          <h1 className={styles.title}>Character Roster</h1>
          <p className={styles.subtitle}>{characters.length} character{characters.length !== 1 ? 's' : ''} tracked</p>
        </div>
        <Button onClick={() => setModal({ mode: 'create' })}>+ Add character</Button>
        </header>


      {isLoading && <Spinner />}
      {error && <p className={styles.errorBanner}>{error}</p>}
      {!isLoading && !error && (
        <CharacterList
          characters={characters}
          onEdit={(character) => setModal({ mode: 'edit', character })}
          onDelete={(character) => setModal({ mode: 'delete', character })}
        />
      )}
       <Modal isOpen={modal.mode === 'create'} onClose={closeModal} title="Add character">
        <CharacterForm
          onSubmit={async (data) => {
            await createCharacter(data);
            closeModal();
          }}
          onCancel={closeModal}
        />
      </Modal>

      <Modal isOpen={modal.mode === 'edit'} onClose={closeModal} title="Edit character">
        {modal.mode === 'edit' && (
          <CharacterForm
            initialValue={modal.character}
            onSubmit={async (data) => {
              await updateCharacter(modal.character.id,data);
              closeModal();
            }}
            onCancel={closeModal}
          />
        )}
      </Modal>

            <Modal isOpen={modal.mode === 'delete'} onClose={closeModal} title="Delete character">
        {modal.mode === 'delete' && (
          <div className={styles.confirmDelete}>
            <p>Remove <strong>{modal.character.name}</strong> from the roster? This can't be undone.</p>
            <div className={styles.confirmActions}>
              <Button variant="ghost" onClick={closeModal}>Cancel</Button>
              <Button
                variant="danger"
                onClick={async () => {
                  await deleteCharacter(modal.character.id);
                  closeModal();
                }}
              >
                Delete
              </Button>
            </div>
          </div>
        )}
      </Modal>

     </div>)

  }