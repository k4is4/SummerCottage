import axios, { AxiosError, AxiosResponse } from 'axios';
import Item from '../types/item';
import apiClient from './apiClient';
import ItemFormData from '../types/itemFormData';
import { ProblemDetails } from '../types/problemDetails';

class ItemService {
	// async getItems(): Promise<Item[]> {
	// 	return apiClient
	// 		.get<Item[]>("/items")
	// 		.then((response: AxiosResponse<Item[]>) => response.data);
	// }

	async getItems(token: any): Promise<Item[]> {
		const backendUrl = 'https://app-cottage.azurewebsites.net/api';
		// return axios
		// 	.get<Item[]>(`${backendUrl}/items`)
		// 	.then((response: AxiosResponse<Item[]>) => response.data);
		try {
			// Make the API request with the token in the Authorization header
			return axios
				.get<Item[]>(`${backendUrl}/items`, {
					headers: {
						Authorization: `Bearer ${token}`,
						Accept: 'application/json',
					},
				})
				.then((response: AxiosResponse<Item[]>) => response.data);
		} catch (error) {
			console.error('Error acquiring token or fetching items:', error);
			throw error; // Propagate the error to the caller
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
