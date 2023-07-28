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
	const [status, setStatus] = useState("4");
	const [comment, setComment] = useState("");
	const [category, setCategory] = useState("4");

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
			};
			try {
				const addedItem: Item = await itemService.addItem(item);
				props.setItems([...props.items, addedItem]);
				props.setShowModal(false);
			} catch (error) {
				console.error("Error adding item:", error);
			}
		}
	};

	return (
		<Modal show={true} onHide={() => props.setShowModal(false)}>
			<Modal.Header closeButton>
				<Modal.Title>Lisää</Modal.Title>
			</Modal.Header>
			<Modal.Body>
				<div className="form-group">
					<label htmlFor="name">Nimi</label>
					<input
						type="text"
						id="name"
						className="form-control"
						value={name}
						onChange={(e) => setName(e.target.value)}
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
						onChange={(e) => setStatus(e.target.value)}
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
						type="text"
						id="comment"
						className="form-control"
						value={comment}
						onChange={(e) => setComment(e.target.value)}
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
						onChange={(e) => setCategory(e.target.value)}
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

export default AddModal;
