import React, { useState } from "react";
import { Button, Form, Modal } from "react-bootstrap";
import { CalendarEventColor } from "../../types/enums";
import moment from "moment";
import { SlotInfo } from "react-big-calendar";
import CalendarEvent from "../../types/calendarEvent";
import calendarEventService from "../../services/CalendarEventService";
import EventFormData from "../../types/eventFormData";

interface AddModalProps {
	slotInfo: SlotInfo;
	onClose: () => void;
	onSave: (newEvent: CalendarEvent) => void;
	setError: (error: string) => void;
}

const AddModal: React.FC<AddModalProps> = ({
	slotInfo,
	onClose,
	onSave,
	setError,
}) => {
	const [formData, setFormData] = useState<EventFormData>({
		startDate: moment(slotInfo.start).startOf("day").add(12, "hours").toDate(),
		endDate: moment(slotInfo.end).startOf("day").add(12, "hours").toDate(),
		note: "",
		color: CalendarEventColor.white,
	});

	const handleSave = async (): Promise<void> => {
		try {
			const addedEvent: CalendarEvent =
				await calendarEventService.addCalendarEvent(formData);
			onSave(addedEvent);
		} catch (e) {
			console.error("Error adding event:", e);
			setError("Lisäys ei onnistunut");
		}
	};

	return (
		<Modal show={true} onHide={onClose}>
			<Modal.Header closeButton>
				<Modal.Title>Lisää uusi mökkireissu</Modal.Title>
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
						value={moment.utc(formData.endDate).format("YYYY-MM-DD")}
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
				<Button variant="primary" onClick={handleSave}>
					Tallenna
				</Button>
			</Modal.Footer>
		</Modal>
	);
};

export default AddModal;
