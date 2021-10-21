/*
    Function handling Zoom_In keydown event
    If supported cTab interface is visible, decrease map scale
    Returns TRUE when action was taken
    Returns FALSE when no action was taken (i.e. no interface open, or unsupported interface)

    (previously in player_init.sqf)
*/
if (cTabIfOpenStart || (isNil "cTabIfOpen")) exitWith {false};
_displayName = cTabIfOpen select 1;
if !([_displayName] call cTab_fnc_isDialog) exitWith {
    _mapScale = ([_displayName,"mapScaleDsp"] call cTab_fnc_getSettings) / 2;
    _mapScaleMin = [_displayName,"mapScaleMin"] call cTab_fnc_getSettings;
    if (_mapScale < _mapScaleMin) then {
        _mapScale = _mapScaleMin;
    };
    _mapScale = [_displayName,[["mapScaleDsp",_mapScale]]] call cTab_fnc_setSettings;
    true
};
false