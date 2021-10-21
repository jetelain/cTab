#include "script_component.hpp"
params ["_pos"];
INFO_1("%1", _pos);
if ( GVAR(gridPrecision) == 1 ) exitWith // 10 m
{
    format ["%1-%2", [(_pos select 0) / 10, 4] call CBA_fnc_formatNumber, [(_pos select 1)/ 10, 4] call CBA_fnc_formatNumber]
};
if ( GVAR(gridPrecision) == 2 ) exitWith // 1 m
{
    format ["%1-%2", [_pos select 0,5] call CBA_fnc_formatNumber, [_pos select 1,5] call CBA_fnc_formatNumber]
};
format ["%1", mapGridPosition _pos] // map default


