import React from "react";
import { Modal, Button } from "react-bootstrap";
import ModalProps from "../../types/modalProps";
import itemService from "../../services/ItemService";

const DeleteModal: React.FC<ModalProps> = (props) => {
	const handleConfirmDelete = async () => {
		if (props.selectedItem) {
			try {
				deleteItem(props.selectedItem.id);
				props.setShowModal(false);
			} catch (e) {
				console.error("Error deleting item:", e);
				props.setError("Poistaminen ei onnistunut");
			}
		}
	};

	const deleteItem = async (itemId: number) => {
		await itemService.deleteItem(itemId);
		const updatedItems = props.items.filter((item) => item.id !== itemId);
		props.setItems(updatedItems);
	};

	return (
		<div>
			<Modal show={true} onHide={() => props.setShowModal(false)}>
				<Modal.Header closeButton>
					<Modal.Title>Poista</Modal.Title>
				</Modal.Header>
				<Modal.Body>
					<p>Haluatko varmasti poistaa {props.selectedItem?.name}?</p>
				</Modal.Body>
				<Modal.Footer>
					<Button variant="secondary" onClick={() => props.setShowModal(false)}>
						Peruuta
					</Button>
					<Button variant="danger" onClick={handleConfirmDelete}>
						Poista
					</Button>
				</Modal.Footer>
			</Modal>
		</div>
	);
};

export default DeleteModal;
