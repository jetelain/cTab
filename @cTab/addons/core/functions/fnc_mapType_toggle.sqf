/*
    Function to toggle mapType to the next one in the list of available map types
    Parameter 0: String of uiNamespace variable for which to toggle to mapType for
    Returns TRUE

    (previously in player_init.sqf)
*/
_displayName = _this select 0;
_mapTypes = [_displayName,"mapTypes"] call cTab_fnc_getSettings;
_currentMapType = [_displayName,"mapType"] call cTab_fnc_getSettings;
_currentMapTypeIndex = [_mapTypes,_currentMapType] call BIS_fnc_findInPairs;
if (_currentMapTypeIndex == count _mapTypes - 1) then {
    [_displayName,[["mapType",_mapTypes select 0 select 0]]] call cTab_fnc_setSettings;
} else {
    [_displayName,[["mapType",_mapTypes select (_currentMapTypeIndex + 1) select 0]]] call cTab_fnc_setSettings;
};
true