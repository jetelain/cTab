#include "script_component.hpp"
/*
    This is drawn every frame on the vehicle display. fnc

    (previously in player_init.sqf)
*/
_cntrlScreen = _this select 0;
_display = ctrlParent _cntrlScreen;

cTabMapWorldPos = [_cntrlScreen] call cTab_fnc_ctrlMapCenter;
cTabMapScale = ctrlMapScale _cntrlScreen;

[_cntrlScreen,true] call cTab_fnc_drawUserMarkers;
private _drawPlayer = [_cntrlScreen,0] call cTab_fnc_drawBftMarkers;

// draw directional arrow at own location
_veh = vehicle cTab_player;
_playerPos = getPosASL _veh;
if (_drawPlayer) then {
    _heading = direction _veh;
    _cntrlScreen drawIcon ["\A3\ui_f\data\map\VehicleIcons\iconmanvirtual_ca.paa",cTabMicroDAGRfontColour,_playerPos,cTabTADownIconBaseSize,cTabTADownIconBaseSize,_heading,"", 1,cTabTxtSize,"TahomaB","right"];
};

// update hook information
if (cTabDrawMapTools) then {
    [_display,_cntrlScreen,_playerPos,cTabMapCursorPos,0,false] call cTab_fnc_drawHook;
};

true