#include "script_component.hpp"

if (!hasInterface) exitWith {};

GVAR(caliberTypeCache) = createHashMap;
GVAR(shotsToProcess) = [];
GVAR(detectedShots) = [];
GVAR(distanceLimitPerCaliber) = [250,      750,       350,      550,      1000,     1350,     1650,      2400];
GVAR(caliberLabel) =            ["Rocket", "Missile", "5.56mm", "7.62mm", "12.7mm", "14.5mm", "20-40mm", "90mm+"];
GVAR(nextShotId) = 1;
GVAR(isActive) = false;
GVAR(pfhHandle) = -1;
GVAR(isInitialized) = false;

["CBA_settingsInitialized", FUNC(applySettings)] call CBA_fnc_addEventHandler;
