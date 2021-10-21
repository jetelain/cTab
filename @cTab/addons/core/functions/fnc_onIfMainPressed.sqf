/*
    Function handling IF_Main keydown event
    Based on player equipment and the vehicle type he might be in, open or close a cTab device as Main interface.
    No Parameters
    Returns TRUE when action was taken (interface opened or closed)
    Returns FALSE when no action was taken (i.e. player has no cTab device / is not in cTab enabled vehicle)

    (previously in player_init.sqf)
*/
if (cTabIfOpenStart) exitWith {false};
_previousInterface = "";
if (cTabUavViewActive) exitWith {
    objNull remoteControl (gunner cTabActUav);
    vehicle cTab_player switchCamera 'internal';
    cTabUavViewActive = false;
    call cTab_fnc_onIfTertiaryPressed;
    true
};
if (!isNil "cTabIfOpen" && {cTabIfOpen select 0 == 0}) exitWith {
    // close Main
    call cTab_fnc_close;
    true
};
if !(isNil "cTabIfOpen") then {
    _previousInterface = cTabIfOpen select 1;
    // close Secondary / Tertiary
    call cTab_fnc_close;
};

_player = cTab_player;
_vehicle = vehicle _player;
_interfaceName = call {
    if ([_player,_vehicle,"TAD"] call cTab_fnc_unitInEnabledVehicleSeat) exitWith {
        cTabPlayerVehicleIcon = getText (configFile/"CfgVehicles"/typeOf _vehicle/"Icon");
        "cTab_TAD_dsp"
    };
    if ([_player,["ItemAndroid"]] call cTab_fnc_checkGear) exitWith {"cTab_Android_dsp"};
    if ([_player,["ItemMicroDAGR"]] call cTab_fnc_checkGear) exitWith {
        cTabMicroDAGRmode = if ([_player,["ItemcTab"]] call cTab_fnc_checkGear) then {0} else {2};
        "cTab_microDAGR_dsp"
    };
    if ([_player,_vehicle,"FBCB2"] call cTab_fnc_unitInEnabledVehicleSeat) exitWith {"cTab_FBCB2_dlg"};
    if ([_player,["ItemcTab"]] call cTab_fnc_checkGear) exitWith {"cTab_Tablet_dlg"};
    // default
    ""
};

if (_interfaceName != "" && _interfaceName != _previousInterface) exitWith {
    // queue the start up of the interface as we might still have one closing down
    [{
        if (isNil "cTabIfOpen") then {
            [_this select 1] call CBA_fnc_removePerFrameHandler;
            (_this select 0) call cTab_fnc_open;
        };
    },0,[0,_interfaceName,_player,_vehicle]] call CBA_fnc_addPerFrameHandler;
    true
};

false