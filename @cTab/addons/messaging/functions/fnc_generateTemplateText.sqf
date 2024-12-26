#include "script_component.hpp"
#include "../defines.hpp"

params ["_display", "_commit"];

if (isNil QGVAR(currentTemplateData)) exitWith {
    ["", MSG_TYPE_GENERIC, []]
};

GVAR(currentTemplateData) params ["", "_messageType", "", "", "_lines"];

private _attachement = [];
private _linesText = [];

{
    private _lineIndex = _foreachIndex;
    _x params ["_lineTitle", "", "_fields"];

    private _fieldsText = [];

    {
        private _fieldIndex = _foreachIndex;

        _x params ["_fieldTitle", "", "_fieldType"];

        private _idc = FIELD_IDC(_lineIndex,_fieldIndex); 
        private _label = _display displayCtrl (_idc);
        private _control = _display displayCtrl (_idc + 1);

        switch (_fieldType) do
        {
            case MSG_FIELD_TYPE_STRING:
            case MSG_FIELD_TYPE_NUMBER:
            case MSG_FIELD_TYPE_DATETIME:
            case MSG_FIELD_TYPE_CALLSIGN:
            case MSG_FIELD_TYPE_FREQUENCY:
            case MSG_FIELD_TYPE_GRID:
            { 
                private _text = ctrlText _control;
                if (_text != "") then {
                    _fieldsText pushBack format ["%1%2", _fieldTitle, _text];
                };
            };
            case MSG_FIELD_TYPE_CHECKBOX: 
            {
                if ( cbChecked _control ) then {
                    _fieldsText pushBack _fieldTitle;
                };
            };
        };
    } forEach _fields;
    _linesText pushBack format ["%1: %2", _lineTitle, (_fieldsText joinString " ")];
} forEach _lines;

[_linesText joinString "\n", _messageType, _attachement];