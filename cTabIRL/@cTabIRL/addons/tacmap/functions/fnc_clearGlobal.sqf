#include "script_component.hpp"

{
	deleteMarker _x;
} forEach GVAR(allMarkersGlobal);
GVAR(allMarkersGlobal) = [];

{
	[_x] call mts_markers_fnc_deleteMarker
} forEach GVAR(allMetisMarkersGlobal);
GVAR(allMetisMarkersGlobal) = [];
