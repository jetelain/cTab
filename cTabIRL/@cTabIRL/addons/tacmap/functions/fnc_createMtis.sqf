#include "script_component.hpp"
params ['_id', '_x', '_y', '_sideid', '_dashed', '_icon', '_mod1', '_mod2', '_size', '_designation'];

if ( !isNil "mts_markers_fnc_createMarker") then {
	private _existing = GVAR(allMetisMarkers) get _id;
	if ( !isNil "_existing" ) then {
		[_existing # 0] call mts_markers_fnc_deleteMarker;
	};
	private _marker = [[_x,_y], GVAR(channel), true, [[_sideid, _dashed], [_icon, _mod1, _mod2], [_size, false, false], [], _designation]] call mts_markers_fnc_createMarker;
	GVAR(allMetisMarkers) set [_id, [_marker, _this]];
};