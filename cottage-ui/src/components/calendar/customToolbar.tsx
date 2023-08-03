import React, { useState, useEffect } from "react";
import moment from "moment";
import "./customToolbar.css";
import { ToolbarProps } from "react-big-calendar";

const CustomToolbar: React.FC<ToolbarProps> = (props: ToolbarProps) => {
	const date = props.date;
	const onNavigate = props.onNavigate;
	const [month, setMonth] = useState("tammikuu");
	const mMonth = moment(date).format("MMMM");

	useEffect(() => {
		setMonth(mMonth);
	}, [mMonth]);

	const goToBack = () => {
		onNavigate("PREV");
	};
	const goToNext = () => {
		onNavigate("NEXT");
	};

	return (
		<div className="rbc-toolbar">
			<div className="toolbar-container">
				<button onClick={goToBack}>{"<<"}</button>
				<span className="toolbar-label">
					{month} {moment(date).format("YYYY")}
				</span>
				<button onClick={goToNext}>{">>"}</button>
			</div>
		</div>
	);
};

export default CustomToolbar;
