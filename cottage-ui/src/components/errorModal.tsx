import { useState } from "react";
import { Modal, Button } from "react-bootstrap";

interface ErrorHandlerProps {
	initialMessage?: string;
	onClose: () => void;
}

const ErrorModal: React.FC<ErrorHandlerProps> = ({
	initialMessage,
	onClose,
}) => {
	const [errorMessage, setErrorMessage] = useState<string | null>(
		initialMessage ?? null
	);
	const [show, setShow] = useState<boolean>(Boolean(initialMessage));

	const closeHandler = () => {
		setErrorMessage(null);
		setShow(false);
		onClose();
	};

	return (
		<Modal show={show} onHide={closeHandler}>
			<Modal.Header closeButton>
				<Modal.Title>Oho! Jotain meni vikaan</Modal.Title>
			</Modal.Header>
			<Modal.Body>{errorMessage}</Modal.Body>
			<Modal.Footer>
				<Button variant="secondary" onClick={closeHandler}>
					Sulje
				</Button>
			</Modal.Footer>
		</Modal>
	);
};

export default ErrorModal;
