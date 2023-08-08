import { Category, Status } from "./enums";

export default interface ItemFormData {
	name: string;
	status: Status;
	comment: string;
	category: Category;
}
