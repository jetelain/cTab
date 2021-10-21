/*
    Function handling Zoom_Out keydown event
    If supported cTab interface is visible, increase map scale
    Returns TRUE when action was taken
    Returns FALSE when no action was taken (i.e. no interface open, or unsupported interface)

    (previously in player_init.sqf)
*/
if (cTabIfOpenStart || (isNil "cTabIfOpen")) exitWith {false};
_displayName = cTabIfOpen select 1;
if !([_displayName] call cTab_fnc_isDialog) exitWith {
    _mapScale = ([_displayName,"mapScaleDsp"] call cTab_fnc_getSettings) * 2;
    _mapScaleMax = [_displayName,"mapScaleMax"] call cTab_fnc_getSettings;
    if (_mapScale > _mapScaleMax) then {
        _mapScale = _mapScaleMax;
    };
    _mapScale = [_displayName,[["mapScaleDsp",_mapScale]]] call cTab_fnc_setSettings;
    true
};
false