import styles from './WhyChoose.module.css';
import { Check } from 'lucide-react';

function WhyChoose() {
  return (
    <section className={styles.whySection}>
      <div className={styles.header}>
        <h2 className={styles.title}>Varför team väljer PlanIt</h2>
        <p className={styles.description}>
          Team är komplexa, men att hantera dem behöver inte vara det. Planit samlar viktiga verktyg i en sömlös upplevelse.
        </p>
      </div>

      <div className={styles.problemSolutionContainer}>
        <div className={styles.card}>
          <h3 className={styles.cardTitle}>Problemet</h3>
          <ul className={styles.list}>
            <li className={styles.listItem}>
              <div className={styles.problemIcon}>×</div>
              <p>Flera separata verktyg för teamhantering</p>
            </li>
            <li className={styles.listItem}>
              <div className={styles.problemIcon}>×</div>
              <p>Ingen insyn i teamets välmående</p>
            </li>
            <li className={styles.listItem}>
              <div className={styles.problemIcon}>×</div>
              <p>Inget sätt att spåra mål eller framsteg</p>
            </li>
          </ul>
        </div>

        <div className={styles.card}>
          <h3 className={styles.cardTitle}>Vår lösning</h3>
          <ul className={styles.list}>
            <li className={styles.listItem}>
              <div className={styles.solutionIcon}><Check size={14} /></div>
              <p>Allt-i-ett-plattform för team och välmående</p>
            </li>
            <li className={styles.listItem}>
              <div className={styles.solutionIcon}><Check size={14} /></div>
              <p>Regelbundna check-ins och humörspårning</p>
            </li>
            <li className={styles.listItem}>
              <div className={styles.solutionIcon}><Check size={14} /></div>
              <p>Visuella verktyg för att spåra och fira framsteg</p>
            </li>
          </ul>
        </div>
      </div>
    </section>
  );
}

export default WhyChoose;