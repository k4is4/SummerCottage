import React, { useState } from "react";
import { Modal, Button } from "react-bootstrap";
import Item from "../types/item";
import ModalProps from "../types/modalProps";
import itemService from "../services/ItemService";

const EditModal: React.FC<ModalProps> = (props) => {
	const [editedName, setEditedName] = useState(props.selectedItem?.name ?? "");
	const [editedStatus, setEditedStatus] = useState(
		props.selectedItem?.status ?? 0
	);
	const [editedComment, setEditedComment] = useState(
		props.selectedItem?.comment
	);
	const [editedCategory, setEditedCategory] = useState(
		props.selectedItem?.category ?? 0
	);

	const handleSave = async () => {
		if (props.selectedItem) {
			try {
				editItem(props.selectedItem.id);
				props.setShowModal(false);
			} catch (error) {
				console.error("Error updating item:", error);
			}
		}
	};

	const editItem = async (itemId: number) => {
		const updatedItem: Item = {
			id: itemId,
			name: editedName,
			status: editedStatus,
			comment: editedComment,
			category: editedCategory,
		};

		await itemService.updateItem(updatedItem);
		const updatedItems = props.items.map((item) =>
			item.id === itemId ? updatedItem : item
		);
		props.setItems(updatedItems);
	};

	return (
		<Modal show={true} onHide={() => props.setShowModal(false)}>
			<Modal.Header closeButton>
				<Modal.Title>Muokkaa</Modal.Title>
			</Modal.Header>
			<Modal.Body>
				<div className="form-group">
					<label htmlFor="name">Nimi</label>
					<input
						type="text"
						id="name"
						className="form-control"
						value={editedName}
						onChange={(e) => setEditedName(e.target.value)}
					/>
				</div>
				<div className="form-group">
					<label htmlFor="status">Jäljellä</label>
					<input
						type="number"
						id="status"
						className="form-control"
						value={editedStatus}
						onChange={(e) => setEditedStatus(Number(e.target.value))}
					/>
				</div>
				<div className="form-group">
					<label htmlFor="comment">Kommentti</label>
					<input
						type="string"
						id="comment"
						className="form-control"
						value={editedComment}
						onChange={(e) => setEditedComment(e.target.value)}
					/>
				</div>
				<div className="form-group">
					<label htmlFor="category">Kategoria</label>
					<input
						type="number"
						id="category"
						className="form-control"
						value={editedCategory}
						onChange={(e) => setEditedCategory(Number(e.target.value))}
					/>
				</div>
			</Modal.Body>
			<Modal.Footer>
				<Button variant="secondary" onClick={() => props.setShowModal(false)}>
					Cancel
				</Button>
				<Button variant="primary" onClick={handleSave}>
					Save
				</Button>
			</Modal.Footer>
		</Modal>
	);
};

export default EditModal;
