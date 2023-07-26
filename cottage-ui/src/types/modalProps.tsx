import Item from "./item";

export default interface ModalProps {
	selectedItem: Item | null;
	items: Item[];
	setItems: React.Dispatch<React.SetStateAction<Item[]>>;
	setShowModal: React.Dispatch<React.SetStateAction<boolean>>;
}
