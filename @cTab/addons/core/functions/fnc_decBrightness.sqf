/*
	Function to decrease brightness
	Parameter 0: String of uiNamespace variable for which to decreas brightness for
	Returns TRUE

	(previously in player_init.sqf)
*/
_displayName = _this select 0;
_brightness = [_displayName,"brightness"] call cTab_fnc_getSettings;
_brightness = _brightness - 0.1;
// make sure brightness is not larger than 0.5
if (_brightness < 0.5) then {_brightness = 0.5};
[_displayName,[["brightness",_brightness]]] call cTab_fnc_setSettings;

true