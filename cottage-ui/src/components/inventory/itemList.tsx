import React, { useEffect, useState } from "react";
import Item from "../../types/item";
import EditModal from "./editModal";
import DeleteModal from "./deleteModal";
import { Button } from "react-bootstrap";
import AddModal from "./addModal";
import "./itemList.css";
import itemService from "../../services/ItemService";
import { Spinner } from "react-bootstrap";
import StatusFilter from "./statusFilter";
import CategoryFilter from "./categoryFilter";
import ItemRow from "./itemRow";

const ItemList: React.FC = () => {
	const [items, setItems] = useState<Item[]>([]);
	const [selectedItem, setSelectedItem] = useState<Item | null>(null);
	const [showAddModal, setShowAddModal] = useState(false);
	const [showEditModal, setShowEditModal] = useState(false);
	const [showDeleteModal, setShowDeleteModal] = useState(false);
	const [selectedCategory, setSelectedCategory] = useState<number | null>(null);
	const [selectedStatus, setSelectedStatus] = useState<number | null>(null);
	const [isLoading, setIsLoading] = useState(false);
	const sortedItems = [...items].sort((a, b) => a.name.localeCompare(b.name));

	useEffect(() => {
		fetchData();
	}, []);

	const fetchData = async () => {
		setIsLoading(true);
		try {
			const itemsFromApi = await itemService.getItems();
			setItems(itemsFromApi);
		} catch (error) {
			console.error("Error fetching items:", error);
		} finally {
			setIsLoading(false);
		}
	};

	const handleEdit = (item: Item) => {
		setSelectedItem(item);
		setShowEditModal(true);
	};

	const handleDelete = (item: Item) => {
		setSelectedItem(item);
		setShowDeleteModal(true);
	};

	const handleStatusUpdate = async (item: Item) => {
		try {
			const numberOfStatuses: number = 4;
			const nextStatus: number = (item.status % numberOfStatuses) + 1;
			const updatedItem: Item = { ...item, status: nextStatus };
			await itemService.updateItem(updatedItem);
			const updatedItems: Item[] = items.map((it) =>
				it.id === item.id ? updatedItem : it
			);
			setItems(updatedItems);
		} catch (error) {
			console.error("Error updating status:", error);
		}
	};

	const handleCommentChange = async (item: Item, comment: string) => {
		const updatedItem: Item = { ...item, comment: comment };
		await itemService.updateItem(updatedItem);
		const updatedItems: Item[] = items.map((i) =>
			i.id === item.id ? { ...i, comment } : i
		);
		setItems(updatedItems);
	};

	const handleKeyDown = (
		itemId: number,
		e: React.KeyboardEvent<HTMLInputElement>
	) => {
		if (e.key === "Enter") {
			e.currentTarget.blur();
		}
	};

	if (isLoading) {
		return (
			<div className="container d-flex justify-content-center align-items-center">
				<Spinner animation="border" variant="primary" className="spinner" />{" "}
			</div>
		);
	}

	return (
		<div className="container">
			<table className="table">
				<thead>
					<tr>
						<th scope="col">Nimi</th>
						<th scope="col">
							<StatusFilter
								selectedStatus={selectedStatus}
								setSelectedStatus={setSelectedStatus}
							/>
							Jäljellä
						</th>
						<th scope="col">Kommentti</th>
						<th scope="col">Muokattu</th>
						<th scope="col">
							<CategoryFilter
								selectedCategory={selectedCategory}
								setSelectedCategory={setSelectedCategory}
							/>
							Kategoria
						</th>
						<th scope="col"></th>
						<th scope="col">
							<Button
								className="btn-sm"
								onClick={() => setShowAddModal(true)}
								aria-label="Lisää uusi tuote"
							>
								Lisää
							</Button>
						</th>
					</tr>
				</thead>
				<tbody>
					{sortedItems.map((item) => {
						if (
							(selectedCategory === null ||
								item.category === selectedCategory) &&
							(selectedStatus === null || item.status === selectedStatus)
						) {
							return (
								<ItemRow
									item={item}
									handleStatusUpdate={handleStatusUpdate}
									handleCommentChange={handleCommentChange}
									handleKeyDown={handleKeyDown}
									handleEdit={handleEdit}
									handleDelete={handleDelete}
								/>
							);
						}
						return null;
					})}
				</tbody>
			</table>
			<div>
				{showAddModal && (
					<AddModal
						selectedItem={null}
						items={items}
						setItems={setItems}
						setShowModal={setShowAddModal}
					></AddModal>
				)}
				{showEditModal && (
					<EditModal
						selectedItem={selectedItem}
						items={items}
						setItems={setItems}
						setShowModal={setShowEditModal}
					></EditModal>
				)}
				{showDeleteModal && (
					<DeleteModal
						selectedItem={selectedItem}
						items={items}
						setItems={setItems}
						setShowModal={setShowDeleteModal}
					></DeleteModal>
				)}
			</div>
		</div>
	);
};

export default ItemList;
