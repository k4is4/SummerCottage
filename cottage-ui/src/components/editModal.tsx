import React, { useState } from "react";
import { Modal, Button } from "react-bootstrap";
import Item from "../types/item";
import ModalProps from "../types/modalProps";
import itemService from "../services/ItemService";
import useValidation from "../hooks/useValidation";
import { Category, Status } from "../types/enums";

const EditModal: React.FC<ModalProps> = (props) => {
	const [editedName, setEditedName] = useState(props.selectedItem?.name ?? "");
	const [editedStatus, setEditedStatus] = useState(
		props.selectedItem?.status ?? 0
	);
	const [editedComment, setEditedComment] = useState(
		props.selectedItem?.comment ?? ""
	);
	const [editedCategory, setEditedCategory] = useState(
		props.selectedItem?.category ?? 0
	);

	const { nameError, commentError, formSubmitted, setFormSubmitted } =
		useValidation(
			editedName,
			editedComment,
			props.items.map((item) => item.name)
		);

	const handleSave = async () => {
		if (props.selectedItem) {
			setFormSubmitted(true);
			if (nameError.length < 1 && commentError.length < 1) {
				try {
					editItem(props.selectedItem.id);
					props.setShowModal(false);
				} catch (error) {
					console.error("Error updating item:", error);
				}
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
					{nameError && formSubmitted && (
						<span className="text-danger">{nameError}</span>
					)}
				</div>
				<div className="form-group">
					<label htmlFor="status">Jäljellä</label>
					<select
						id="status"
						className="form-control"
						value={editedStatus}
						onChange={(e) => setEditedStatus(Number(e.target.value))}
					>
						<option value="1">{Status[1]}</option>
						<option value="2">{Status[2]}</option>
						<option value="3">{Status[3]}</option>
						<option value="4">{Status[4]}</option>
					</select>
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
					{commentError && formSubmitted && (
						<span className="text-danger">{commentError}</span>
					)}
				</div>
				<div className="form-group">
					<label htmlFor="category">Kategoria</label>
					<select
						id="category"
						className="form-control"
						value={editedCategory}
						onChange={(e) => setEditedCategory(Number(e.target.value))}
					>
						<option value="1">{Category[1]}</option>
						<option value="2">{Category[2]}</option>
						<option value="3">{Category[3]}</option>
						<option value="4">{Category[4]}</option>
					</select>
				</div>
			</Modal.Body>
			<Modal.Footer>
				<Button variant="secondary" onClick={() => props.setShowModal(false)}>
					Peruuta
				</Button>
				<Button variant="primary" onClick={handleSave}>
					Tallenna
				</Button>
			</Modal.Footer>
		</Modal>
	);
};

export default EditModal;
