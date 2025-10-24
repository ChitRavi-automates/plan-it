import styles from './CallToAction.module.css';
import { Link } from 'react-router-dom';

function CallToAction() {
  return (
    <section className={styles.ctaSection}>
      <h2 className={styles.heading}>Redo att komma igång?</h2>
      <p className={styles.subtext}>Skapa ett konto och börja planera smartare med ditt team idag.</p>
      <button className={styles.ctaButton}><Link to="/sign-up">Skapa konto</Link></button>
    </section>
  );
}

export default CallToAction;