import React from 'react';

/**
 * Renders data retrieved from API
 * @param props
 */
export const APIData = (props) => {
	return (
		<div>
			<p>
				<strong>API Returned: </strong> {props.apiData}
			</p>
		</div>
	);
};
