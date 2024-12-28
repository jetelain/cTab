#include "script_component.hpp"
#include "../defines.hpp"

params ["_display", "_data", "_targetControl"];

GVAR(currentTemplateData) = _data;

_data params ["", "_messageType", "_title", "", "_lines"];

private _maxWidth = ((1 - SCROLLBAR_WIDTH) / GRID_W) - 2; 

{
    private _lineIndex = _foreachIndex;
    _x params ["_lineTitle", "_lineDescription", "_fields"];

    private _ctrl = _display ctrlCreate ["RscText", -1, _targetControl];
    _ctrl ctrlSetPosition [ 0, _posY, GRID_W * ( _maxWidth + 2), 6 * GRID_H ];
    _ctrl ctrlSetText format ["%1: %2", _lineTitle, _lineDescription];
    _ctrl ctrlCommit 0;

    _posY = _posY + (GRID_H * 6);

    if ( count _fields > 0 ) then {
        private _posX = GRID_W;
        private _gridX = 0;

        {
            private _fieldIndex = _foreachIndex;
            private _idc = FIELD_IDC(_lineIndex,_fieldIndex); 

            _x params ["_fieldTitle", "_fieldDescription", "_fieldType"];

            if ( _fieldType == MSG_FIELD_TYPE_CHECKBOX ) then
            {
                private _wantedWidth = (count _fieldTitle + count _fieldDescription + 2 ) * WIDTH_PER_CHAR + 9;

                if ( _gridX + _wantedWidth > _maxWidth ) then {
                    _gridX = GRID_W;
                    _posX = 0;
                    _posY = _posY + (GRID_H * 9);
                };

                _ctrl = _display ctrlCreate ["RscText", _idc, _targetControl];
                _ctrl ctrlSetPosition [ _posX, _posY, GRID_W * _wantedWidth, GRID_H * 8];
                _ctrl ctrlCommit 0;

                _ctrl = _display ctrlCreate ["RscCheckBox", _idc + 1, _targetControl];
                _ctrl ctrlSetPosition [ _posX, _posY, GRID_W * 8, GRID_H * 8 ];
                _ctrl ctrlAddEventHandler ["CheckedChanged", { [ctrlParent (_this select 0)] call FUNC(updateMessagePreview); }];
                _ctrl ctrlCommit 0;

                _ctrl = _display ctrlCreate ["RscText", _idc + 2, _targetControl];
                _ctrl ctrlSetPosition [ _posX + (8 * GRID_W), _posY, GRID_W * (_wantedWidth - 8), GRID_H * 8 ];
                _ctrl ctrlSetText format ["%1 : %2", _fieldTitle, _fieldDescription];
                _ctrl ctrlCommit 0;

                _gridX = _gridX + _wantedWidth + 1;
                _posX = GRID_W * (_gridX+1);
            } 
            else
            { 
                private _labelWidth = 0;
                private _fieldWidth = (((count _fieldDescription) * WIDTH_PER_CHAR) min 40) max 10;

                if ( count _fieldTitle > 0 ) then {
                    _labelWidth = (((count _fieldTitle) * WIDTH_PER_CHAR) + 2) max 5;
                };

                if ( _fieldType != MSG_FIELD_TYPE_NUMBER ) then {
                    if ( count _fields < 3 ) then {
                        _fieldWidth = ((_maxWidth - (count _fields) + 1) / (count _fields)) - _labelWidth;
                    } else {
                        if ( count _fields == _fieldIndex + 1 ) then {
                            _fieldWidth = _fieldWidth max (_maxWidth - _labelWidth - _gridX);
                        };
                    };
                };

                private _wantedWidth = _labelWidth + _fieldWidth;
                if ( _gridX + _wantedWidth > _maxWidth ) then {
                    _gridX = GRID_W;
                    _posX = 0;
                    _posY = _posY + (GRID_H * 9);
                };

                _ctrl = _display ctrlCreate ["RscText", _idc, _targetControl];
                _ctrl ctrlSetPosition [ _posX, _posY, GRID_W * _wantedWidth, GRID_H * 8];
                _ctrl ctrlSetText _fieldTitle;
                _ctrl ctrlCommit 0;

                _ctrl = _display ctrlCreate ["RscText", _idc + 2, _targetControl];
                _ctrl ctrlSetPosition [ _posX+(GRID_W*_labelWidth), _posY, GRID_W * _fieldWidth, GRID_H * 8 ];
                _ctrl ctrlSetText _fieldDescription;
                _ctrl ctrlSetTextColor [1, 1, 1, 0.75];
                _ctrl ctrlCommit 0;

                _ctrl = _display ctrlCreate ["RscEdit", _idc + 1, _targetControl];
                _ctrl ctrlSetPosition [ _posX+(GRID_W*(_labelWidth+0.5)), _posY+(GRID_H*0.5), GRID_W * (_fieldWidth-1), GRID_H * 7 ];
                _ctrl ctrlSetText ([_fieldType] call FUNC(getDefaultFieldValue));

                _ctrl ctrlAddEventHandler ["KeyUp", { [ctrlParent (_this select 0)] call FUNC(updateMessagePreview); }];
                _ctrl ctrlCommit 0;

                _gridX = _gridX + _wantedWidth + 1;
                _posX = GRID_W * (_gridX+1);
            };
        } forEach _fields;
        _posY = _posY + (GRID_H * 9);
    };

    _ctrl = _display ctrlCreate ["RscLine", -1, _targetControl];
    _ctrl ctrlSetPosition [ 0, _posY + GRID_H, GRID_W * ( _maxWidth + 2), 0 ];
    _ctrl ctrlCommit 0;
    _posY = _posY + (2*GRID_H); 

} forEach _lines;

private _middle = GRID_W * (_maxWidth / 2);

_ctrl = _display ctrlCreate [QGVAR(templateFooter), -1, _targetControl];
_ctrl ctrlSetPosition [0, _posY];
_ctrl ctrlCommit 0;

[(_display displayCtrl IDC_RECIPIENTS)] call FUNC(fillRecipientList);

[_display] call FUNC(updateMessagePreview);