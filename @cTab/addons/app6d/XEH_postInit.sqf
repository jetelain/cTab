#include "script_component.hpp"

plop = ["common identifier", "higher formation", "unique designation", 1];

// Only for tests
(findDisplay 12 displayCtrl 51) ctrlAddEventHandler ["draw", { 
	params ["_ctrl"];
	[_ctrl, "10031000131211050051", player, plop] call FUNC(drawMilsymbol);
}];