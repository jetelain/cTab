#include "script_component.hpp"

{
	deleteMarker (_y # 0);
} forEach GVAR(allIconMarkers);
GVAR(allIconMarkers) = createHashMap;

{
	deleteMarker (_y # 0);
} forEach GVAR(allLineMarkers);
GVAR(allLineMarkers) = createHashMap;

{
	[_y # 0] call mts_markers_fnc_deleteMarker;
} forEach GVAR(allMetisMarkers);
GVAR(allMetisMarkers) = createHashMap;
