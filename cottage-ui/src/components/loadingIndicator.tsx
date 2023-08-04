import React from "react";
import { Spinner } from "react-bootstrap";

const LoadingIndicator: React.FC = () => (
	<div className="container d-flex justify-content-center align-items-center">
		<Spinner animation="border" variant="primary" />
	</div>
);

export default LoadingIndicator;
