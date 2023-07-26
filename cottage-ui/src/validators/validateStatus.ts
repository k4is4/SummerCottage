const validateStatus = (status: string) => {
	if (status.trim() === "") {
		return "Status is required";
	} else {
		const statusValue = Number(status);
		if (isNaN(statusValue) || statusValue < 0 || statusValue > 200) {
			return "Status should be a number between 0 and 200";
		} else {
			return "";
		}
	}
};

export default validateStatus;
