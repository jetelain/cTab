#include "script_component.hpp"

if ( !GVAR(isActive) ) exitWith { };

params ["_unit", "", "", "", "_ammo", "", "_projectile", "_vehicle"];

if ( _unit == player || { _vehicle == vehicle player } ) exitWith {}; // system is able to ignore shots from vehicle

private _pos1 = if ( isNull _vehicle ) then { getPosASL _unit } else { getPosASL _vehicle };
private _pos2 = getPosASL _projectile;

private _caliber = GVAR(caliberTypeCache) getOrDefaultCall [_ammo, {[_ammo] call FUNC(computeAmmoCaliberType)}, true];

if ( _caliber != -1 ) then {
	// Register shot for PFH
	GVAR(shotsToProcess) pushBack [diag_tickTime, _pos1, _caliber];
};
