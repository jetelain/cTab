#include "script_component.hpp"

params ["_map", "_sidc", "_position", ["_options", []], ["_size", 96]];

private _images = [_sidc] call FUNC(getSidcImagesCached);
{
	_map drawIcon [_x,[1,1,1,1],_position,_size,_size,0];
} forEach _images;

if ( count _options == 0 ) exitWith{};

_options params [["_commonIdentifier", ""], ["_higherFormation", ""], ["_uniqueDesignation", ""], ["_reinforcedReduced", 0]];

private _textSpacing = _size * 0.6;
//
//    TahomaBLineOne +--------+ TahomaBLineOne
//    TahomaBLineTwo |  ICON  | TahomaBLineTwo
//  TahomaBLineThree +--------+ TahomaBLineThree
// 
if ( _reinforcedReduced != 0 ) then {
	_map drawIcon [GVAR(reinforcedReducedIcons) select _reinforcedReduced,[1,1,1,1],_position,_size,_size,0];
};
if ( _commonIdentifier != "" ) then {
	_map drawIcon ["\A3\ui_f\data\map\Markers\System\dummy_ca.paa",[1,1,1,1],_position,_textSpacing,_textSpacing,0,_commonIdentifier,0,0.15,"TahomaBLineTwo"];
};
if ( _higherFormation != "" ) then {
	_map drawIcon ["\A3\ui_f\data\map\Markers\System\dummy_ca.paa",[1,1,1,1],_position,_textSpacing,_textSpacing,0,_higherFormation,0,0.15,"TahomaBLineThree"];
};
if ( _uniqueDesignation != "") then {
	_map drawIcon ["\A3\ui_f\data\map\Markers\System\dummy_ca.paa",[1,1,1,1],_position,_textSpacing,_textSpacing,0,_uniqueDesignation,0,0.15,"TahomaBLineThree","left"];
};
