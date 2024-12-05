import axios, { AxiosError, AxiosResponse } from 'axios';
import Item from '../types/item';
import apiClient from './apiClient';
import ItemFormData from '../types/itemFormData';
import { ProblemDetails } from '../types/problemDetails';

// import {
// 	PublicClientApplication,
// 	InteractionRequiredAuthError,
// } from '@azure/msal-browser'; // MSAL import for token management
// import { msalConfig, loginRequest } from '../msalConfig'; // Import your msalConfig

// const msalInstance = new PublicClientApplication(msalConfig);

class ItemService {
	// private async getAccessToken(): Promise<string> {
	// try {
	// 		const accounts = msalInstance.getAllAccounts();
	// 		if (accounts.length === 0) {
	// 			throw new Error('No accounts.');
	// 		}

	// 		// Try to get the token silently (without re-login)
	// 		const accessTokenResponse = await msalInstance.acquireTokenSilent({
	// 			...loginRequest,
	// 			account: accounts[0], // Get the current authenticated user
	// 		});

	// 		return accessTokenResponse.accessToken;
	// 	} catch (error) {
	// 		// If token acquisition fails (e.g., token expired), try interactive flow
	// 		if (error instanceof InteractionRequiredAuthError) {
	// 			const interactiveResponse = await msalInstance.acquireTokenPopup(
	// 				loginRequest
	// 			);
	// 			return interactiveResponse.accessToken;
	// 		} else {
	// 			console.error('Error acquiring token:', error);
	// 			throw error;
	// 		}
	// 	}
	// }

	public async getItems(): Promise<Item[]> {
		const backendUrl = 'https://app-cottage.azurewebsites.net/api';
		// const token = await this.getAccessToken();

		try {
			// Make the API request with the token in the Authorization header
			const response: AxiosResponse<Item[]> = await axios.get(
				`${backendUrl}/items`
				// {
				// 	headers: {
				// 		Authorization: `Bearer ${token}`, // Pass the token in the Authorization header
				// 	},
				// }
			);
			return response.data;
		} catch (error) {
			console.error('Error fetching items:', error);
			throw error;
		}
	}

	async addItem(item: ItemFormData): Promise<Item> {
		return apiClient
			.post<Item>('/items', item)
			.then((response: AxiosResponse<Item>) => response.data)
			.catch(this.parseError<Item>);
	}

	async updateItem(item: Item): Promise<Item> {
		return apiClient
			.put<Item>(`/items/${item.id}`, item)
			.then((response: AxiosResponse<Item>) => response.data)
			.catch(this.parseError<Item>);
	}

	async deleteItem(id: number): Promise<void> {
		return apiClient.delete(`/items/${id}`);
	}

	parseError<T>(errorResponse: AxiosError<ProblemDetails>): Promise<T> {
		const errors = errorResponse.response?.data.errors;
		if (errors) {
			let errorsToDisplay: string = '';
			Object.keys(errors).forEach((key: string) => {
				errorsToDisplay += errors[key] + '\n';
			});
			throw Error(errorsToDisplay);
		} else {
			throw Error(errorResponse.response?.data);
		}
	}
}

const itemService = new ItemService();
export default itemService;
