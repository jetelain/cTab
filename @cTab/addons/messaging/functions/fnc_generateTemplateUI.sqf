#include "script_component.hpp"
#include "../defines.hpp"

params ["_display", "_uid"];

private _data = GVAR(templatesByUid) get _uid;
if ( isNil "_data" ) exitWith {
    WARNING_1("Template with UID '%1' does not exist", _uid);
};

GVAR(currentTemplateData) = _data;

_data params ["", "_messageType", "_title", "", "_lines"];

private _posY = 0;
private _targetControl = controlNull;

{
    private _lineIndex = _foreachIndex;
    _x params ["_lineTitle", "_lineDescription", "_fields"];

    private _ctrl = _display ctrlCreate [QGVAR(lineTitle), -1, _targetControl];
    _ctrl ctrlSetPosition [ 0, _posY, 10 * GRID_W, GRID_H ];
    _ctrl ctrlSetText _lineTitle;
    _ctrl ctrlCommit 0;

    _ctrl = _display ctrlCreate [QGVAR(lineDescription), -1, _targetControl];
    _ctrl ctrlSetPosition [10 * GRID_W, _posY, 90 * GRID_W, GRID_H ];
    _ctrl ctrlSetText _lineDescription;
    _ctrl ctrlCommit 0;

    _posY = _posY + GRID_H;

    if ( count _fields > 0 ) then {
        private _posX = 0;

        {
            private _fieldIndex = _foreachIndex;
            private _idc = FIELD_IDC(_lineIndex,_fieldIndex); 

            _x params ["_fieldTitle", "_fieldDescription", "_fieldType"];

            _ctrl = _display ctrlCreate [QGVAR(fieldTitle), _idc, _targetControl];
            _ctrl ctrlSetPosition [ _posX, _posY, 10 * GRID_W, GRID_H * 2];
            _ctrl ctrlSetText _fieldTitle;
            _ctrl ctrlCommit 0;
            _posX = _posX + GRID_W * 11;

            switch (_fieldType) do
            {
                case MSG_FIELD_TYPE_STRING:
                case MSG_FIELD_TYPE_NUMBER:
                case MSG_FIELD_TYPE_DATETIME:
                case MSG_FIELD_TYPE_CALLSIGN:
                case MSG_FIELD_TYPE_FREQUENCY:
                case MSG_FIELD_TYPE_GRID:
                { 
                    _ctrl = _display ctrlCreate [QGVAR(fieldDescription), _idc + 2, _targetControl];
                    _ctrl ctrlSetPosition [ _posX, _posY, 30 * GRID_W, GRID_H ];
                    _ctrl ctrlSetText _fieldDescription;
                    _ctrl ctrlCommit 0;

                    _ctrl = _display ctrlCreate [QGVAR(fieldText), _idc + 1, _targetControl];
                    _ctrl ctrlSetPosition [ _posX, _posY+GRID_H, 30 * GRID_W, GRID_H ];
                    _ctrl ctrlSetText "";
                    _ctrl ctrlCommit 0;
                    _posX = _posX + GRID_W * 31;
                };
                case MSG_FIELD_TYPE_CHECKBOX: 
                {
                    _ctrl = _display ctrlCreate [QGVAR(fieldCheckbox), _idc + 1, _targetControl];
                    _ctrl ctrlSetPosition [ _posX, _posY, 2 * GRID_W, GRID_H * 2 ];
                    _ctrl ctrlSetText _fieldDescription;

                    _ctrl ctrlCommit 0;
                    _posX = _posX + GRID_W * 3;
                 };
            };

        } forEach _fields;

        _posY = _posY + (GRID_H * 2);
    };


} forEach _lines;

