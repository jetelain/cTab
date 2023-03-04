#include "script_component.hpp"
if (isNil QGVAR(data)) exitWith {};
INFO("Performance STD");
{
	_x params ['_name','_counterFnc','_elapsedFnc'];
	private _counter = call _counterFnc;
	private _elapsed = (call _elapsedFnc) * 1000;
	if ( _counter > 0) then {
		INFO_4("%1;%2;%3;%4",_name, _counter, _elapsed, _elapsed / _counter);
	};
} forEach GVAR(data);
