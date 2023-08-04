import axios, { AxiosInstance } from "axios";

const apiClient: AxiosInstance = axios.create({
	// baseURL: "https://app-cottage.azurewebsites.net/api",
	baseURL: "https://localhost:7202/api",
	headers: {
		"Content-Type": "application/json",
	},
});

export default apiClient;
