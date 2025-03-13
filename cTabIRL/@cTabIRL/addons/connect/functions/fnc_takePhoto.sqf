#include "script_component.hpp"	

if (!([] call ctab_intel_fnc_canTakePhoto)) exitWith {
	systemChat "Invalid device";
};

// Check if service is available
if (!GVAR(canTakePhoto)) exitWith {
	systemChat LLSTRING(screenShotFailed);
};

private _areaHalfWidth = (safeZoneH * 3 / 4) / 2 * GVAR(photoRation);
private _areaLeft = 0.5 - _areaHalfWidth;
private _areaRight = 0.5 + _areaHalfWidth;
private _areaTop = safeZoneY;
private _areaBottom = safeZoneY + safeZoneH;

private _worldCenter = AGLToASL screenToWorld [0.5, 0.5];

private _worldTop = AGLToASL screenToWorld [0.5, _areaTop];
private _worldBottom = AGLToASL screenToWorld [0.5, _areaBottom];
private _dir = _worldBottom getDir _worldTop;

private _worldTopLeft = AGLToASL screenToWorld [_areaLeft, _areaTop];
private _worldBottomLeft = AGLToASL screenToWorld [_areaLeft, _areaBottom];
private _worldTopRight = AGLToASL screenToWorld [_areaRight, _areaTop];
private _worldBottomRight = AGLToASL screenToWorld [_areaRight, _areaBottom];

private _worldTop = AGLToASL screenToWorld [0.5, _areaTop];
private _worldBottom = AGLToASL screenToWorld [0.5, _areaBottom];
private _worldRight = AGLToASL screenToWorld [_areaRight, 0.5];
private _worldLeft = AGLToASL screenToWorld [_areaLeft, 0.5];

private _worldWidthRatio = (_worldTopLeft vectorDistance _worldTopRight) / (_worldBottomLeft vectorDistance _worldBottomRight);
private _worldHeightRatio = (_worldTopLeft vectorDistance _worldBottomLeft) / (_worldTopRight vectorDistance _worldBottomRight);
private _canBeProjected = _worldWidthRatio > 0.33 && _worldWidthRatio < 3 && _worldHeightRatio > 0.33 && _worldHeightRatio < 3;

private _data = [
	_worldCenter, // "Cross air" pointed position
	date, // In-game date time
	_canBeProjected, // Image can be diplayed on map without significant distortion
	_dir, // Image direction
	[_worldRight vectorDistance _worldLeft, _worldTop vectorDistance _worldBottom], // Image size
	[_worldTopLeft, _worldTopRight, _worldBottomRight, _worldBottomLeft], // Area on map (to be able to draw polygon)
	AGLToASL positionCameraToWorld [0,0,0] // Camera position
];

"cTabExtension" callExtension ["ScreenShot",[_data]];