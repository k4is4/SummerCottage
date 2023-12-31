import React from "react";
import "./index.css";
import App from "./App";
import reportWebVitals from "./reportWebVitals";
import "./custom.scss";
import { BrowserRouter } from "react-router-dom";
import { createRoot } from "react-dom/client";

const root = document.getElementById("root");
if (root !== null) {
	const appRoot = createRoot(root);
	appRoot.render(
		<React.StrictMode>
			<BrowserRouter>
				<App />
			</BrowserRouter>
		</React.StrictMode>
	);
}

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
