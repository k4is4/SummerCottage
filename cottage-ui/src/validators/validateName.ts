const validateName = (name: string) => {
	if (name.trim() === "") {
		return "Name is required";
	} else if (name.length < 2 || name.length > 20) {
		return "Name should be between 2 and 20 characters";
	} else {
		return "";
	}
};

export default validateName;
