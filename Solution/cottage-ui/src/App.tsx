import React, { useState } from 'react';
import { MsalAuthenticationTemplate } from '@azure/msal-react';
import { InteractionType } from '@azure/msal-browser';
// import { loginRequest } from './authConfig';
import { PageLayout } from './components/PageLayout';
import { APIData } from './components/APIData';
import Button from 'react-bootstrap/Button';
import './styles/App.css';

/**
 * Renders name of the signed-in user and a button to retrieve data from an API
 */
const AppContent = () => {
	const [apiData, setApiData] = useState(null);

	function CallAPI() {
		fetch('https://app-kaisa-backend.azurewebsites.net/', {
			method: 'get',
		})
			.then((data) => data.json())
			.then((json) => {
				console.log(json);
				setApiData(json);
			});
	}

	return (
		<>
			<h5 className="card-title">Welcome</h5>
			{apiData ? (
				<APIData apiData={apiData} />
			) : (
				<div>
					No Data from API. Click Call API!
					<br />
					<br />
				</div>
			)}
			<Button variant="secondary" onClick={CallAPI}>
				Call API
			</Button>
		</>
	);
};

/**
 * If a user is authenticated the AppContent component above is rendered. Otherwise the content is not rendered.
 */
const MainContent = () => {
	return (
		<div className="App">
			{
				<MsalAuthenticationTemplate
					interactionType={InteractionType.Redirect}
					// authenticationRequest={loginRequest}
				>
					<AppContent />
				</MsalAuthenticationTemplate>
			}
		</div>
	);
};

export default function App() {
	return (
		<PageLayout>
			<MainContent />
		</PageLayout>
	);
}
