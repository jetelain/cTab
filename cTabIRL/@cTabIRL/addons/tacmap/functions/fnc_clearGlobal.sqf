#include "script_component.hpp"

{
	deleteMarker _x;
} forEach GVAR(allMarkersGlobal);
GVAR(allMarkersGlobal) = [];

{
	[_y] call mts_markers_fnc_deleteMarker
} forEach GVAR(allMetisMarkersGlobal);
GVAR(allMetisMarkersGlobal) = createHashMap;
