#include "script_component.hpp"

params ["_target"];

private _actions = [];
private _player = cTab_player;
private _vehicle = vehicle _player;
private _hasItem = false;

private _subactions = {
    params ["_target","","_params"];
    _params params ["_type", "_dlg", "_player", "_vehicle"];
    [
        [[
            "dsp",
            LLSTRING(DisplayHide),
            "",
            {
                params ["", "", "_params"];
                _params call FUNC(toggleInterface);
            },
            {true},
            _subactions,
            [0, [_dlg, "_dlg", "_dsp"] call CBA_fnc_replace, _player, _vehicle]
        ] call ace_interact_menu_fnc_createAction, [], _target]
    ]
};

private _toggleInterface = {
    params ["", "", "_params"];
    _params call FUNC(toggleInterface);
};

if ([_player,_vehicle,"TAD"] call cTab_fnc_unitInEnabledVehicleSeat) then {
    _actions pushBack [[
        "cTab_TAD",
        LLSTRING(ConfigureTAD),
        "",
        _toggleInterface,
        {true},
        _subactions,
        [1, "cTab_TAD_dlg", _player, _vehicle]
    ] call ace_interact_menu_fnc_createAction, [], _target];
    _hasItem = true;
};

if ([_player,["ItemAndroid"]] call cTab_fnc_checkGear) then {
    _actions pushBack [[
        "cTab_Android",
        LLSTRING(ConfigureAndroid),
        "\cTab\img\icon_Android.paa",
        _toggleInterface,
        {true},
        _subactions,
        [1, "cTab_Android_dlg", _player,_vehicle]
    ] call ace_interact_menu_fnc_createAction, [], _target];
    _hasItem = true;
};

if ([_player,["ItemMicroDAGR"]] call cTab_fnc_checkGear) then {
    _actions pushBack [[
        "cTab_microDAGR",
        LLSTRING(ConfigureMicroDAGR),
        "\cTab\img\icon_MicroDAGR.paa",
        _toggleInterface,
        {true},
        _subactions,
        [1, "cTab_microDAGR_dlg", _player,_vehicle]
    ] call ace_interact_menu_fnc_createAction, [], _target];
};

if ([_player,_vehicle,"FBCB2"] call cTab_fnc_unitInEnabledVehicleSeat) then {
    _actions pushBack [[
        "cTab_FBCB2",
        LLSTRING(FBCB2),
        "",
        _toggleInterface,
        {true},
        {},
        [1, "cTab_FBCB2_dlg", _player,_vehicle]
    ] call ace_interact_menu_fnc_createAction, [], _target];
    _hasItem = true;
};

if ([_player,["ItemcTab"]] call cTab_fnc_checkGear) then {
    _actions pushBack [[
        "cTab_Tablet",
        LLSTRING(Tablet),
        "\cTab\img\icon_dk10.paa",
        _toggleInterface,
        {true},
        {},
        [1, "cTab_Tablet_dlg", _player,_vehicle]
    ] call ace_interact_menu_fnc_createAction, [], _target];
    _hasItem = true;
};

if (_hasItem && {(leader _player) == _player}) then {

    _actions pushBack [[
        "cTab_TechData",
        LLSTRING(techData),
        "",
        { createDialog "cTab_TechnicalData_dlg"; },
        {true},
        {},
        []
    ] call ace_interact_menu_fnc_createAction, [], _target];

};

_actions