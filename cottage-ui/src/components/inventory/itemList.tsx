import React, { useEffect, useState } from "react";
import { AiOutlineEdit, AiOutlineDelete } from "react-icons/ai";
import Item from "../../types/item";
import EditModal from "./editModal";
import DeleteModal from "./deleteModal";
import { Button } from "react-bootstrap";
import AddModal from "./addModal";
import "./itemList.css";
import itemService from "../../services/ItemService";
import { Category, Status } from "../../types/enums";

const ItemList: React.FC = () => {
	const [items, setItems] = useState<Item[]>([]);
	const [selectedItem, setSelectedItem] = useState<Item | null>(null);
	const [showAddModal, setShowAddModal] = useState(false);
	const [showEditModal, setShowEditModal] = useState(false);
	const [showDeleteModal, setShowDeleteModal] = useState(false);
	const [selectedCategory, setSelectedCategory] = useState<number | null>(null);
	const [selectedStatus, setSelectedStatus] = useState<number | null>(null);
	const sortedItems = [...items].sort((a, b) => a.name.localeCompare(b.name));

	useEffect(() => {
		fetchData();
	}, []);

	const fetchData = async () => {
		try {
			const itemsFromApi = await itemService.getItems();
			setItems(itemsFromApi);
		} catch (error) {
			console.error("Error fetching items:", error);
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
			const nextStatus = (item.status % 4) + 1;
			const updatedItem = { ...item, status: nextStatus };
			await itemService.updateItem(updatedItem);
			const updatedItems = items.map((it) =>
				it.id === item.id ? updatedItem : it
			);
			setItems(updatedItems);
		} catch (error) {
			console.error("Error updating status:", error);
		}
	};

	const handleCommentChange = async (item: Item, comment: string) => {
		const updatedItem = { ...item, comment: comment };
		await itemService.updateItem(updatedItem);
		const updatedItems = items.map((i) =>
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

	return (
		<div className="container">
			<table className="table">
				<thead>
					<tr>
						<th scope="col">Nimi</th>
						<th scope="col">
							<div>
								<label htmlFor="statusFilter"></label>
								<select
									id="statusFilter"
									value={selectedStatus || ""}
									onChange={(e) =>
										setSelectedStatus(
											e.target.value ? parseInt(e.target.value, 10) : null
										)
									}
								>
									<option value="">Kaikki</option>
									<option value={Status.Paljon}>{Status[Status.Paljon]}</option>
									<option value={Status.Löytyy}>{Status[Status.Löytyy]}</option>
									<option value={Status.Lopussa}>
										{Status[Status.Lopussa]}
									</option>
									<option value={Status["?"]}>{Status[Status["?"]]}</option>
								</select>
							</div>
							Jäljellä
						</th>
						<th scope="col">Kommentti</th>
						<th scope="col">Muokattu</th>
						<th scope="col">
							<div>
								<label htmlFor="categoryFilter"></label>
								<select
									id="categoryFilter"
									value={selectedCategory || ""}
									onChange={(e) =>
										setSelectedCategory(
											e.target.value ? parseInt(e.target.value, 10) : null
										)
									}
								>
									<option value="">Kaikki</option>
									<option value={Category.Kuivaruoka}>
										{Category[Category.Kuivaruoka]}
									</option>
									<option value={Category.Jääkaappi}>
										{Category[Category.Jääkaappi]}
									</option>
									<option value={Category.Juomat}>
										{Category[Category.Juomat]}
									</option>
									<option value={Category.Muut}>
										{Category[Category.Muut]}
									</option>
								</select>
							</div>
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
								<tr key={item.id} tabIndex={0}>
									<td>{item.name}</td>
									<td>
										<Button
											onClick={() => handleStatusUpdate(item)}
											variant={`${
												item.status === 1
													? "primary"
													: item.status === 2
													? "warning"
													: item.status === 3
													? "danger"
													: ""
											}`}
											className="btn-sm"
										>
											{Status[item.status]}
										</Button>
									</td>
									<td>
										<input
											type="text"
											value={item.comment || ""}
											onChange={(e) =>
												handleCommentChange(item, e.target.value)
											}
											onKeyDown={(e) => handleKeyDown(item.id, e)}
											className="comment-input"
										/>
									</td>
									<td>
										{new Date(item.updatedOn ?? "").toLocaleDateString("fi-FI")}
									</td>
									<td>{Category[item.category]}</td>
									<td>
										<AiOutlineEdit
											tabIndex={0}
											className="edit-icon"
											onClick={() => handleEdit(item)}
											onKeyDown={(e) => {
												if (e.key === "Enter") {
													handleEdit(item);
												}
											}}
											aria-label={`Muokkaa ${item.name}`}
										/>
									</td>
									<td>
										<AiOutlineDelete
											tabIndex={0}
											className="delete-icon"
											onClick={() => handleDelete(item)}
											onKeyDown={(e) => {
												if (e.key === "Enter") {
													handleDelete(item);
												}
											}}
											aria-label={`Poista ${item.name}`}
										/>
									</td>
								</tr>
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
