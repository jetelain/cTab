#include "script_component.hpp"
params ['_type', '_data'];
_data params ['_id'];
switch (_type) do {
	case "icon": { 
		private _existing = GVAR(allIconMarkers) get _id;
		if ( !isNil "_existing" ) then {
			deleteMarker (_existing # 0); 
			GVAR(allIconMarkers) deleteAt _id;
		};
	};
	case "poly": { 
		private _existing = GVAR(allLineMarkers) get _id;
		if ( !isNil "_existing" ) then {
			deleteMarker (_existing # 0); 
			GVAR(allLineMarkers) deleteAt _id;
		};
	};
	case "mtis": { 
		private _existing = GVAR(allMetisMarkers) get _id;
		if ( !isNil "_existing" ) then {
			[_existing # 0] call mts_markers_fnc_deleteMarker;
			GVAR(allMetisMarkers) deleteAt _id;
		};
	};
	default { WARNING_1("Unknown marker type %1", _type); };
};