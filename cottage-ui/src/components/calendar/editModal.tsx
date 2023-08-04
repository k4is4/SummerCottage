import React, { useState, useEffect } from "react";
import { Button, Form, Modal } from "react-bootstrap";
import { CalendarEventColor } from "../../types/enums";
import moment from "moment";
import CalendarEvent from "../../types/calendarEvent";
import calendarEventService from "../../services/CalendarEventService";
import EventFormData from "../../types/eventFormData";

interface EditModalProps {
	event: CalendarEvent;
	onClose: () => void;
	onSave: (oldEvent: CalendarEvent, newEvent: CalendarEvent) => void;
	onDelete: (event: CalendarEvent) => void;
	setError: (error: string) => void;
}

const EditModal: React.FC<EditModalProps> = ({
	event,
	onClose,
	onSave,
	onDelete,
	setError,
}) => {
	const [formData, setFormData] = useState<EventFormData>({
		startDate: moment(event.startDate).startOf("day").add(12, "hours").toDate(),
		endDate: moment(event.endDate).startOf("day").add(12, "hours").toDate(),
		note: event.note,
		color: event.color,
	});

	useEffect(() => {
		setFormData({
			startDate: moment(event.startDate)
				.startOf("day")
				.add(12, "hours")
				.toDate(),
			endDate: moment(event.endDate).startOf("day").add(12, "hours").toDate(),
			note: event.note,
			color: event.color,
		});
	}, [event]);

	const handleSave = async (): Promise<void> => {
		try {
			const updateData: CalendarEvent = { ...event, ...formData };
			const updatedEvent: CalendarEvent =
				await calendarEventService.updateCalendarEvent(updateData);
			onSave(event, updatedEvent);
		} catch (e) {
			console.error("Error updating event:", e);
			setError("Muokkaus ei onnistunut");
		}
	};

	const handleDelete = async (): Promise<void> => {
		try {
			await calendarEventService.deleteCalendarEvent(event.id);
			onDelete(event);
		} catch (e) {
			console.error("Error deleting event:", e);
			setError("Poisto ei onnistunut");
		}
	};

	return (
		<Modal show={true} onHide={onClose}>
			<Modal.Header closeButton>
				<Modal.Title>Muokkaa</Modal.Title>
			</Modal.Header>
			<Modal.Body>
				<Form.Group>
					<Form.Label htmlFor="startDate">Saapumispäivä</Form.Label>
					<Form.Control
						id="startDate"
						type="date"
						value={moment(formData.startDate).format("YYYY-MM-DD")}
						onChange={(e) =>
							setFormData({
								...formData,
								startDate: moment(e.target.value)
									.startOf("day")
									.add(12, "hours")
									.toDate(),
							})
						}
					/>
				</Form.Group>
				<Form.Group>
					<Form.Label htmlFor="endDate">Lähtöpäivä</Form.Label>
					<Form.Control
						id="endDate"
						type="date"
						value={moment(formData.endDate).format("YYYY-MM-DD")}
						onChange={(e) =>
							setFormData({
								...formData,
								endDate: moment(e.target.value)
									.startOf("day")
									.add(12, "hours")
									.toDate(),
							})
						}
					/>
				</Form.Group>
				<Form.Group>
					<Form.Label htmlFor="note">Nimi</Form.Label>
					<Form.Control
						id="note"
						type="text"
						value={formData.note}
						onChange={(e) => setFormData({ ...formData, note: e.target.value })}
					/>
				</Form.Group>
				<Form.Group>
					<Form.Label htmlFor="color">Väri</Form.Label>
					<Form.Control
						id="color"
						as="select"
						value={formData.color}
						onChange={(e) =>
							setFormData({
								...formData,
								color: Number(e.target.value) as CalendarEventColor,
							})
						}
					>
						<option value={CalendarEventColor.white}>Tyhjä</option>
						<option value={CalendarEventColor["#BB0C0C"]}>
							Punainen - Varattu
						</option>
						<option value={CalendarEventColor.green}>
							Vihreä - Saa tulla mukaan
						</option>
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
