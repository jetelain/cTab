#include "script_component.hpp"

params ['_display', '_ctrlBar', '_posX', '_height', '_idc', '_markerData', '_distance'];

_markerData params ['', '_texture1', '_texture2', '', '_color'];

private _ctrl = _display ctrlCreate [ "RscPicture", _idc, _ctrlBar];
_ctrl ctrlSetPosition [_posX-(_height*0.35), _height * 0.3, _height * 0.7, _height * 0.7];
_ctrl ctrlSetText _texture1;
_ctrl ctrlSetTextColor _color;
_ctrl ctrlCommit 0;

_ctrl = _display ctrlCreate [ "RscText", _idc+1, _ctrlBar];
_ctrl ctrlSetPosition [_posX+(_height*0.35), _height * 0.6, _height, _height * 0.4];
_ctrl ctrlSetText (format ['%1m', round _distance]);
_ctrl ctrlSetTextColor cTabColorRed;
_ctrl ctrlCommit 0;
