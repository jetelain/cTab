#include "script_component.hpp"
#include "../defines.hpp"

params ["_list"];

lbClear _list;
{
	_x params ['_id', '_type','_pos','_dateTime'];
	_dateTime params ['','','', '_hours','_minutes'];
	private _index = _list lbAdd format ["%1, %2%3:%4%5", [_pos] call EFUNC(core,gridPosition), if (_hours<10) then {"0"} else {""}, _hours, if (_minutes<10) then {"0"} else {""}, _minutes];
	_list lbSetData [_index, format["%1",_id]];
} forEachReversed GVAR(feed);
