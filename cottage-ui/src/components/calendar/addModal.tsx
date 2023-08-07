import React, { useState } from "react";
import { Button, Modal } from "react-bootstrap";
import { CalendarEventColor } from "../../types/enums";
import moment from "moment";
import { SlotInfo } from "react-big-calendar";
import CalendarEvent from "../../types/calendarEvent";
import calendarEventService from "../../services/CalendarEventService";
import EventFormData from "../../types/eventFormData";
import "../modal.css";

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
		color: CalendarEventColor.green,
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
				<div className="form-group">
					<label htmlFor="startDate">Saapumispäivä</label>
					<input
						id="startDate"
						type="date"
						className="form-control"
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
				</div>
				<div className="form-group">
					<label htmlFor="endDate">Lähtöpäivä</label>
					<input
						id="endDate"
						type="date"
						className="form-control"
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
				</div>
				<div className="form-group">
					<label htmlFor="note">Nimi</label>
					<input
						id="note"
						type="text"
						className="form-control"
						value={formData.note}
						onChange={(e) => setFormData({ ...formData, note: e.target.value })}
					/>
				</div>
				<div className="form-group">
					<label htmlFor="color">Väri</label>
					<select
						id="color"
						className="form-control"
						value={formData.color}
						onChange={(e) =>
							setFormData({
								...formData,
								color: Number(e.target.value) as CalendarEventColor,
							})
						}
					>
						<option value={CalendarEventColor["#BB0C0C"]}>
							Punainen - Varattu
						</option>
						<option value={CalendarEventColor.green}>
							Vihreä - Saa tulla mukaan
						</option>
					</select>
				</div>
			</Modal.Body>
			<Modal.Footer>
				<Button variant="secondary" onClick={onClose} aria-label="Cancel">
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
