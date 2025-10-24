import styles from './FeaturesGrid.module.css';
import { Check, Target, Calendar, Users } from 'lucide-react';

function FeaturesGrid() {
  const features = [
    {
      title: 'Check-ins',
      description: 'Stäm av hur teamet mår med regelbundna check-ins.',
      icon: <Check size={24} />,
    },
    {
      title: 'Mål',
      description: 'Sätt upp, följ och fira teamets mål tillsammans.',
      icon: <Target size={24} />,
    },
    {
      title: 'Kalender',
      description: 'Organisera alla uppgifter och deadlines i ett flöde.',
      icon: <Calendar size={24} />,
    },
    {
      title: 'Team',
      description: 'Se teamstruktur och roller på ett överskådligt sätt.',
      icon: <Users size={24} />,
    },
  ];

  return (
    <section className={styles.featuresSection}>
     
      <div className={styles.sectionHeader}>
        <h2 className={styles.sectionTitle}>Everything you need in one place</h2>
        <p className={styles.sectionTitle}>Combinerar teambuilding och projektledning i en sömlös upplevelse.</p>
      </div>

      <div className={styles.grid}>
        {features.map((feature, index) => (
          <div key={index} className={styles.card}>
            <div className={styles.icon}>{feature.icon}</div>
            <h3>{feature.title}</h3>
            <p>{feature.description}</p>
          </div>
        ))}
      </div>
    </section>
  );
}

export default FeaturesGrid;