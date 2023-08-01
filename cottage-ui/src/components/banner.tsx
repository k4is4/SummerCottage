import React from "react";
import "./banner.css";
import logo from "../pictures/logo.jpg";

const Banner: React.FC = () => {
	return (
		<div className="p-3 text-center">
			<img src={logo} alt="CottageLogo" />
		</div>
	);
};

export default Banner;
