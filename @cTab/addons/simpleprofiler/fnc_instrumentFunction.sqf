#include "script_component.hpp"
params ["_code","_name","_isReturning",["_isHighFreq",false]];
if (isNil QGVAR(nextUid)) then {
	GVAR(nextUid) = 1;
	GVAR(data) = [];
	GVAR(highFreq) = [];
};
private _uid = GVAR(nextUid);
GVAR(nextUid) = GVAR(nextUid) + 1;
private _entry = [_name, compileFinal(format ["bsp_counter_%1", _uid]), compileFinal(format ["bsp_elapsed_%1", _uid]), compileFinal(format ["bsp_elapsed_%1=0;bsp_counter_%1=0;", _uid])];
if (_isHighFreq) then {
	GVAR(highFreq) pushBack _entry;
} else {
	GVAR(data) pushBack _entry;
};
missionNamespace setVariable [format ["bsp_fnc_%1", _uid], _code];
missionNamespace setVariable [format ["bsp_counter_%1", _uid], 0];
missionNamespace setVariable [format ["bsp_elapsed_%1", _uid], 0];
if ( _isReturning ) then {
	compileFinal (format ["bsp_counter_%1=bsp_counter_%1 + 1;private _s = diag_tickTime;private _r=_this call bsp_fnc_%1;bsp_elapsed_%1 = bsp_elapsed_%1 + diag_tickTime - _s;_r", _uid])
} else {
	compileFinal (format ["bsp_counter_%1=bsp_counter_%1 + 1;private _s = diag_tickTime;_this call bsp_fnc_%1;bsp_elapsed_%1 = bsp_elapsed_%1 + diag_tickTime - _s;", _uid])
}