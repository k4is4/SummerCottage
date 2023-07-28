import React, { useState, useEffect } from "react";
import { Button, Form, Modal } from "react-bootstrap";
import { CalendarEventColor } from "../../types/enums";
import moment from "moment";
import CalendarEvent from "../../types/calendarEvent";
import calendarEventService from "../../services/CalendarEventService";

interface EditModalProps {
	event: CalendarEvent;
	onClose: () => void;
	onSave: (oldEvent: CalendarEvent, newEvent: CalendarEvent) => void;
	onDelete: (event: CalendarEvent) => void;
}

const EditModal: React.FC<EditModalProps> = ({
	event,
	onClose,
	onSave,
	onDelete,
}) => {
	const [startDate, setStartDate] = useState(event.startDate);
	const [endDate, setEndDate] = useState(event.endDate);
	const [note, setNote] = useState(event.note);
	const [color, setColor] = useState(event.color);

	useEffect(() => {
		setStartDate(event.startDate);
		setEndDate(event.endDate);
		setNote(event.note);
		setColor(event.color);
	}, [event]);

	const handleSave = async () => {
		const updateData: CalendarEvent = {
			id: event.id,
			startDate: startDate,
			endDate: endDate,
			note: note,
			color: color,
		};
		const updatedEvent: CalendarEvent =
			await calendarEventService.updateCalendarEvent(updateData);

		onSave(event, updatedEvent);
	};

	const handleDelete = async () => {
		await calendarEventService.deleteCalendarEvent(event.id);
		onDelete(event);
	};

	return (
		<Modal show={true} onHide={onClose}>
			<Modal.Header closeButton>
				<Modal.Title>Edit Calendar Event</Modal.Title>
			</Modal.Header>
			<Modal.Body>
				<Form.Group>
					<Form.Label>Saapumispäivä</Form.Label>
					<Form.Control
						type="date"
						value={moment(startDate).format("YYYY-MM-DD")}
						onChange={(e) => setStartDate(new Date(e.target.value))}
					/>
				</Form.Group>
				<Form.Group>
					<Form.Label>Lähtöpäivä</Form.Label>
					<Form.Control
						type="date"
						value={moment(endDate).format("YYYY-MM-DD")}
						onChange={(e) => setEndDate(new Date(e.target.value))}
					/>
				</Form.Group>
				<Form.Group>
					<Form.Label>Nimi</Form.Label>
					<Form.Control
						type="text"
						value={note}
						onChange={(e) => setNote(e.target.value)}
					/>
				</Form.Group>
				<Form.Group>
					<Form.Label>Väri</Form.Label>
					<Form.Control
						as="select"
						value={color}
						onChange={(e) =>
							setColor(Number(e.target.value) as CalendarEventColor)
						}
					>
						<option value={CalendarEventColor.white}>Tyhjä</option>
						<option value={CalendarEventColor.orange}>Oranssi</option>
						<option value={CalendarEventColor.green}>Vihreä</option>
					</Form.Control>
				</Form.Group>
			</Modal.Body>
			<Modal.Footer>
				<Button variant="secondary" onClick={onClose}>
					Peruuta
				</Button>
				<Button variant="danger" onClick={handleDelete}>
					Poista
				</Button>
				<Button variant="primary" onClick={handleSave}>
					Tallenna
				</Button>
			</Modal.Footer>
		</Modal>
	);
};

export default EditModal;
