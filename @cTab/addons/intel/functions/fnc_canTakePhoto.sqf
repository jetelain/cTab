#include "script_component.hpp"
if ( cameraView != "GUNNER" ) exitWith {
	false
};
if (cameraOn != player) exitWith {
	// Gunner view of a vehicle or drone
	true
};
if (binocular player == currentWeapon player) exitWith {
	// Looking through binoculars
	true
};
false