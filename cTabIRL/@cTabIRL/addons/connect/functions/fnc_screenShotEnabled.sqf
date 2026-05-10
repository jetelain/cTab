#include "script_component.hpp"

params ['_width','_height'];

if (!(isNil QGVAR(diaryRecord))) then {
	private _keybind = ((["cTab", "photo"] call CBA_fnc_getKeybind) select 5) call CBA_fnc_localizeKey;
	private _add = format [LLSTRING(screenShotDetails),_keybind];

	private _newContent = +GVAR(diaryContent);
	_newContent set [1, format["%1<br/><br/>%2",_newContent select 1,_add]];
	player setDiaryRecordText [["cTab", GVAR(diaryRecord)], _newContent];
};

GVAR(photoRation) = _width / _height;
GVAR(canTakePhoto) = true;
