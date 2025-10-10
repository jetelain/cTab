/*
	This is drawn every frame on the TAD dialog. fnc
*/
// is disableSerialization really required? If so, not sure this is the right place to call it
disableSerialization;

_cntrlScreen = _this select 0;
_display = ctrlParent _cntrlScreen;

cTabMapWorldPos = [_cntrlScreen] call cTab_fnc_ctrlMapCenter;
cTabMapScale = ctrlMapScale _cntrlScreen;

[_cntrlScreen,true] call cTab_fnc_drawUserMarkers;
[_cntrlScreen,1] call cTab_fnc_drawBftMarkers;

// current position
_veh = vehicle cTab_player;
_playerPos = getPosASL _veh;
_heading = direction _veh;
_lastKnownPosition = [_veh] call cTab_fnc_getBftLastKnownTracking;
if (!(_lastKnownPosition select 0)) then {
	_playerPos = _lastKnownPosition select 2;
	// only position is disable so no: _heading = _lastKnownPosition select 4;
};

_cntrlScreen drawIcon [cTabPlayerVehicleIcon,cTabTADfontColour,_playerPos,cTabTADownIconScaledSize,cTabTADownIconScaledSize,_heading,"", 1,cTabTxtSize,"TahomaB","right"];

// update hook information
call {
	if (cTabDrawMapTools) exitWith {
		[_display,_cntrlScreen,_playerPos,cTabMapCursorPos,0,true] call cTab_fnc_drawHook;
	};
	[_display,_cntrlScreen,_playerPos,cTabMapCursorPos,1,true] call cTab_fnc_drawHook;
};
true