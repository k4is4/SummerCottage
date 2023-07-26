import React, { useEffect, useState } from "react";
import { AiOutlineEdit, AiOutlineDelete } from "react-icons/ai";
import Item from "../types/item";
import EditModal from "./editModal";
import DeleteModal from "./deleteModal";
import { Button } from "react-bootstrap";
import AddModal from "./addModal";
import "./itemList.css";
import itemService from "../services/ItemService";

const ItemList: React.FC = () => {
	const [items, setItems] = useState<Item[]>([]);
	const [selectedItem, setSelectedItem] = useState<Item | null>(null);
	const [showAddModal, setShowAddModal] = useState(false);
	const [showEditModal, setShowEditModal] = useState(false);
	const [showDeleteModal, setShowDeleteModal] = useState(false);

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

	// Step 1: Define a map for status translations
	const statusMap: { [key: number]: string } = {
		1: "Paljon",
		2: "Löytyy",
		3: "Lopussa",
	};

	const categoryMap: { [key: number]: string } = {
		1: "Kuivaruoka",
		2: "Jääkaappi",
		3: "Juomat",
		4: "Muut",
	};

	// Step 3: Handle status update when clicking on the status text
	const handleStatusUpdate = async (item: Item) => {
		try {
			// Calculate the next status value (handling the undefined case)
			const nextStatus = item.status === undefined ? 1 : (item.status % 3) + 1;

			// Update the item with the new status
			const updatedItem = { ...item, status: nextStatus };
			await itemService.updateItem(updatedItem);

			// Update the item list in the state
			const updatedItems = items.map((it) =>
				it.id === item.id ? updatedItem : it
			);
			setItems(updatedItems);
		} catch (error) {
			console.error("Error updating status:", error);
		}
	};

	return (
		<div className="container">
			<table className="table">
				<thead>
					<tr>
						<th>Nimi</th>
						<th>Jäljellä</th>
						<th>Kommentti</th>
						<th>Kategoria</th>
						<th></th>
						<th>
							<Button className="btn-sm" onClick={() => setShowAddModal(true)}>
								Lisää
							</Button>
						</th>
					</tr>
				</thead>
				<tbody>
					{items.map((item) => (
						<tr key={item.id}>
							<td>{item.name}</td>
							<td>
								<Button
									onClick={() => handleStatusUpdate(item)}
									variant={`${
										item.status !== undefined
											? item.status === 1
												? "primary"
												: item.status === 2
												? "warning"
												: "danger"
											: ""
									}`}
									className="btn-sm"
								>
									{item.status !== undefined ? statusMap[item.status] : "N/A"}
								</Button>
							</td>
							<td>{item.comment}</td>
							<td>
								{item.category !== undefined
									? categoryMap[item.category]
									: "N/A"}
							</td>
							<td>
								<AiOutlineEdit
									className="edit-icon"
									onClick={() => handleEdit(item)}
								/>
							</td>
							<td>
								<AiOutlineDelete
									className="delete-icon"
									onClick={() => handleDelete(item)}
								/>
							</td>
						</tr>
					))}
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
