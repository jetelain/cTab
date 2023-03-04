#include "script_component.hpp"
params ["_sqfFile","_name","_isReturning","_isHighFreq"];
private _fnc = [compileFinal preprocessFileLineNumbers _sqfFile, _name, _isReturning, _isHighFreq] call FUNCMAIN(instrumentFunction);
missionNamespace setVariable [_name, _fnc];
_fnc