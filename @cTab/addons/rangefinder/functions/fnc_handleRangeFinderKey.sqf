#include "script_component.hpp"

if (!(call FUNC(canUseRangefinder))) exitWith { };

private _camPosition = AGLToASL positionCameraToWorld [0, 0, 1];
private _aimLinePos = AGLToASL positionCameraToWorld [0, 0, 5000];
private _LIS = lineIntersectsSurfaces [_camPosition, _aimLinePos];
private _position = ((_LIS select 0) select 0);
private _distance = _camPosition vectorDistance _position;

[QGVAR(data), [_position, _distance]] call CBA_fnc_localEvent;