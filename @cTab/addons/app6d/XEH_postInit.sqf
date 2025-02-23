#include "script_component.hpp"

plop = [];//["common identifier", "higher formation", "unique designation", 1, "date time group"];

// Only for tests
(findDisplay 12 displayCtrl 51) ctrlAddEventHandler ["draw", { 
	params ["_ctrl"];
	{
		private _sidc = markerText _x;
		if ( count _idc == 0 ) then { _sidc = DEFAULT_SIDC; };
		[_ctrl, _sidc, markerPos _x, _y, BASELINE_ICON_SIZE * ((markerSize _x) # 0), markerAlpha _x] call FUNC(drawMilsymbol);
	} forEach GVAR(makers);
}];

addMissionEventHandler ["MarkerCreated", {
	params ['_name'];
	if ( markerType _name == "ctab_app6d_generic") then {
		GVAR(makers) getOrDefault [_name, [], true];
		[{
			params ['_name'];
			private _sidc = markerText _name;
			if ( _sidc == "" ) then {
				_name setMarkerTextLocal "10031000131211050051";
			};
			_name setMarkerColorLocal "Transparent";
		}, [_name]] call CBA_fnc_execNextFrame;
	};
}];

addMissionEventHandler ["MarkerDeleted", {
	params ['_name'];
	if ( markerType _name == "ctab_app6d_generic") then {
		GVAR(makers) deleteAt _name;
	};
}];

["ace_markers_markerPlaced", {
	params ["_name"];
	// Marker is not yet created, look at ACE config informations
	if (ace_markers_currentMarkerConfigName == "ctab_app6d_generic") then {
		ace_markers_currentMarkerColorConfigName = "Transparent";
		GVAR(makers) set [_name, GVAR(editingOptions)];
	};
}] call CBA_fnc_addEventHandler;
