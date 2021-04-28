import Auth from './components/Auth';
import './App.css';
import { Route } from 'react-router-dom'

function App() {
  return (
    <div className="App">
      <header>FaCeBoOk</header>
      <div className="single-container">
        <Route path="/" component={ Auth }/>
      </div>
    </div>
  );
}

export default App;
