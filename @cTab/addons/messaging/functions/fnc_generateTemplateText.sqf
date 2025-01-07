#include "script_component.hpp"
#include "../defines.hpp"

params ["_display"];

if (isNil QGVAR(currentTemplateData)) exitWith {
    ["", MSG_TYPE_GENERIC, []]
};

GVAR(currentTemplateData) params ["", "_messageType", "", "_shortTitle", "", "_lines"];

private _attachement = [];
private _linesText = [];
private _shortTitle = "";
private _persistValues = [];

{
    private _lineIndex = _foreachIndex;
    _x params ["_lineTitle", "", "_fields"];

    if (_lineIndex == 0) then {
        _shortTitle = _lineTitle;
    };

    private _fieldsText = [];
    private _persistFields = [];

    {
        private _fieldIndex = _foreachIndex;

        _x params ["_fieldTitle", "_fieldDescription", "_fieldType"];

        private _idc = FIELD_IDC(_lineIndex,_fieldIndex); 
        private _label = _display displayCtrl (_idc);
        private _control = _display displayCtrl (_idc + 1);
        private _desc = _display displayCtrl (_idc + 2);

        if ( _fieldType == MSG_FIELD_TYPE_CHECKBOX ) then
        {
            private _checked = cbChecked _control;
            _persistFields pushBack _checked;
            if ( _checked ) then {
                _fieldsText pushBack _fieldTitle;
                _label ctrlSetBackgroundColor [0.1, 0.21, 0.32, 1];
                _control ctrlSetBackgroundColor [0.1, 0.21, 0.32, 1];
            } else {
                _label ctrlSetBackgroundColor [0, 0, 0, 0];
                _control ctrlSetBackgroundColor [0, 0, 0, 0];
            };
        }
        else
        {
            private _text = ctrlText _control;
            _persistFields pushBack _text;
            if (_text != "") then {
                _fieldsText pushBack format ["%1%2", _fieldTitle, _text];
                _label ctrlSetBackgroundColor [0.1, 0.21, 0.32, 1];
                _desc ctrlSetText "";
                if ( _fieldType == MSG_FIELD_TYPE_MARKER ) then {
                    private _parsed=[_text] call FUNC(parseGridPosition);
                    if ( count _parsed == 2 ) then {
                        _parsed params ["_pos", "_precision"];
                        private _center = [(_pos # 0) + ((_precision # 0) / 2),(_pos # 1) + ((_precision # 1) / 2)];
                        _attachement pushBack [MSG_ATTACHMENT_MARKER, format [LLSTRING(MarkerAt), [_center] call EFUNC(core,gridPosition)], _center, _pos, _precision];
                    }; 
                };
            } else {
                _label ctrlSetBackgroundColor [0, 0, 0, 0];
                _desc ctrlSetText _fieldDescription;
            };
        };
    } forEach _fields;
    _linesText pushBack format ["%1: %2", _lineTitle, (_fieldsText joinString " ")];
    _persistValues pushBack _persistFields;
} forEach _lines;

GVAR(currentTemplateValues) = _persistValues;

[_shortTitle, _linesText joinString endl, _messageType, _attachement];