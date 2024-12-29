#include "script_component.hpp"
#include "../defines.hpp"

params ["_uid", ["_deviceInfos", ["cTab\img\android_s7_ca.paa", (452/2048), (713/2048), (1134/2048), (624/2048)]]];

private _data = GVAR(templatesByUid) get _uid;
if ( isNil "_data" ) exitWith {
    WARNING_1("Template with UID '%1' does not exist", _uid);
};

private _display = createDialog  [QGVAR(templateDialog), true];

_deviceInfos params ["_deviceTexture", "_deviceX", "_deviceY", "_deviceWidth", "_deviceHeight"];

private _bgWidth = 1/_deviceWidth;
private _height = _deviceHeight/_deviceWidth*4/3;

private _bg = _display ctrlCreate ["RscPicture", -1];
_bg ctrlSetPosition [ -1 * _deviceX * _bgWidth, -1 * _deviceY * _bgWidth*4/3, _bgWidth, _bgWidth*4/3];
_bg ctrlSetText _deviceTexture;
_bg ctrlCommit 0;

private _back = _display ctrlCreate ["IGUIBack", -1];
_back ctrlSetPosition [ 0, 0, 1, _height];
_back ctrlSetBackgroundColor [0.2, 0.431, 0.647, 1];
_back ctrlCommit 0;

private _posY = 0;
private _targetControl = _display ctrlCreate ["RscControlsGroup", -1];
_targetControl ctrlSetPosition [ 0, 0, 1, _height];
_targetControl ctrlCommit 0;

[_display, _data, _targetControl] call FUNC(generateTemplateUI);
