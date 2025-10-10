/*
	This is drawn every frame on the TAD display. fnc

	(previously in player_init.sqf)
*/

// is disableSerialization really required? If so, not sure this is the right place to call it
disableSerialization;

_cntrlScreen = _this select 0;
_display = ctrlParent _cntrlScreen;


// current position
_veh = vehicle cTab_player;
_playerPos = getPosASL _veh;
_heading = direction _veh;
_lastKnownPosition = [_veh] call cTab_fnc_getBftLastKnownTracking;
if (!(_lastKnownPosition select 0)) then {
	_playerPos = _lastKnownPosition select 2;
	// only position is disable so no: _heading = _lastKnownPosition select 4;
};

// change scale of map and centre to player position
_cntrlScreen ctrlMapAnimAdd [0, cTabMapScale, _playerPos];
ctrlMapAnimCommit _cntrlScreen;

[_cntrlScreen,false] call cTab_fnc_drawUserMarkers;
[_cntrlScreen,1] call cTab_fnc_drawBftMarkers;

// draw vehicle icon at own location
_cntrlScreen drawIcon [cTabPlayerVehicleIcon,cTabTADfontColour,_playerPos,cTabTADownIconBaseSize,cTabTADownIconBaseSize,_heading,"", 1,cTabTxtSize,"TahomaB","right"];

// draw TAD overlay (two circles, one at full scale, the other at half scale + current heading)
_cntrlScreen drawIcon ["\cTab\img\TAD_overlay_ca.paa",cTabTADfontColour,_playerPos,250,250,_heading,"",1,cTabTxtSize,"TahomaB","right"];

true