import { useState } from 'react';
import '../styles/Auth.css'

function Auth (props) {

	function Login () {
		return (
			<div className="auth-container">
				<div className="auth-header">
					<span>Login</span>
				</div>
				<form action="#">
					<input type="email" name="login_email" id="login_email" placeholder="Email" autoComplete="none" required />
					<input type="password" name="login_password" id="login_password" placeholder="Password" required />
					<input type="submit" value="Submit"/>
				</form>
				<div className="auth-footer">
					<span onClick={() => SetLoginShown("none")}>Sign Up ?</span>
				</div>
			</div>
		);
	}
	
	function SignUp () {
		return(
			<div className="auth-container">
				<div className="auth-header">
					<span>Sign Up</span>
				</div>
				<form action="#">
					<input type="text" name="signup_name" id="signup_name" placeholder="User name" autoComplete="none" required />
					<input type="email" name="signup_email" id="signup_email" placeholder="Email" autoComplete="none" required />
					<input type="password" name="signup_password" id="signup_password" placeholder="Password" required />
					<input type="submit" value="Submit"/>
				</form>
				<div className="auth-footer">
					<span onClick={() => SetLoginShown("block")}>Login ?</span>
				</div>
			</div>
		);
	}
	
	const [LoginShown, SetLoginShown] = useState("block")

	return (
		<div>
			{ LoginShown === "block" ? 
				<Login />
			: 
				<SignUp />
			}
		</div>
	);
}

export default Auth;