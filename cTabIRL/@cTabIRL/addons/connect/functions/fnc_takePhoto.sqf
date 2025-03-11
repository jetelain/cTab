#include "script_component.hpp"	

if ( cameraView != "GUNNER" ) exitWith {
	INFO_1("Player does not have correct cameraView: '%1'",cameraView);
};

// Check if player is looking with a supported device:
// if cameraOn!=player => OK, vehicle or drone
// if cameraOn==player && {binocular player == currentWeapon player} => OK, using a binocular
if (cameraOn == player && {binocular player != currentWeapon player}) exitWith {
	INFO_1("Player not using vehicle optics or a binocular: currentWeapon='%1'",currentWeapon player);
};

// Check if service is available
if (!GVAR(canTakePhoto)) exitWith {
	systemChat LLSTRING(screenShotFailed);
};

private _squareHalfWidth = (safeZoneH * 3 / 4) / 2;
private _squareLeft = 0.5 - _squareHalfWidth;
private _squareRight = 0.5 + _squareHalfWidth;
private _squareTop = safeZoneY;
private _squareBottom = safeZoneY + safeZoneH;

private _data = [
	AGLToASL positionCameraToWorld [0,0,0], // Camera position
	AGLToASL screenToWorld [0.5, 0.5], // "Cross air" pointed position
	AGLToASL screenToWorld [_squareLeft, _squareTop], // Square top left
	AGLToASL screenToWorld [_squareLeft, _squareBottom], // Square bottom left
	AGLToASL screenToWorld [_squareRight, _squareTop], // Square top right
	AGLToASL screenToWorld [_squareRight, _squareBottom], // Square bottom right
	date
];

"cTabExtension" callExtension ["ScreenShot",[_data]];