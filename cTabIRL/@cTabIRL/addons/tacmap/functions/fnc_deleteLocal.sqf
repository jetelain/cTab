#include "script_component.hpp"
params ['_type', '_data'];
_data params ['_id'];
switch (_type) do {
	case "icon";
	case "poly": { 
		private _name = format ['_USER_DEFINED #0/tacmap%1/-1', _id];
		deleteMarker _name; 
		GVAR(allMarkersLocal) deleteAt (GVAR(allMarkersLocal) find _name); 
	};
	case "mtis": { 	
		private _key = format ['%1', _id];
		private _existing = GVAR(allMetisMarkersLocal) get _key;
		if ( !isNil "_existing" ) then {
			[_existing] call mts_markers_fnc_deleteMarker;
			GVAR(allMetisMarkersLocal) deleteAt _key;
		};
	};
	default { WARNING_1("Unknown marker type %1", _type); };
};