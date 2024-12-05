import React from 'react';
import { Route, Routes } from 'react-router-dom';

import 'bootstrap/dist/css/bootstrap.min.css';
import Banner from './components/banner';
import ItemList from './components/inventory/itemList';
import Navigation from './components/navigation';
import CalendarComponent from './components/calendar/calendarComponent';

import { MsalProvider } from '@azure/msal-react';
import { PublicClientApplication } from '@azure/msal-browser';

interface AppProps {
	instance: PublicClientApplication;
}

const App: React.FC<AppProps> = ({ instance }) => {
	return (
		<MsalProvider instance={instance}>
			<Banner></Banner>
			<div>
				<Navigation />
				<Routes>
					<Route path="/" element={<ItemList />} />
					<Route path="/calendar" element={<CalendarComponent />} />
				</Routes>
			</div>
		</MsalProvider>
	);
};

export default App;
