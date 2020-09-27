/*
	This is drawn every frame on the tablet helmet cam screen. fnc
*/
if (isNil 'cTabHcams') exitWith {};
_camHost = cTabHcams select 2;

_cntrlScreen = _this select 0;
_display = ctrlParent _cntrlScreen;
_pos = getPosASL _camHost;

[_cntrlScreen,false] call cTab_fnc_drawUserMarkers;
[_cntrlScreen,0] call cTab_fnc_drawBftMarkers;

// draw icon at own location
_veh = vehicle cTab_player;
_cntrlScreen drawIcon ["\A3\ui_f\data\map\VehicleIcons\iconmanvirtual_ca.paa",cTabMicroDAGRfontColour,getPosASL _veh,cTabTADownIconBaseSize,cTabTADownIconBaseSize,direction _veh,"", 1,cTabTxtSize,"TahomaB","right"];

// draw icon at helmet cam location
_cntrlScreen drawIcon ["\A3\ui_f\data\map\VehicleIcons\iconmanvirtual_ca.paa",cTabTADhighlightColour,_pos,cTabTADownIconBaseSize,cTabTADownIconBaseSize,direction _camHost,"",0,cTabTxtSize,"TahomaB","right"];

_cntrlScreen ctrlMapAnimAdd [0,cTabMapScaleHCam,_pos];
ctrlMapAnimCommit _cntrlScreen;
true