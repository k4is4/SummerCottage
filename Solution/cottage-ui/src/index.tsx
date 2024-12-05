import React from 'react';
import './index.css';
import App from './App';
import reportWebVitals from './reportWebVitals';
import './custom.scss';
import { BrowserRouter } from 'react-router-dom';
import { createRoot } from 'react-dom/client';

import { PublicClientApplication, EventType } from '@azure/msal-browser';
import { msalConfig } from './msalConfig';

/**
 * MSAL should be instantiated outside of the component tree to prevent it from being re-instantiated on re-renders.
 * For more, visit: https://github.com/AzureAD/microsoft-authentication-library-for-js/blob/dev/lib/msal-react/docs/getting-started.md
 */
const msalInstance = new PublicClientApplication(msalConfig);

const activeAccount = msalInstance.getActiveAccount();

// Default to using the first account if no account is active on page load
if (activeAccount && msalInstance.getAllAccounts().length > 0) {
	// Account selection logic is app dependent. Adjust as needed for different use cases.
	msalInstance.setActiveAccount(msalInstance.getAllAccounts()[0]);
}

// Listen for sign-in event and set active account
msalInstance.addEventCallback((event: any) => {
	if (event.eventType === EventType.LOGIN_SUCCESS && event.payload.account) {
		const account = event.payload.account;
		msalInstance.setActiveAccount(account);
	}
});

const root = document.getElementById('root');
if (root !== null) {
	const appRoot = createRoot(root);
	appRoot.render(
		<React.StrictMode>
			<BrowserRouter>
				<App instance={msalInstance} />
			</BrowserRouter>
		</React.StrictMode>
	);
}

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
