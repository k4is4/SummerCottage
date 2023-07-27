const validateComment = (comment: string) => {
	if (comment.length > 100) {
		return "Kommentti voi olla enintään 100 merkkiä";
	} else {
		return "";
	}
};

export default validateComment;
