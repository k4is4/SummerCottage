import React, { useEffect, useState } from "react";
import { Modal, Button } from "react-bootstrap";
import Item from "../types/item";
import ModalProps from "../types/modalProps";
import ItemWithoutId from "../types/itemWithoutId";
import validateName from "../validators/validateName";
import validateStatus from "../validators/validateStatus";
import itemService from "../services/ItemService";

const AddModal: React.FC<ModalProps> = (props) => {
	const [name, setName] = useState("");
	const [status, setStatus] = useState("");
	const [comment, setComment] = useState("");
	const [category, setCategory] = useState("");
	const [nameError, setNameError] = useState("");
	const [statusError, setStatusError] = useState("");
	const [formSubmitted, setFormSubmitted] = useState(false);

	useEffect(() => {
		setNameError(validateName(name));
		setStatusError(validateStatus(status));
	}, [name, status]);

	const handleSave = async () => {
		setFormSubmitted(true);
		if (nameError.length < 1 && statusError.length < 1) {
			const item: ItemWithoutId = {
				name: name,
				status: Number(status),
				comment: comment,
				category: category,
			};
			try {
				const addedItem: Item = await itemService.addItem(item);
				props.setItems([...props.items, addedItem]);
				props.setShowModal(false);
			} catch (error) {
				console.error("Error updating item:", error);
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
					<input
						type="number"
						id="status"
						className="form-control"
						value={status}
						onChange={(e) => setStatus(e.target.value)}
					/>
					{statusError && formSubmitted && (
						<span className="text-danger">{statusError}</span>
					)}
				</div>
				<div className="form-group">
					<label htmlFor="comment">Kommentti</label>
					<input
						type="number"
						id="comment"
						className="form-control"
						value={comment}
						onChange={(e) => setComment(e.target.value)}
					/>
				</div>
				<div className="form-group">
					<label htmlFor="category">Kategoria</label>
					<input
						type="number"
						id="category"
						className="form-control"
						value={category}
						onChange={(e) => setCategory(e.target.value)}
					/>
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
