import React, { useState } from "react";
import { Modal, Button } from "react-bootstrap";
import Item from "../../types/item";
import ModalProps from "../../types/modalProps";
import itemService from "../../services/ItemService";
import useValidation from "../../hooks/useValidation";
import { Category, Status } from "../../types/enums";

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
			props.items
				.filter((item) => item.id !== props.selectedItem?.id)
				.map((item) => item.name)
		);

	const handleSave = async () => {
		if (props.selectedItem) {
			setFormSubmitted(true);
			if (nameError.length < 1 && commentError.length < 1) {
				try {
					editItem(props.selectedItem.id);
					props.setShowModal(false);
				} catch (e) {
					console.error("Error updating item:", e);
					props.setError("Muokkaus ei onnistunut");
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
			updatedOn: props.selectedItem?.updatedOn,
		};

		await itemService.updateItem(updatedItem);
		const updatedItems = props.items.map((item) =>
			item.id === itemId ? updatedItem : item
		);
		props.setItems(updatedItems);
	};

	return (
		<Modal
			show={true}
			onHide={() => props.setShowModal(false)}
			aria-labelledby="edit-item-modal"
		>
			<Modal.Header closeButton>
				<Modal.Title id="edit-item-modal">Muokkaa</Modal.Title>
			</Modal.Header>
			<Modal.Body>
				<div className="form-group">
					<label htmlFor="name-input">Nimi</label>
					<input
						type="text"
						id="name-input"
						className="form-control"
						value={editedName}
						onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
							setEditedName(e.target.value)
						}
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
						onChange={(e: React.ChangeEvent<HTMLSelectElement>) =>
							setEditedStatus(Number(e.target.value) as Status)
						}
					>
						{Object.entries(Status)
							.filter(([key]) => isNaN(Number(key)))
							.map(([key, value]) => (
								<option key={key} value={value}>
									{key}
								</option>
							))}
					</select>
				</div>
				<div className="form-group">
					<label htmlFor="comment-input">Kommentti</label>
					<input
						type="string"
						id="comment-input"
						className="form-control"
						value={editedComment}
						onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
							setEditedComment(e.target.value)
						}
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
						onChange={(e: React.ChangeEvent<HTMLSelectElement>) =>
							setEditedCategory(Number(e.target.value) as Category)
						}
					>
						{Object.entries(Category)
							.filter(([key]) => isNaN(Number(key)))
							.map(([key, value]) => (
								<option key={key} value={value}>
									{key}
								</option>
							))}
					</select>
				</div>
			</Modal.Body>
			<Modal.Footer>
				<Button
					variant="secondary"
					onClick={() => props.setShowModal(false)}
					aria-label="Cancel"
				>
					Peruuta
				</Button>
				<Button
					variant="primary"
					onClick={handleSave}
					aria-label={`Save changes on ${props.selectedItem?.name}`}
				>
					Tallenna
				</Button>
			</Modal.Footer>
		</Modal>
	);
};

export default EditModal;
