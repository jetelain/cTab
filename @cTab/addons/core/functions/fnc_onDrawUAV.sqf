/*
	This is drawn every frame on the tablet uav screen. fnc
*/
if (isNil 'cTabActUav') exitWith {};
if (cTabActUav == player) exitWith {};

_cntrlScreen = _this select 0;
_display = ctrlParent _cntrlScreen;
_pos = getPosASL cTabActUav;

if (isNil "cTab_player" || {isNull cTab_player}) exitWith {};

private _visBounds = [_cntrlScreen] call cTab_fnc_ctrlMapVisibleBounds;
[_cntrlScreen,false,_visBounds] call cTab_fnc_drawUserMarkers;
[_cntrlScreen,0,_visBounds] call cTab_fnc_drawBftMarkers;

// draw icon at own location
_veh = vehicle cTab_player;
_cntrlScreen drawIcon ["\A3\ui_f\data\map\VehicleIcons\iconmanvirtual_ca.paa",cTabMicroDAGRfontColour,getPosASL _veh,cTabTADownIconBaseSize,cTabTADownIconBaseSize,direction _veh,"", 1,cTabTxtSize,"TahomaB","right"];

// draw icon at UAV location
_cntrlScreen drawIcon ["\A3\ui_f\data\map\VehicleIcons\iconmanvirtual_ca.paa",cTabTADhighlightColour,_pos,cTabTADownIconBaseSize,cTabTADownIconBaseSize,direction cTabActUav,"",0,cTabTxtSize,"TahomaB","right"];

_cntrlScreen ctrlMapAnimAdd [0,cTabMapScaleUAV,_pos];
ctrlMapAnimCommit _cntrlScreen;
true