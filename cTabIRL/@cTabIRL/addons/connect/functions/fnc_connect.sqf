#include "script_component.hpp"

if ( GVAR(key) == "" ) then {
	private _str = (ceil random 1000000) toFixed 0;
	_str = ['000000', _str] joinString ''; 
	private _key = _str select [count _str - 6, 6] ;
	[QGVAR(key), _key, 0, "client", true] call CBA_settings_fnc_set;
};

INFO_2("Connect to %1 with key %2", GVAR(uri), GVAR(key));

// Connects to server
"cTabExtension" callExtension ["Connect", [GVAR(uri), getPlayerUID player, profileName, GVAR(key)]];

// Send mission data
"cTabExtension" callExtension ["StartMission", [worldName, worldSize, date]];

// Setup initial loadout
call FUNC(updateDevices);