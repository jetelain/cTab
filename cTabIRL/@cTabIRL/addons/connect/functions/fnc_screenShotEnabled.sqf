#include "script_component.hpp"

params ['_width','_height'];

if ( isNil QGVAR(diaryScreenShotRecord)) then {
	private _keybind = ((["cTab", "photo"] call CBA_fnc_getKeybind) select 5) call CBA_fnc_localizeKey;
	GVAR(diaryScreenShotRecord) = player createDiaryRecord ["cTab", [LLSTRING(screenShotTitle), format [LLSTRING(screenShotDetails),_keybind]]];
};
GVAR(photoRation) = _width / _height;
GVAR(canTakePhoto) = true;