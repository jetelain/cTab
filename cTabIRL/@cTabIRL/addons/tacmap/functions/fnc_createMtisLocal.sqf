#include "script_component.hpp"
params ['_id', '_x', '_y', '_sideid', '_dashed', '_icon', '_mod1', '_mod2', '_size', '_designation'];

if ( !isNil "mts_markers_fnc_createMarker") then {
	private _key = format ['%1', _id];
	private _existing = GVAR(allMetisMarkersLocal) get _key;
	if ( !isNil "_existing" ) then {
		[_existing] call mts_markers_fnc_deleteMarker;
	};
	private _marker = [[_x,_y], -1, true, [_sideid, _dashed], [_icon, _mod1, _mod2], [_size, false, false], [], _designation] call mts_markers_fnc_createMarker;
	GVAR(allMetisMarkersLocal) set [_key, _marker];
};