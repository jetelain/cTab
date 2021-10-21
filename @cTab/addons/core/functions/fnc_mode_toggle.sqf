
/*
    Function to toggle mode
    Parameter 0: String of uiNamespace variable for which to toggle mode for
    Returns TRUE

    (previously in player_init.sqf)
*/
_displayName = _this select 0;
_mode = [_displayName,"mode"] call cTab_fnc_getSettings;

call {
    if (_displayName == "cTab_Android_dlg") exitWith {
        call {
            if (_mode != "BFT") exitWith {_mode = "BFT"};
            _mode = "MESSAGE";
        };
    };
};
[_displayName,[["mode",_mode]]] call cTab_fnc_setSettings;
true