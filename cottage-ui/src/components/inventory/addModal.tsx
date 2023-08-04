import React, { useState } from "react";
import { Modal, Button } from "react-bootstrap";
import Item from "../../types/item";
import ModalProps from "../../types/modalProps";
import ItemWithoutId from "../../types/itemWithoutId";
import itemService from "../../services/ItemService";
import useValidation from "../../hooks/useValidation";
import { Category, Status } from "../../types/enums";

const AddModal: React.FC<ModalProps> = (props) => {
	const [name, setName] = useState("");
	const [status, setStatus] = useState(Status["?"]);
	const [comment, setComment] = useState("");
	const [category, setCategory] = useState(Category.Muut);

	const { nameError, commentError, formSubmitted, setFormSubmitted } =
		useValidation(
			name,
			comment,
			props.items.map((item) => item.name)
		);

	const handleSave = async () => {
		setFormSubmitted(true);
		if (nameError.length < 1 && commentError.length < 1) {
			const item: ItemWithoutId = {
				name: name,
				status: Number(status),
				comment: comment,
				category: Number(category),
				updatedOn: undefined,
			};
			try {
				const addedItem: Item = await itemService.addItem(item);
				props.setItems([...props.items, addedItem]);
				props.setShowModal(false);
			} catch (e) {
				console.error("Error adding item:", e);
				props.setError("Lisäys ei onnistunut");
			}
		}
	};

	return (
		<Modal
			show={true}
			onHide={() => props.setShowModal(false)}
			aria-labelledby="add-item-modal"
		>
			<Modal.Header closeButton>
				<Modal.Title id="add-item-modal">Lisää</Modal.Title>
			</Modal.Header>
			<Modal.Body>
				<div className="form-group">
					<label htmlFor="name-input">Nimi</label>
					<input
						type="text"
						id="name-input"
						className="form-control"
						value={name}
						onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
							setName(e.target.value)
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
						value={status}
						onChange={(e: React.ChangeEvent<HTMLSelectElement>) =>
							setStatus(Number(e.target.value) as Status)
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
						type="text"
						id="comment-input"
						className="form-control"
						value={comment}
						onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
							setComment(e.target.value)
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
						value={category}
						onChange={(e: React.ChangeEvent<HTMLSelectElement>) =>
							setCategory(Number(e.target.value) as Category)
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
				<Button variant="primary" onClick={handleSave} aria-label="Save">
					Tallenna
				</Button>
			</Modal.Footer>
		</Modal>
	);
};

export default AddModal;
