import { Link } from 'react-router-dom';

function SignUp (props) {

	function signup () {
		let formData = new FormData(document.getElementById("signup-form"));
		for(let entry of formData.entries()){
			console.log(entry);
		}
	}

	return(
		<div className="auth-container">
			<div className="auth-header">
				<span>Sign Up</span>
			</div>
			<form action="#" id="signup-form">
				<input type="text" name="signup_name" id="signup_name" placeholder="User name" autoComplete="off" required />
				<input type="email" name="signup_email" id="signup_email" placeholder="Email" autoComplete="off" required />
				<input type="password" name="signup_password" id="signup_password" placeholder="Password" required />
				<input type="button" value="Submit" onClick={() => signup()}/>
			</form>
			<div className="auth-footer">
				<Link to="/login">Login ?</Link>
			</div>
		</div>
	);
}

export default SignUp;