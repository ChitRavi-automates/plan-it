import "../index.css";
import React from 'react';
import HeroSection from '../components/homePage/HeroSection';
import WhyChoose from '../components/homePage/WhyChoose';
import FeaturesGrid from '../components/homePage/FeaturesGrid';
import HowItWorks from '../components/homePage/HowItWorks';
import CallToAction from '../components/homePage/CallToAction';


      
    const Home = () => {
      return (
        <div>
          <HeroSection />
          {/* Här kommer nästa sektioner senare */}
          <WhyChoose />
          <FeaturesGrid />
          {<HowItWorks /> }
          {<CallToAction />}
        </div>
      );
    };
    
    export default Home;
    