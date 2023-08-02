import React from "react";
import "./banner.css";
import logo from "../pictures/logo.jpg";

const Banner: React.FC = () => {
	return (
		<div className="p-3 text-center logo">
			<img src={logo} alt="CottageLogo" className="img-fluid" />
		</div>
	);
};

export default Banner;
