import React from 'react';
import styles from './HeroSection.module.css';
import teamImage from "../../assets/team.svg";

const HeroSection = () => {
  return (
    <section className={styles.hero}>
  <div className={styles.contentWrapper}>
    <div className={styles.textContainer}>
      <h1>
        <span>Bygg starkare team.</span> <br />
        <span className={styles.highlight}>Tillsammans.</span>
      </h1>
      <p>Den kompletta plattformen för teambyggande, målspårning och välmående.</p>
      <div className={styles.buttons}>
        <button className={styles.primary}>Prova gratis</button>
        <button className={styles.secondary}>Boka demo</button>
      </div>
    </div>

    <div className={styles.imageContainer}>
      <img src={teamImage} alt="Team working together" />
    </div>
  </div>
</section>
  );
} 
export default HeroSection;