import { Link } from 'react-router-dom';

function Login (props) {

	function login() {
		let formData = new FormData(document.getElementById("login-form"));
		for(let entry of formData.entries()){
			console.log(entry);
		}
	}
	
	return (
		<div className="auth-container">
			<div className="auth-header">
				<span>Login</span>
			</div>
			<form action="#" id="login-form">
				<input type="email" name="login_email" id="login_email" placeholder="Email" autoComplete="off" required />
				<input type="password" name="login_password" id="login_password" placeholder="Password" required />
				<input type="button" value="Submit" onClick={() => login()}/>
			</form>
			<div className="auth-footer">
				<Link to="/signup">Sign Up ?</Link>
			</div>
		</div>
	);
}

export default Login;