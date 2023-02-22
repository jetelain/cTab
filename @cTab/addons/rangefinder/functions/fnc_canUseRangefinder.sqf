#include "script_component.hpp"

if (!(cameraView in ["GUNNER","GROUP"])) exitWith {
	INFO_1("Wrong camera view: %1",cameraView);
	false
};

// In a vehicle
private _vehicle = vehicle ctab_player;
if (_vehicle != ctab_player ) exitWith {
	private _cargoIndex = _vehicle getCargoIndex ctab_player;
	if (_cargoIndex != -1) exitWith {
		INFO_1("Wrong cargo index: %1", _cargoIndex);
		false
	};
	true
};

// On foot
private _weapon = currentWeapon ctab_player;
if (_weapon in GVAR(binoculars)) exitWith {
	true
};
INFO_1("Wrong weapon: %1", _weapon);
false