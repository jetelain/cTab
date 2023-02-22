#include "script_component.hpp"
#include "\a3\editor_f\Data\Scripts\dikCodes.h"

if (!hasInterface) exitWith {};

GVAR(lastPosition) = [];
GVAR(binoculars) = ["Rangefinder", "Laserdesignator", "Laserdesignator_02", "Laserdesignator_02_ghex_F", "Laserdesignator_03", "Laserdesignator_03_ghex_F"];

// Vanilla RangeFinder
addUserActionEventHandler ["gunElevAuto", "Activate", {
	call FUNC(handleRangeFinderKey);
}];

// ACE Vector RangeFinder
["ace_vector_rangefinderData", { 
	params ["_distance", "_azimuth", "_inclination"];
	private _position = (eyePos player) vectorAdd ([_distance, _azimuth, _inclination] call CBA_fnc_polar2vect);
	[QGVAR(data), [_position, _distance]] call CBA_fnc_localEvent;
}] call CBA_fnc_addEventHandler;

// Ingame UI integration
[QGVAR(data), {
	params ["_position", "_distance"];

	INFO_1('data %1', _position);

	GVAR(lastPosition) = _position;

	['cTab_Tablet_dlg',[['mode','BFT']]] call cTab_fnc_setSettings;
	['cTab_Android_dlg',[['mode','BFT']]] call cTab_fnc_setSettings;

}] call CBA_fnc_addEventHandler;

[QGVARMAIN(interface_open), { 
	params ["_display", "_displayName", "_player", "_vehicle"];

	if ( count GVAR(lastPosition) != 0 && {[_displayName] call cTab_fnc_isDialog}) then {

		private _targetMapName = [_displayName,"mapType"] call cTab_fnc_getSettings;
		private _mapTypes = [_displayName,"mapTypes"] call cTab_fnc_getSettings;
		private _targetMapIDC = [_mapTypes,_targetMapName] call cTab_fnc_getFromPairs;
		private _targetMapCtrl = _display displayCtrl _targetMapIDC;

		[_targetMapCtrl] spawn {
			params ['_targetMapCtrl'];
			_targetMapCtrl ctrlMapAnimAdd [0, ctrlMapScale _targetMapCtrl, GVAR(lastPosition)];
			ctrlMapAnimCommit _targetMapCtrl;
			sleep 0.25;
			private _uiPosition = _targetMapCtrl ctrlMapWorldToScreen GVAR(lastPosition);
			[3300,[_targetMapCtrl, 0, _uiPosition select 0, _uiPosition select 1]] execVM '\cTab\shared\cTab_markerMenu_load.sqf';
			GVAR(lastPosition) = [];
		};
	};

}] call CBA_fnc_addEventHandler;
