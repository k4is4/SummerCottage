import React, { useState } from "react";
import { Button, Form, Modal } from "react-bootstrap";
import { CalendarEventColor } from "../../types/enums";
import moment from "moment";
import { SlotInfo } from "react-big-calendar";
import CalendarEvent from "../../types/calendarEvent";
import calendarEventService from "../../services/CalendarEventService";
import CalendarEventWithoutId from "../../types/calendarEventWithoutId";

interface AddModalProps {
	slotInfo: SlotInfo;
	onClose: () => void;
	onSave: (newEvent: CalendarEvent) => void;
}

const AddModal: React.FC<AddModalProps> = ({ slotInfo, onClose, onSave }) => {
	const [startDate, setStartDate] = useState(
		moment(slotInfo.start).startOf("day").add(12, "hours").toDate()
	);
	const [endDate, setEndDate] = useState(
		moment(slotInfo.end).startOf("day").add(12, "hours").toDate()
	);
	const [note, setNote] = useState("");
	const [color, setColor] = useState(CalendarEventColor.white);

	const handleSave = async () => {
		const eventToAdd: CalendarEventWithoutId = {
			startDate: startDate,
			endDate: endDate,
			note: note,
			color: color,
			updatedOn: undefined,
		};
		const addedEvent: CalendarEvent =
			await calendarEventService.addCalendarEvent(eventToAdd);

		onSave(addedEvent);
	};

	return (
		<Modal show={true} onHide={onClose}>
			<Modal.Header closeButton>
				<Modal.Title>Lisää uusi mökkireissu</Modal.Title>
			</Modal.Header>
			<Modal.Body>
				<Form.Group>
					<Form.Label>Saapumispäivä</Form.Label>
					<Form.Control
						type="date"
						value={moment(startDate).format("YYYY-MM-DD")}
						onChange={(e) =>
							setStartDate(
								moment(e.target.value).startOf("day").add(12, "hours").toDate()
							)
						}
					/>
				</Form.Group>
				<Form.Group>
					<Form.Label>Lähtöpäivä</Form.Label>
					<Form.Control
						type="date"
						value={moment.utc(endDate).format("YYYY-MM-DD")}
						onChange={(e) =>
							setEndDate(
								moment(e.target.value).startOf("day").add(12, "hours").toDate()
							)
						}
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
				<Button variant="primary" onClick={handleSave}>
					Tallenna
				</Button>
			</Modal.Footer>
		</Modal>
	);
};

export default AddModal;
