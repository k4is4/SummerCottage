import React from "react";
import { Category } from "../../types/enums";

interface CategoryFilterProps {
	selectedCategory: number | null;
	setSelectedCategory: React.Dispatch<React.SetStateAction<number | null>>;
}

const CategoryFilter: React.FC<CategoryFilterProps> = ({
	selectedCategory,
	setSelectedCategory,
}) => {
	return (
		<div>
			<label htmlFor="categoryFilter"></label>
			<select
				id="categoryFilter"
				value={selectedCategory || ""}
				onChange={(e) =>
					setSelectedCategory(
						e.target.value ? parseInt(e.target.value, 10) : null
					)
				}
			>
				<option value="">Kaikki</option>
				<option value={Category.Kuivaruoka}>
					{Category[Category.Kuivaruoka]}
				</option>
				<option value={Category.Jääkaappi}>
					{Category[Category.Jääkaappi]}
				</option>
				<option value={Category.Juomat}>{Category[Category.Juomat]}</option>
				<option value={Category.Muut}>{Category[Category.Muut]}</option>
			</select>
		</div>
	);
};

export default CategoryFilter;
