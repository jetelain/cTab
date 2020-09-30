params ["_interfaceType", "_interfaceName", "_player", "_vehicle"];

if (cTabIfOpenStart) exitWith {false};

if (cTabUavViewActive) then {
	objNull remoteControl (gunner cTabActUav);
	vehicle cTab_player switchCamera 'internal';
	cTabUavViewActive = false;
};

private _previousInterface = "";

if !(isNil "cTabIfOpen") then {
	_previousInterface = cTabIfOpen select 1;
	call cTab_fnc_close;
};

if (_interfaceName != "" && _interfaceName != _previousInterface) exitWith {
	// queue the start up of the interface as we might still have one closing down
	[{
		if (isNil "cTabIfOpen") then {
			[_this select 1] call CBA_fnc_removePerFrameHandler;
			(_this select 0) call cTab_fnc_open;
		};
	},0,[_interfaceType,_interfaceName,_player,_vehicle]] call CBA_fnc_addPerFrameHandler;
	true
};

false