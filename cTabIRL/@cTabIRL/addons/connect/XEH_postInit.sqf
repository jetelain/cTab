#include "script_component.hpp"

// Make Arma load the extension
"cTabExtension" callExtension "Warmup";

// Enable logging to rpt file if debug mode
#ifdef DEBUG_MODE_FULL
	"cTabExtension" callExtension "Debug";
#endif

// Connects to server
"cTabExtension" callExtension ["Connect", ["http://localhost:5000/hub", getPlayerUID player, profileName, "Key"]];

// Send mission data
"cTabExtension" callExtension ["StartMission", [worldName, worldSize, date]];

// Setup initial loadout
call FUNC(updateDevices);

// Update loadout each time required
["loadout", FUNC(updateDevices)] call CBA_fnc_addPlayerEventHandler;
["unit", FUNC(updateDevices)] call CBA_fnc_addPlayerEventHandler;
["vehicle", FUNC(updateDevices)] call CBA_fnc_addPlayerEventHandler;

// Update position 4 times per second
[FUNC(updatePosition), 0.25] call CBA_fnc_addPerFrameHandler;

// Update markers each time required
["ctab_listsUpdated", FUNC(updateMarkers)] call CBA_fnc_addEventHandler;
["ctab_userMarkerListUpdated", FUNC(updateMarkers)] call CBA_fnc_addEventHandler;
