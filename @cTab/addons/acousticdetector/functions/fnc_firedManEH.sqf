#include "script_component.hpp"

if ( !GVAR(isActive) ) exitWith { };

params ["_unit", "", "", "", "_ammo", "", "_projectile", "_vehicle"];

if ( _unit == player || { _vehicle == vehicle player } ) exitWith {}; // system is able to ignore shots from vehicle

private _pos1 = if ( isNull _vehicle ) then { getPosASL _unit } else { getPosASL _vehicle };
private _pos2 = getPosASL _projectile;

// TODO: Arma 2.12 => private _caliber = GVAR(caliberTypeCache) getOrDefaultCall [_ammo, {[_ammo] call FUNC(computeAmmoCaliberType)}, true];

private _caliber = GVAR(caliberTypeCache) getOrDefault [_ammo,-2];
if (_caliber == -2) then {
	_caliber = [_ammo] call FUNC(computeAmmoCaliberType);
	GVAR(caliberTypeCache) set [_ammo, _caliber];
}; 

if ( _caliber != -1 ) then {
	// Register shot for PFH
	#ifdef DEBUG_MODE_FULL
	private _caliberLabel = GVAR(caliberLabel) select _caliber;
	TRACE_4("FiredMan", _pos1, _pos2, _ammo, _caliberLabel);
	#endif
	GVAR(shotsToProcess) pushBack [diag_tickTime, _pos1, _caliber];
};
