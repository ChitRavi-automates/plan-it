import styles from './HowItWorks.module.css';

function HowItWorks() {
  const steps = [
    {
      step: '①',
      title: 'Skapa konto',
      description: 'Börja genom att skapa ett konto för dig och ditt team.',
    },
    {
      step: '②',
      title: 'Lägg till medlemmar',
      description: 'Bjud in teammedlemmar för att samarbeta effektivt.',
    },
    {
      step: '③',
      title: 'Börja planera',
      description: 'Använd våra verktyg för att sätta mål och följa upp dem.',
    },
  ];

  return (
    <section className={styles.howItWorks}>
      <h2 className={styles.heading}>Hur PlanIt fungerar.</h2>
      <p className={styles.sectionTitle}>Combinerar teambuilding och projektledning i en sömlös upplevelse.</p>
      <div className={styles.steps}>
        {steps.map((item, index) => (
          <div key={index} className={styles.card}>
            <div className={styles.stepNumber}>{item.step}</div>
            <h3>{item.title}</h3>
            <p>{item.description}</p>
          </div>
        ))}
      </div>
    </section>
  );
}

export default HowItWorks;