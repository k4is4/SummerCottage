import React, { useState } from 'react';
import { useMsal, MsalAuthenticationTemplate } from '@azure/msal-react';
import { InteractionType } from '@azure/msal-browser';
import { loginRequest } from './authConfig';
import { PageLayout } from './components/PageLayout';
import { APIData } from './components/APIData';
import Button from 'react-bootstrap/Button';
import './styles/App.css';

/**
 * Renders name of the signed-in user and a button to retrieve data from an API
 */
const AppContent = () => {
	const { instance, accounts } = useMsal();
	const [apiData, setApiData] = useState(null);

	function CallAPI() {
		// Silently acquires an access token which is then attached to a request for API call
		instance
			.acquireTokenSilent({
				...loginRequest,
				account: accounts[0],
			})
			.then((response) => {
				console.log(response.accessToken);

				fetch('{Function App API URL}', {
					method: 'post',
					headers: new Headers({
						Authorization: 'Bearer ' + response.accessToken,
						Accept: 'application/json',
					}),
				})
					.then((data) => data.json())
					.then((json) => {
						console.log(json);
						setApiData(json);
					});
			});
	}

	return (
		<>
			<h5 className="card-title">Welcome {accounts[0].name}</h5>
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
					authenticationRequest={loginRequest}
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
