import React from "react";
import { Status } from "../../types/enums";

interface StatusFilterProps {
	selectedStatus: number | null;
	setSelectedStatus: React.Dispatch<React.SetStateAction<number | null>>;
}

const StatusFilter: React.FC<StatusFilterProps> = ({
	selectedStatus,
	setSelectedStatus,
}) => {
	return (
		<div>
			<label htmlFor="statusFilter"></label>
			<select
				id="statusFilter"
				value={selectedStatus || ""}
				onChange={(e) =>
					setSelectedStatus(
						e.target.value ? parseInt(e.target.value, 10) : null
					)
				}
			>
				<option value="">Kaikki</option>
				<option value={Status.Paljon}>{Status[Status.Paljon]}</option>
				<option value={Status.Löytyy}>{Status[Status.Löytyy]}</option>
				<option value={Status.Lopussa}>{Status[Status.Lopussa]}</option>
				<option value={Status["?"]}>{Status[Status["?"]]}</option>
			</select>
		</div>
	);
};

export default StatusFilter;
