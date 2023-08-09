import axios, { AxiosInstance } from "axios";

const apiClient: AxiosInstance = axios.create({
	baseURL: process.env.REACT_APP_API_BASE_URL,
	headers: {
		"Content-Type": "application/json",
	},
});

export default apiClient;
