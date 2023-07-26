import React from "react";
import { Link, useMatch } from "react-router-dom";

const Navigation: React.FC = () => {
	const isActive = useMatch("/");
	const isRequestActive = useMatch("/request");

	return (
		<nav className="navbar navbar-expand-lg navbar-dark">
			<div className="container">
				<ul className="nav nav-tabs justify-content-center">
					<li className="nav-item">
						<Link
							className={`nav-link ${isActive ? "active" : ""}`}
							to="/"
							aria-current={isActive ? "page" : undefined}
						>
							Inventaario
						</Link>
					</li>
					<li className="nav-item">
						<Link
							className={`nav-link ${isRequestActive ? "active" : ""}`}
							to="/request"
							aria-current={isRequestActive ? "page" : undefined}
						>
							Kalenteri
						</Link>
					</li>
				</ul>
			</div>
		</nav>
	);
};

export default Navigation;
