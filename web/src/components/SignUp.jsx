import { useState } from 'react';
import { Link } from 'react-router-dom';

function SignUp (props) {

	const [SignupError, setSignupError] = useState("");

	function showErrorText(error_text, input_element) {
		setSignupError(error_text);
		input_element.classList.add("error_input");

		setTimeout(() => {
			setSignupError("");
			input_element.classList.remove("error_input");
		}, 1500);
	}

	function validate (name, email, password) {
		let emailPattern = /^[.a-zA-Z0-9_]+@[a-z]+(\.[a-z]+)+$/i;
		password = password.trim();
		name = name.trim();

		if(name.length === 0){
			showErrorText("Enter valid name !", document.getElementById("signup_name"));
			return false;
		}

		if(!emailPattern.test(email)) {
			showErrorText("Invalid email !", document.getElementById("signup_email"));
			return false;
		}

		if(password.length < 5){
			showErrorText("Minimum length is 5 !", document.getElementById("signup_password"));
			return false;
		}

		return true;
	}

	function signup () {
		let formData = new FormData(document.getElementById("signup-form"));
		const [name, email, password] = formData.values();
		if(validate(name, email, password)) {
			console.log(email + " " + password);
		}
	}

	return(
		<div className="auth-container">
			<div className="auth-header">
				<span>Sign Up</span>
			</div>
			<form action="#" id="signup-form">
				<input type="text" name="signup_name" id="signup_name" placeholder="Name" autoComplete="off" required />
				<input type="email" name="signup_email" id="signup_email" placeholder="Email" autoComplete="off" required />
				<input type="password" name="signup_password" id="signup_password" placeholder="Password" required />
				{ SignupError !== "" ? <p className="error-text">{SignupError}</p>
					:
					""
				}
				<input type="button" value="Submit" onClick={() => signup()}/>
			</form>
			<div className="auth-footer">
				<Link to="/login">Login ?</Link>
			</div>
		</div>
	);
}

export default SignUp;