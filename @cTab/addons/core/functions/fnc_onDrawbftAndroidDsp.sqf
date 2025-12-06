#include "script_component.hpp"
/*
	This is drawn every frame on the android display. fnc
*/
_cntrlScreen = _this select 0;
_display = ctrlParent _cntrlScreen;

// current position
_veh = vehicle cTab_player;
_playerPos = getPosASL _veh;
_heading = direction _veh;
if (!(_veh getVariable [QGVAR(enabled),true])) then {
    _lastKnownTracking = _veh getVariable [QGVAR(lastKnownTracking), [ [0, 0, 0], 0, "000000"]];
    _playerPos = _lastKnownTracking select 0;
};

// change scale of map and centre to player position
_cntrlScreen ctrlMapAnimAdd [0, cTabMapScale, _playerPos];
ctrlMapAnimCommit _cntrlScreen;

[_cntrlScreen,false] call cTab_fnc_drawUserMarkers;
private _drawPlayer = [_cntrlScreen,0] call cTab_fnc_drawBftMarkers;

if (_drawPlayer) then {
	// draw directional arrow at own location
	_cntrlScreen drawIcon ["\A3\ui_f\data\map\VehicleIcons\iconmanvirtual_ca.paa",cTabMicroDAGRfontColour,_playerPos,cTabTADownIconBaseSize,cTabTADownIconBaseSize,_heading,"", 1,cTabTxtSize,"TahomaB","right"];
};

true