#include "script_component.hpp"
#include "../defines.hpp"

params ["_button", ["_messageSent", false]];

private _display = ctrlParent _button;

closeDialog ([2, 1] select _messageSent);
