#include "script_component.hpp"

params ['_display', '_posX', '_height', '_idc', '_distance'];

private _ctrl = _display displayCtrl _idc;
_ctrl ctrlSetPositionX _posX-(_height*0.35);
_ctrl ctrlCommit 0;

_ctrl = _display displayCtrl (_idc+1);
_ctrl ctrlSetPositionX _posX+(_height*0.35);
_ctrl ctrlSetText (format ['%1m', round _distance]);
_ctrl ctrlCommit 0;
