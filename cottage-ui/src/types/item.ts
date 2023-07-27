import { Category, Status } from "./enums";

interface Item {
	id: number;
	name: string;
	status: Status;
	comment: string | undefined;
	category: Category;
}

export default Item;
