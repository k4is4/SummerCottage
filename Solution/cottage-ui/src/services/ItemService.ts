import axios, { AxiosError, AxiosResponse } from 'axios';
import Item from '../types/item';
import apiClient from './apiClient';
import ItemFormData from '../types/itemFormData';
import { ProblemDetails } from '../types/problemDetails';
// import msalInstance from '../msalConfig';

class ItemService {
	// private async getAccessToken(): Promise<string> {
	// 	try {
	// 		const accounts = msalInstance.getAllAccounts();
	// 		if (accounts.length === 0) {
	// 			throw new Error('No accounts found. Please log in.');
	// 		}

	// 		const request = {
	// 			scopes: ['api://d76453d7-bc8c-425f-9ee9-bdb7d2d071ce/Invoke'], // Replace with your API's scope
	// 			account: accounts[0], // Use the first account (or let the user pick one)
	// 		};

	// 		const response = await msalInstance.acquireTokenSilent(request);
	// 		return response.accessToken;
	// 	} catch (error) {
	// 		console.error('Error acquiring token silently:', error);
	// 		throw error;
	// 	}
	// }

	public async getItems(): Promise<Item[]> {
		const backendUrl = 'https://app-kaisa-backend.azurewebsites.net/api';
		// const accessToken = await this.getAccessToken();

		try {
			// Make the API request with the token in the Authorization header
			const response: AxiosResponse<Item[]> = await axios.get(
				`${backendUrl}/items`
				// {
				// 	headers: {
				// 		Authorization: `Bearer ${accessToken}`,
				// 		Accept: 'application/json',
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
