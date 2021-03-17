#include "script_component.hpp"
params ['_id', '_x', '_y', '_sideid', '_dashed', '_icon', '_mod1', '_mod2', '_size', '_designation'];

if ( !isNil "mts_markers_fnc_createMarker") then {
	private _marker = [[_x,_y], -1, true, [_sideid, _dashed], [_icon, _mod1, _mod2], [_size, false, false], [], _designation] call mts_markers_fnc_createMarker;
	GVAR(allMetisMarkersLocal) pushBack _marker;
};