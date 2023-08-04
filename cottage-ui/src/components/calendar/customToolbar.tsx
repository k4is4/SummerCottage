import React from "react";
import moment from "moment";
import "./customToolbar.css";
import { ToolbarProps } from "react-big-calendar";

const CustomToolbar: React.FC<ToolbarProps> = (props: ToolbarProps) => {
	const { date, onNavigate } = props;

	const month = moment(date).format("MMMM");
	const year = moment(date).format("YYYY");

	return (
		<div className="rbc-toolbar">
			<div className="toolbar-container">
				<button onClick={() => onNavigate("PREV")} aria-label="Previous Month">
					{"<<"}
				</button>
				<span className="toolbar-label">
					{month} {year}
				</span>
				<button onClick={() => onNavigate("NEXT")} aria-label="Next Month">
					{">>"}
				</button>
			</div>
		</div>
	);
};

export default CustomToolbar;
