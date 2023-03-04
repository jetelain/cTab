#include "script_component.hpp"
params [["_function", {}, [{}]], ["_name", "", [""]], ["_delay", 0, [0]], ["_args", []]];
if (_delay<2) then {
	[[_function,_name,false,true] call FUNCMAIN(instrumentFunction),_delay,_args] call CBA_fnc_addPerFrameHandler
} else {
	[[_function,_name,false,false] call FUNCMAIN(instrumentFunction),_delay,_args] call CBA_fnc_addPerFrameHandler
}