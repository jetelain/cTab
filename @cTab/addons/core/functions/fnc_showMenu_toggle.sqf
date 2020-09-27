/*
	Function to toggle showMenu
	Parameter 0: String of uiNamespace variable for which to toggle showMenu for
	Returns TRUE

	(previously in player_init.sqf)
*/
_displayName = _this select 0;
_showMenu = [_displayName,"showMenu"] call cTab_fnc_getSettings;
_showMenu = !_showMenu;
[_displayName,[["showMenu",_showMenu]]] call cTab_fnc_setSettings;
true