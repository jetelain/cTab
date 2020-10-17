#include "script_component.hpp"

if (!hasInterface) exitWith { };

// Make Arma load the extension
"cTabExtension" callExtension "Warmup";

// Enable logging to rpt file if debug mode
#ifdef DEBUG_MODE_FULL
	"cTabExtension" callExtension "Debug";
#endif

["CBA_settingsInitialized", {

	if ( !GVAR(enabled) ) exitWith {
		INFO("Mod is disabled");
	};

	// Connect to server
	call FUNC(connect);

	// Update loadout each time required
	["loadout", FUNC(updateDevices)] call CBA_fnc_addPlayerEventHandler;
	["unit", FUNC(updateDevices)] call CBA_fnc_addPlayerEventHandler;
	["vehicle", FUNC(updateDevices)] call CBA_fnc_addPlayerEventHandler;

	// Update position 4 times per second
	[FUNC(updatePosition), 0.25] call CBA_fnc_addPerFrameHandler;

	// Update markers each time required
	["ctab_listsUpdated", FUNC(updateMarkers)] call CBA_fnc_addEventHandler;
	["ctab_userMarkerListUpdated", FUNC(updateMarkers)] call CBA_fnc_addEventHandler;
	["ctab_messagesUpdated", FUNC(updateMessages)] call CBA_fnc_addEventHandler;

}] call CBA_fnc_addEventHandler;