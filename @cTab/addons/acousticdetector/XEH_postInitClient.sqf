#include "script_component.hpp"

if (!hasInterface) exitWith {};

GVAR(caliberTypeCache) = createHashMap;
GVAR(shotsToProcess) = [];
GVAR(detectedShots) = [];
GVAR(distanceLimitPerCaliber) = [250,      750,       350,      550,      1000,     1350,     1650,      2400];
GVAR(caliberLabel) =            ["Rocket", "Missile", "5.56mm", "7.62mm", "12.7mm", "14.5mm", "20-40mm", "90mm+"];
GVAR(nextShotId) = 1;
GVAR(isActive) = false;
GVAR(pfhHandle) = -1;
GVAR(isInitialized) = false;

["CBA_settingsInitialized", FUNC(applySettings)] call CBA_fnc_addEventHandler;

#ifdef DEBUG_MODE_FULL

// GVAR(drawn) = [];

// [QGVAR(update), {
// 	private _toKeep = [];
// 	{
// 		_x params ['','_shotId','_point','_radius'];
// 		if ( !(_shotId in GVAR(drawn)) ) then {
// 			GVAR(drawn) pushBack _shotId;
// 			private _marker = createMarker [format ['_USER_DEFINED #0/shot%1C/0', _shotId], _point];
// 			_marker setMarkerShape "ELLIPSE";
// 			_marker setMarkerColor "ColorRed"; 
// 			_marker setMarkerSize [_radius, _radius];
// 		};
// 		_toKeep pushBack _shotId;
// 	} forEach GVAR(detectedShots);

// 	{
// 		deleteMarker format ['_USER_DEFINED #0/shot%1C/0', _x];
// 	} forEach (GVAR(drawn) - _toKeep);

// /*
// 	private _marker1 = createMarker [ format ['_USER_DEFINED #0/shot%1A/0', _shotId], _pointA];
// 	_marker1 setMarkerShape 'polyline';
// 	_marker1 setMarkerPolyline ((_pointA select [0,2]) + (_pointB select [0,2]) + (_pointC select [0,2]) +(_pointD select [0,2]) +(_pointA select [0,2]));
// 	_marker1 setMarkerColor "ColorRed"; 

// 	private _marker2 = createMarker [format ['_USER_DEFINED #0/shot%1C/0', _shotId], _point];
// 	_marker2 setMarkerShape "ELLIPSE";
// 	_marker2 setMarkerColor "ColorRed"; 
// 	_marker2 setMarkerSize [_radius, _radius];

// 	private _marker3 = createMarker [format ['_USER_DEFINED #0/shot%1R/0', _shotId], _source];
// 	_marker3 setMarkerType "mil_dot";
// 	_marker3 setMarkerColor "ColorBlack"; 
// */

// }] call CBA_fnc_addEventHandler;






#endif