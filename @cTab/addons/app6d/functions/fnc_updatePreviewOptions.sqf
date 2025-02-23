#include "script_component.hpp"
params ["_ctrl"];
private _display = ctrlParent _ctrl;
GVAR(previewOptions) set [0, ctrlText (_display displayCtrl 9900)];
GVAR(previewOptions) set [1, ctrlText (_display displayCtrl 9901)];
GVAR(previewOptions) set [2, ctrlText (_display displayCtrl 9902)];
GVAR(previewOptions) set [3, lbCurSel (_display displayCtrl 9903)];
GVAR(previewOptions) set [4, ctrlText (_display displayCtrl 9904)];