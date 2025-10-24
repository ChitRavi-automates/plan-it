import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

const LogOut = () => {
  const navigate = useNavigate();

  useEffect(() => {
    // Rensa eventuell inloggningsstatus
    localStorage.removeItem('user'); // eller vad du nu sparar
    // Navigera tillbaka till login
    navigate('/log-in');
  }, [navigate]);

  return null; // Denna komponent visar inget – den bara loggar ut direkt
};

export default LogOut;