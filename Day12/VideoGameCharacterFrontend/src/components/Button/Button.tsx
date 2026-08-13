
import type { ButtonHTMLAttributes } from "react";

import styles from './Button.module.css'

type Variant ='primary'|'ghost'|'danger';

interface ButtonProps extends ButtonHTMLAttributes<HTMLButtonElement>{
    variant?:Variant;
}

export function Button({variant='primary',className,children,...rest}:ButtonProps){
    return (
        <button className={`${styles.button}${styles[variant]}${className?? ''}`}{...rest}>
            {children}
        </button>
    )
}