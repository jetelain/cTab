#include "script_component.hpp"

{
	deleteMarker _x;
} forEach GVAR(allMarkersLocal);
GVAR(allMarkersLocal) = [];

{
	[_x] call mts_markers_fnc_deleteMarker
} forEach GVAR(allMetisMarkersLocal);
GVAR(allMetisMarkersLocal) = [];
