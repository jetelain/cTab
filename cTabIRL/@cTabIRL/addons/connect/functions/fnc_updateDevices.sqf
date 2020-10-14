#include "script_component.hpp"

params ['_player'];

private _vehicle = vehicle _player;
private _deviceLevel = 0;

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

if ( GVAR(deviceLevel) != _deviceLevel ) then {
	GVAR(deviceLevel) = _deviceLevel;
	INFO_1("Device level changed to %1", _deviceLevel);

	"cTabExtension" callExtension ["Devices", _deviceLevel, ctab_core_useMils];
};
