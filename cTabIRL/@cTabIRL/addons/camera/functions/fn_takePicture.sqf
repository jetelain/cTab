#include "script_component.hpp"

// Code forked from Hate's Digital Camera
// https://steamcommunity.com/sharedfiles/filedetails/?id=2577441180

params ["_unit"];

if (cameraView == "gunner") then
{
	//Play shutter sound in 3D (for multiplayer reasons)
	[player, [hateShutterSound, 15, 1]] remoteExec ["say3D", 0, false];

	if (_unit getVariable ["Lala_Hate_DSLR_Flash_Enabled", false]) then {
		//flash script
		DSLR_light = "#lightpoint" createVehicle getPos player;
		DSLR_light setLightBrightness 0.6;
		DSLR_light setLightAmbient [0.76,0.99,0.97];
		DSLR_light setLightColor [0.98,0.99,0.81];
		DSLR_light setLightUseFlare true;
		DSLR_light setLightFlareSize 2;
		DSLR_light setLightFlareMaxDistance 100;
		DSLR_light lightAttachObject [player,[0,.1,1.7]];
	};
	sleep .1;

	if ( GVAR(mode) == 0 || GVAR(mode) == 1 ) then {
		// Use cTab connect to take a photo
		EGVAR(connect,ignoreSound) = true;
		call EFUNC(connect,takePhoto);
	};

	if ( GVAR(mode) == 0 || GVAR(mode) == 2 ) then {
		// Save screenshot to /profile directory/Screenshots with format YYYY_MM_DD_hh_mm_ss.png
		screenshot "";
	};
};

sleep 0.4;

private _dslr_light = missionNamespace getVariable ["DSLR_light", objNull];

if (!isNull _dslr_light) then {
	deleteVehicle DSLR_light;
};