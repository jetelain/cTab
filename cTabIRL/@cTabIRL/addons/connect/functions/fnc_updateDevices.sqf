#include "script_component.hpp"

params ['_player'];

private _vehicle = vehicle _player;
private _deviceLevel = 0;
private _vehicleMode = 0;

if ([_player,["ItemcTab", "ItemAndroid"]] call cTab_fnc_checkGear) then {
    _deviceLevel = 3; // BFT + Messaging
} else {
	if ([_player,_vehicle,"TAD"] call cTab_fnc_unitInEnabledVehicleSeat) then {
		_deviceLevel = 2; // BFT
	};
	if ([_player,_vehicle,"FBCB2"] call cTab_fnc_unitInEnabledVehicleSeat) then {
		_deviceLevel = 2; // BFT
	};
};

if ( _vehicle != _player) then {
	if ((_player == driver _vehicle) || { _deviceLevel == 2 || {_player == (_vehicle call cTab_fnc_getCopilot)} }) then {
		_vehicleMode = if (_vehicle isKindOf "Helicopter") then { 2 } else { 1 };
	};
};

if ( GVAR(deviceLevel) != _deviceLevel || { GVAR(vehicleMode) != _vehicleMode }) then {
	GVAR(deviceLevel) = _deviceLevel;
	
	if ( GVAR(vehicleMode) != _vehicleMode ) then {
		// Update PFH speed as EFIS needs more precise data
		private _index = CBA_common_PFHhandles param [GVAR(updatePFH)];
		private _pfh = CBA_common_perFrameHandlerArray select _index;
		private _newDelay = if (_vehicleMode > 1) then { 0.1 } else { 0.25 };
		INFO_2("Update PFH delay from %1 to %2", _pfh select 1, _newDelay);
		_pfh set [1, _newDelay];
	};
	
	GVAR(vehicleMode) = _vehicleMode;
	INFO_2("Devices: Device level to %1, Vehicle mode to %2", _deviceLevel, _vehicleMode);
	"cTabExtension" callExtension ["Devices", [_deviceLevel, ctab_core_useMils, _vehicleMode]];
};
