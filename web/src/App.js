import Login  from './components/Login';
import SignUp from './components/SignUp';
import './App.css';
import { Route } from 'react-router-dom';
import './styles/Auth.css';

function App() {
  return (
    <div className="App">
      <header>FaCeBoOk</header>
      <div className="single-container">
        <Route path="/login" exact component={Login} />
        <Route path="/signup" exact component={SignUp} />
      </div>
    </div>
  );
}

export default App;
