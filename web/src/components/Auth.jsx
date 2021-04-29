import '../styles/Auth.css'
import Login  from './Login';
import SignUp from './SignUp';
import { Route } from 'react-router-dom'

function Auth (props) {	
	return (
		<div>
			<Route path="/login" exact component={Login} />
			<Route path="/signup" exact component={SignUp} />
		</div>
	);
}

export default Auth;