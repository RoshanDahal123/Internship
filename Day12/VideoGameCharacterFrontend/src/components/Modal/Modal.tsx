import type { ReactNode } from "react";
import { useEffect } from "react";
import { createPortal } from "react-dom";

import styles from './Modal.module.css'

interface ModalProps{
    isOpen:boolean;
    onClose:()=>void;
    title:string;
    children:ReactNode;
}

export function Modal({ isOpen, onClose, title, children }: ModalProps) {
  useEffect(() => {
    const handleEscape = (e: KeyboardEvent) => e.key === 'Escape' && onClose();
    if (isOpen) document.addEventListener('keydown', handleEscape);
    return () => document.removeEventListener('keydown', handleEscape);
  }, [isOpen, onClose]);

  if (!isOpen) return null;

  return createPortal(
    <div className={styles.overlay}
    onClick={onClose}>
<div className={styles.modal} onClick={(e) => e.stopPropagation()} role="dialog" aria-modal="true" aria-labelledby="modal-title">
        <div className={styles.header}>
            <h2 id="modal-title" className={styles.title}>{title}</h2>
          <button className={styles.closeButton} onClick={onClose} aria-label="Close">×</button>
        </div>
{children}
    </div>
    </div>,
    document.body
  );

}