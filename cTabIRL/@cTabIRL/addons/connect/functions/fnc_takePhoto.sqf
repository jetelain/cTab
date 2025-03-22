#include "script_component.hpp"	

if (!([] call ctab_intel_fnc_canTakePhoto)) exitWith {
	systemChat "Invalid device";
};

// Check if service is available
if (!GVAR(canTakePhoto)) exitWith {
	systemChat LLSTRING(screenShotNotAvailable);
};

private _areaHalfWidth = (safeZoneH * 3 / 4) / 2 * GVAR(photoRation);

// Extension will not respect ratio if screen ratio is smaller than the expected ratio
// For a server configured with 16:9 ratio, the result will depend on user screen ratio :
// - 4:3, 16:10 : Full image (but not expected ratio) => (safeZoneW/2)
// - 16:9       : Full image                          => _areaHalfWidth == (safeZoneW/2)
// - 21:9, 32:9 : Cropped image                       => _areaHalfWidth
_areaHalfWidth = (safeZoneW/2) min _areaHalfWidth;

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
private _canBeProjected = _worldWidthRatio > 0.25 && _worldWidthRatio < 4 && _worldHeightRatio > 0.25 && _worldHeightRatio < 4;

INFO_4("Take a screen shot. Projected image ratio is W=%1 H=%2. AreaWidth=%3 SafeZoneW=%4",_worldWidthRatio,_worldHeightRatio,_areaHalfWidth*2,safeZoneW);

private _markerPosition = _worldCenter;
if ( !_canBeProjected ) then {
	// If image can't be projected, use the coordinates of the object in front of camera
	private _camPosition = AGLToASL positionCameraToWorld [0, 0, 1];
	private _aimLinePos = AGLToASL positionCameraToWorld [0, 0, 5000];
	private _LIS = lineIntersectsSurfaces [_camPosition, _aimLinePos];
	if ( count _LIS > 0 ) then {
		_markerPosition = ((_LIS select 0) select 0);
	};
};

private _data = [
	_markerPosition, // "Cross air" pointed position
	date, // In-game date time
	_canBeProjected, // Image can be diplayed on map without significant distortion
	_dir, // Image direction
	[_worldRight vectorDistance _worldLeft, _worldTop vectorDistance _worldBottom], // Image size
	[_worldTopLeft, _worldTopRight, _worldBottomRight, _worldBottomLeft], // Area on map (to be able to draw polygon)
	AGLToASL positionCameraToWorld [0,0,0], // Camera position
	_worldCenter
];

"cTabExtension" callExtension ["ScreenShot",[_data]];