#include "script_component.hpp"

params ["_heading"];

if(GVAR(useMils)) then {
    [_heading * 6400 / 360] call CBA_fnc_formatNumber
}
else {
    format ["%1°",[_heading,3] call CBA_fnc_formatNumber]
}
