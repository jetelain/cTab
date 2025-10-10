/*
	This is drawn every frame on the tablet uav screen. fnc
*/
if (isNil 'cTabActUav') exitWith {};
if (cTabActUav == player) exitWith {};

_cntrlScreen = _this select 0;
_display = ctrlParent _cntrlScreen;
_pos = getPosASL cTabActUav;

[_cntrlScreen,false] call cTab_fnc_drawUserMarkers;
[_cntrlScreen,0] call cTab_fnc_drawBftMarkers;

// draw icon at own location
// current position
_veh = vehicle cTab_player;
_playerPos = getPosASL _veh;
_heading = direction _veh;
_lastKnownPosition = [_veh] call cTab_fnc_getBftLastKnownTracking;
if (!(_lastKnownPosition select 0)) then {
	_playerPos = _lastKnownPosition select 2;
	// only position is disable so no: _heading = _lastKnownPosition select 4;
};


_cntrlScreen drawIcon ["\A3\ui_f\data\map\VehicleIcons\iconmanvirtual_ca.paa",cTabMicroDAGRfontColour,_playerPos,cTabTADownIconBaseSize,cTabTADownIconBaseSize,_heading,"", 1,cTabTxtSize,"TahomaB","right"];

// draw icon at UAV location
_cntrlScreen drawIcon ["\A3\ui_f\data\map\VehicleIcons\iconmanvirtual_ca.paa",cTabTADhighlightColour,_pos,cTabTADownIconBaseSize,cTabTADownIconBaseSize,direction cTabActUav,"",0,cTabTxtSize,"TahomaB","right"];

_cntrlScreen ctrlMapAnimAdd [0,cTabMapScaleUAV,_pos];
ctrlMapAnimCommit _cntrlScreen;
true