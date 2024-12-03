import React from 'react';
import './index.css';
import App from './App';
import reportWebVitals from './reportWebVitals';
import './custom.scss';
import { BrowserRouter } from 'react-router-dom';
import { createRoot } from 'react-dom/client';

import { PublicClientApplication } from '@azure/msal-browser';
import { MsalProvider } from '@azure/msal-react';
import { msalConfig } from './authConfig';

/**
 * Initialize a PublicClientApplication instance which is provided to the MsalProvider component
 * We recommend initializing this outside of your root component to ensure it is not re-initialized on re-renders
 */
const msalInstance = new PublicClientApplication(msalConfig);

const root = document.getElementById('root');
if (root !== null) {
	const appRoot = createRoot(root);
	appRoot.render(
		<React.StrictMode>
			<MsalProvider instance={msalInstance}>
				<BrowserRouter>
					<App />
				</BrowserRouter>
			</MsalProvider>
		</React.StrictMode>
	);
}

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
