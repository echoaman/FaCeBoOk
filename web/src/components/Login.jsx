import { useState } from 'react';
import { Link } from 'react-router-dom';

function Login (props) {

	const [LoginError, setLoginError] = useState("");

	function showErrorText(error_text, input_element) {
		setLoginError(error_text);
		input_element.classList.add("error_input");

		setTimeout(() => {
			setLoginError("");
			input_element.classList.remove("error_input");
		}, 1500);
	}

	function validate (email, password) {
		let emailPattern = /^[.a-zA-Z0-9_]+@[a-z]+(\.[a-z]+)+$/i;
		password = password.trim();

		if(!emailPattern.test(email)) {
			showErrorText("Invalid email !", document.getElementById("login_email"));
			return false;
		}

		if(password.length < 5){
			showErrorText("Minimum length is 5", document.getElementById("login_password"));
			return false;
		}

		return true;
	}
	
	function login() {
		let formData = new FormData(document.getElementById("login-form"));
		const [email, password] = formData.values();
		if(validate(email, password)) {
			console.log(email + " " + password);
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
				{ LoginError !== "" ? <p className="error-text">{LoginError}</p>
					:
					""
				}
				<input type="button" value="Submit" onClick={() => login()}/>
			</form>
			<div className="auth-footer">
				<Link to="/signup">Sign Up ?</Link>
			</div>
		</div>
	);
}

export default Login;