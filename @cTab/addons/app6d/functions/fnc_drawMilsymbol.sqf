#include "script_component.hpp"

params ["_map", "_sidc", "_position", ["_options", []], ["_size", BASELINE_ICON_SIZE], ["_alpha", 1]];

if (_alpha == 0) exitWith {};

private _images = [_sidc] call FUNC(getSidcImagesCached);
{
	_map drawIcon [_x,[1,1,1,_alpha],_position,_size,_size,0];
} forEach _images;

if ( count _options == 0 ) exitWith{};

_options params [["_commonIdentifier", ""], ["_higherFormation", ""], ["_uniqueDesignation", ""], ["_reinforcedReduced", 0], ["_dateTimeGroup", ""]];

private _textSpacing = _size * 0.6;
private _baseFontHeight = 0.05;
private _fontColor = [0,0,0,_alpha];
//   TahomaBLineZero            TahomaBLineZero
//    TahomaBLineOne +--------+ TahomaBLineOne
//    TahomaBLineTwo |  ICON  | TahomaBLineTwo
//  TahomaBLineThree +--------+ TahomaBLineThree
// 
if ( _reinforcedReduced != 0 ) then {
	_map drawIcon [GVAR(reinforcedReducedIcons) select _reinforcedReduced,[1,1,1,_alpha],_position,_size,_size,0];
};
if ( _commonIdentifier != "" ) then {
	_map drawIcon ["\A3\ui_f\data\map\Markers\System\dummy_ca.paa",_fontColor,_position,_textSpacing,_textSpacing,0,_commonIdentifier,0,3*_baseFontHeight,"TahomaBLineTwo"];
};
if ( _higherFormation != "" ) then {
	_map drawIcon ["\A3\ui_f\data\map\Markers\System\dummy_ca.paa",_fontColor,_position,_textSpacing,_textSpacing,0,_higherFormation,0,3*_baseFontHeight,"TahomaBLineThree"];
};
if ( _uniqueDesignation != "") then {
	_map drawIcon ["\A3\ui_f\data\map\Markers\System\dummy_ca.paa",_fontColor,_position,_textSpacing,_textSpacing,0,_uniqueDesignation,0,3*_baseFontHeight,"TahomaBLineThree","left"];
};
if ( _dateTimeGroup != "") then {
	_map drawIcon ["\A3\ui_f\data\map\Markers\System\dummy_ca.paa",_fontColor,_position,_textSpacing,_textSpacing,0,_dateTimeGroup,0,5*_baseFontHeight,"TahomaBLineZero","left"];
};
