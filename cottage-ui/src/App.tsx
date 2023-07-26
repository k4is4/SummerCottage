import React from "react";
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";

import "bootstrap/dist/css/bootstrap.min.css";
import Banner from "./components/banner";
import ItemList from "./components/itemList";
import Navigation from "./components/navigation";
import Calendar from "./components/calendar";

const App: React.FC = () => {
	return (
		<>
			<Banner></Banner>
			<Router>
				<div>
					<Navigation />
					<Routes>
						<Route path="/" element={<ItemList />} />
						<Route path="/request" element={<Calendar />} />
					</Routes>
				</div>
			</Router>
		</>
	);
};

export default App;
