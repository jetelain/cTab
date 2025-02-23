#include "script_component.hpp"

params ["_sidc"];

if ( count _sidc < 20 ) exitWith { [] };

private _result = GVAR(cache)  getOrDefaultCall [_sidc, { [_sidc] call FUNC(getSidcImages) }, true];

if ( (count GVAR(cache)) > 1000 ) then {
  GVAR(cache) = createHashMap;
  GVAR(cache) set [_sidc, _result];
};

_result
