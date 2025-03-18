#include "script_component.hpp"

if (!hasInterface) exitWith {};

["ctab_userMarkerListUpdated", { GVAR(markersHaveChanged) = true; }] call CBA_fnc_addEventHandler;
[QEGVAR(acousticdetector,update), { GVAR(acousticChanged) = true; }] call CBA_fnc_addEventHandler;
