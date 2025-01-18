#include "script_component.hpp"
#include "../defines.hpp"

params ["_display"];

private _result = [_display] call FUNC(generateTemplateText);

_result params ["", "_text", "", "_attachements"];

(_display displayCtrl IDC_TEXTPREVIEW) ctrlSetText _text;

private _control = _display displayCtrl IDC_ATTACHEMENTS;

lbClear _control;
{
	_x params ["", "_title", ""];
	_control lbAdd _title;
} forEach _attachements;
