#include "script_component.hpp"
/*
	Name: cTab_fnc_onIfOpen
	
	Author(s):
		Gundy
	
	Description:
		Handles dialog / display setup, called by "onLoad" event
	
	Parameters:
		0: Display
	
	Returns:
		BOOLEAN - TRUE
	
	Example:
		// open TAD display as main interface type
		[_dispaly] call cTab_fnc_onIfOpen;
*/

params ["_display"];

uiNamespace setVariable [cTabIfOpen select 1,_display];

[] call cTab_fnc_updateInterface;

cTabIfOpenStart = false;

// ctab_interface_open : _display, _displayName, _player, _vehicle
[QGVARMAIN(interface_open), [_display,cTabIfOpen select 1,cTabIfOpen select 2,cTabIfOpen select 4]] call CBA_fnc_localEvent;

true