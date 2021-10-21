/*
    Function to toggle night mode
    Parameter 0: String of uiNamespace variable for which to toggle nightMode for
    Returns TRUE

    (previously in player_init.sqf)
*/
_displayName = _this select 0;
_nightMode = [_displayName,"nightMode"] call cTab_fnc_getSettings;

if (_nightMode != 2) then {
    if (_nightMode == 0) then {_nightMode = 1} else {_nightMode = 0};
    [_displayName,[["nightMode",_nightMode]]] call cTab_fnc_setSettings;
};

true