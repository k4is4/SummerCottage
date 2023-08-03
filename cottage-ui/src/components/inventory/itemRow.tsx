import React from "react";
import { AiOutlineEdit, AiOutlineDelete } from "react-icons/ai";
import { Button } from "react-bootstrap";
import Item from "../../types/item";
import { Category, Status } from "../../types/enums";

interface ItemRowProps {
	item: Item;
	handleStatusUpdate: (item: Item) => void;
	handleCommentChange: (item: Item, comment: string) => void;
	handleKeyDown: (
		itemId: number,
		e: React.KeyboardEvent<HTMLInputElement>
	) => void;
	handleEdit: (item: Item) => void;
	handleDelete: (item: Item) => void;
}

const ItemRow: React.FC<ItemRowProps> = ({
	item,
	handleStatusUpdate,
	handleCommentChange,
	handleKeyDown,
	handleEdit,
	handleDelete,
}) => {
	return (
		<tr key={item.id} tabIndex={0}>
			<td>{item.name}</td>
			<td>
				<Button
					onClick={() => handleStatusUpdate(item)}
					variant={
						item.status === 1
							? "primary"
							: item.status === 2
							? "warning"
							: item.status === 3
							? "danger"
							: ""
					}
					className="btn-sm"
				>
					{Status[item.status]}
				</Button>
			</td>
			<td>
				<input
					type="text"
					value={item.comment || ""}
					onChange={(e) => handleCommentChange(item, e.target.value)}
					onKeyDown={(e) => handleKeyDown(item.id, e)}
					className="comment-input"
				/>
			</td>
			<td>{new Date(item.updatedOn ?? "").toLocaleDateString("fi-FI")}</td>
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
};

export default ItemRow;
