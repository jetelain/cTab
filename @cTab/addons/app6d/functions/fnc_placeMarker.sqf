#include "script_component.hpp"
params ["_display", "_closeNum"];

GVAR(drawPreview) = false;
((displayParent _display) displayCtrl 51 /*IDC_MAP*/) ctrlRemoveEventHandler ["draw", GVAR(mapEH)];

if ( !isNil "ace_markers_fnc_placeMarker" ) then {
	_this call ace_markers_fnc_placeMarker;
	if ( _closeNum == 1 ) then { 
		GVAR(editingOptions) set [0, ctrlText (_display displayCtrl 9900)];
		GVAR(editingOptions) set [1, ctrlText (_display displayCtrl 9901)];
		GVAR(editingOptions) set [2, ctrlText (_display displayCtrl 9902)];
		GVAR(editingOptions) set [3, lbCurSel (_display displayCtrl 9903)];
		GVAR(editingOptions) set [4, ctrlText (_display displayCtrl 9904)];
		// TODO: network dispatch
	 };
} else {
	["onUnload",_this,"RscDisplayInsertMarker",'GUI'] call (uinamespace getvariable 'BIS_fnc_initDisplay');
};
