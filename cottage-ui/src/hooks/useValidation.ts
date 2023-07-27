import { useEffect, useState } from "react";
import validateName from "../validators/validateName";
import validateComment from "../validators/validateComment";

const useValidation = (
	name: string,
	comment: string,
	existingNames: string[]
) => {
	const [nameError, setNameError] = useState("");
	const [commentError, setCommentError] = useState("");
	const [formSubmitted, setFormSubmitted] = useState(false);

	useEffect(() => {
		const existingNamesLowercase = existingNames.map((item) =>
			item.toLowerCase()
		);

		setNameError(validateName(name, existingNamesLowercase));
		setCommentError(validateComment(comment));
	}, [name, comment, existingNames]);

	return { nameError, commentError, formSubmitted, setFormSubmitted };
};

export default useValidation;
