import { AxiosError, AxiosResponse } from "axios";
import Item from "../types/item";
import apiClient from "./apiClient";
import ItemWithoutId from "../types/itemFormData";
import { ProblemDetails } from "../types/problemDetails";

class ItemService {
	async getItems(): Promise<Item[]> {
		return apiClient
			.get<Item[]>("/items")
			.then((response: AxiosResponse<Item[]>) => response.data);
	}

	async addItem(item: ItemWithoutId): Promise<Item> {
		return apiClient
			.post<Item>("/items", null, {
				params: {
					name: item.name,
					status: item.status,
					category: item.category,
					comment: item.comment,
				},
			})
			.then((response: AxiosResponse<Item>) => response.data)
			.catch(this.parseError<Item>);
	}

	async updateItem(item: Item): Promise<Item> {
		return apiClient
			.put<Item>(`/items/${item.id}`, null, {
				params: {
					name: item.name,
					status: item.status,
					category: item.category,
					comment: item.comment,
				},
			})
			.then((response: AxiosResponse<Item>) => response.data)
			.catch(this.parseError<Item>);
	}

	async deleteItem(id: number): Promise<void> {
		return apiClient.delete(`/items/${id}`);
	}

	parseError<T>(errorResponse: AxiosError<ProblemDetails>): Promise<T> {
		const errors = errorResponse.response?.data.errors;
		if (errors) {
			let errorsToDisplay: string = "";
			Object.keys(errors).forEach((key: string) => {
				errorsToDisplay += errors[key] + "\n";
			});
			throw Error(errorsToDisplay);
		} else {
			throw Error(errorResponse.response?.data);
		}
	}
}

const itemService = new ItemService();
export default itemService;
