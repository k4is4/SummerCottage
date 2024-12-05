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
	private async getToken() {
		const response = await axios.post(
			`https://login.microsoftonline.com/bf05f699-9934-4606-9238-1a93f805568f/oauth2/v2.0/token`,
			null,
			{
				params: {
					grant_type: 'client_credentials',
					client_id: '111726ea-c3c1-4d90-8ec4-f3a2595c3bd0',
					client_secret: process.env.AZURE_CLIENT_SECRET,
					scope: 'api://d76453d7-bc8c-425f-9ee9-bdb7d2d071ce/Invoke',
				},
				headers: {
					'Content-Type': 'application/x-www-form-urlencoded',
				},
			}
		);
		return response.data.access_token;
	}
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
		let token = await this.getToken();
		const backendUrl = 'https://app-cottage.azurewebsites.net/api';
		// const token = await this.getAccessToken();

		try {
			// Make the API request with the token in the Authorization header
			const response: AxiosResponse<Item[]> = await axios.get(
				`${backendUrl}/items`,
				{
					headers: {
						Authorization: `Bearer ${token}`, // Pass the token in the Authorization header
					},
				}
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
