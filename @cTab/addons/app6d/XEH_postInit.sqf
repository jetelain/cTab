#include "script_component.hpp"

// Only for tests
(findDisplay 12 displayCtrl 51) ctrlAddEventHandler ["draw", { 
	params ["_ctrl"];
	[_ctrl, "10031000131211050051", player] call FUNC(drawSidc);
}];