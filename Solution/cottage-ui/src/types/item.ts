import { Category, Status } from "./enums";

interface Item {
	id: number;
	name: string;
	status: Status;
	comment: string;
	category: Category;
	updatedOn: Date | undefined;
}

export default Item;
