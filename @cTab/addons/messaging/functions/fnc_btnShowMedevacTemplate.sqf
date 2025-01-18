#include "script_component.hpp"
#include "../defines.hpp"

params ["_button"];

private _index = GVAR(templates) findIf { (_x select 1) == MSG_TYPE_MEDEVAC };
if ( _index == -1 ) exitWith {
	// TODO: Failover to a builtin message template
	WARNING("No MEDEVAC template found");
};

[ctrlParent _button, GVAR(templates) select _index] call FUNC(showTemplateUI);
