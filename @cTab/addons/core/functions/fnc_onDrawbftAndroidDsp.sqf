#include "script_component.hpp"
/*
    This is drawn every frame on the android display. fnc
*/
_cntrlScreen = _this select 0;
_display = ctrlParent _cntrlScreen;

_veh = vehicle cTab_player;
_playerPos = getPosASL _veh;

// change scale of map and centre to player position
_cntrlScreen ctrlMapAnimAdd [0, cTabMapScale, _playerPos];
ctrlMapAnimCommit _cntrlScreen;

[_cntrlScreen,false] call cTab_fnc_drawUserMarkers;
private _drawPlayer = [_cntrlScreen,0] call cTab_fnc_drawBftMarkers;

if (_drawPlayer) then {
    // draw directional arrow at own location
    _heading = direction _veh;
    _cntrlScreen drawIcon ["\A3\ui_f\data\map\VehicleIcons\iconmanvirtual_ca.paa",cTabMicroDAGRfontColour,_playerPos,cTabTADownIconBaseSize,cTabTADownIconBaseSize,_heading,"", 1,cTabTxtSize,"TahomaB","right"];
};

true