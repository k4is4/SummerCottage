import { PublicClientApplication } from '@azure/msal-browser';

const msalConfig = {
	auth: {
		clientId: '111726ea-c3c1-4d90-8ec4-f3a2595c3bd0',
		authority:
			'https://login.microsoftonline.com/bf05f699-9934-4606-9238-1a93f805568f', // This is a URL (e.g. https://login.microsoftonline.com/{your tenant ID})
		redirectUri:
			'https://icy-pond-0f1e60f03.4.azurestaticapps.net/.auth/login/aad/callback',
	},
};

const msalInstance = new PublicClientApplication(msalConfig);

export default msalInstance;
