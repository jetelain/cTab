#include "script_component.hpp"

if ( GVAR(key) == "" ) then {
	private _str = (ceil random 1000000) toFixed 0;
	_str = ['000000', _str] joinString ''; 
	private _key = _str select [count _str - 6, 6] ;
	[QGVAR(key), _key, 0, "client", true] call CBA_settings_fnc_set;
};

INFO_1("Connect to %1", GVAR(uri));

// Connects to server
"cTabExtension" callExtension ["Connect", [GVAR(uri), getPlayerUID player, profileName, GVAR(key)]];

private _infos = [
	worldName, 
	worldSize, 
	date,
	getText (configFile >> "CfgPatches" >> "ctab_main" >> "versionStr"),
	getText (configFile >> "CfgPatches" >> "ctab_irl_main" >> "versionStr")];

INFO_1("StartMission %1", _infos);

// Send mission data
"cTabExtension" callExtension ["StartMission", _infos];

// Setup initial loadout
call FUNC(updateDevices);