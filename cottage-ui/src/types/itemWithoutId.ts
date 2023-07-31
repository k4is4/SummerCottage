export default interface ItemWithoutId {
	name: string;
	status: number | string;
	comment: string | undefined;
	category: number | string;
	updatedOn: string | undefined;
}
