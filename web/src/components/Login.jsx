import { Link } from 'react-router-dom';

function Login (props) {
	return (
		<div className="auth-container">
			<div className="auth-header">
				<span>Login</span>
			</div>
			<form action="#" method="get">
				<input type="email" name="login_email" id="login_email" placeholder="Email" autoComplete="off" required />
				<input type="password" name="login_password" id="login_password" placeholder="Password" required />
				<input type="submit" value="Submit"/>
			</form>
			<div className="auth-footer">
				<Link to="/signup">Sign Up ?</Link>
			</div>
		</div>
	);
}

export default Login;