/*
    This is drawn every frame on the microDAGR dialog. fnc
*/

_cntrlScreen = _this select 0;
_display = ctrlParent _cntrlScreen;

cTabMapWorldPos = [_cntrlScreen] call cTab_fnc_ctrlMapCenter;
cTabMapScale = ctrlMapScale _cntrlScreen;

// current position
_veh = vehicle cTab_player;
_playerPos = getPosASL _veh;
_heading = direction _veh;

[_cntrlScreen,false] call cTab_fnc_drawUserMarkers;
[_cntrlScreen,cTabMicroDAGRmode] call cTab_fnc_drawBftMarkers;

// draw directional arrow at own location
_cntrlScreen drawIcon ["\A3\ui_f\data\map\VehicleIcons\iconmanvirtual_ca.paa",cTabMicroDAGRfontColour,_playerPos,cTabTADownIconBaseSize,cTabTADownIconBaseSize,_heading,"", 1,cTabTxtSize,"TahomaB","right"];

// update hook information
if (cTabDrawMapTools) then {
    [_display,_cntrlScreen,_playerPos,cTabMapCursorPos,0,false] call cTab_fnc_drawHook;
};

true