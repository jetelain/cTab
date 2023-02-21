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

	FUNC(markerFilter) = { params ["_name"]; !([_name] call EFUNC(tacmap,isTacMapMarker)) };

	if (!isNil "mts_markers_fnc_isMtsMarker") then {
		// Metis Marker are ignored as they won't be displayed on web app, this can help to reduce messages size
		FUNC(markerFilter) = { params ["_name"]; !([_name] call EFUNC(tacmap,isTacMapMarker)) && { ([_name] call mts_markers_fnc_isMtsMarker) == 0 } };
	};

	// Get devices from ctab, if mod is up-to-date
	if ( !isNil "ctab_core_leaderDevices" ) then {
		GVAR(trackDevices) = ctab_core_leaderDevices;
	};

	// Connect to server
	call FUNC(connect);

	GVAR(nextMPU) = diag_tickTime + 1.5; // Next Marker Position Update if real time tracking is enabled

	// Update loadout each time required
	["loadout", FUNC(updateDevices)] call CBA_fnc_addPlayerEventHandler;
	["unit", FUNC(updateDevices)] call CBA_fnc_addPlayerEventHandler;
	["vehicle", FUNC(updateDevices)] call CBA_fnc_addPlayerEventHandler;

	// Update position 4 times per second
	GVAR(updatePFH) = [FUNC(updatePosition), 0.25] call CBA_fnc_addPerFrameHandler;

	// Update markers each time required
	["ctab_listsUpdated", FUNC(updateMarkers)] call CBA_fnc_addEventHandler;
	["ctab_userMarkerListUpdated", FUNC(updateMarkers)] call CBA_fnc_addEventHandler;
	["ctab_messagesUpdated", FUNC(updateMessages)] call CBA_fnc_addEventHandler;

	addMissionEventHandler ["MarkerCreated", {
		params ['_name'];
		if ( [_name] call FUNC(markerFilter) ) then {
			GVAR(mapMarkersNeedsUpdate) = true;
		};
	}];
	addMissionEventHandler ["MarkerUpdated", {
		params ['_name'];
		if ( [_name] call FUNC(markerFilter) ) then {
			GVAR(mapMarkersNeedsUpdate) = true;
		};
	}];
	addMissionEventHandler ["MarkerDeleted", {
		params ['_name'];
		if ( [_name] call FUNC(markerFilter) ) then {
			GVAR(mapMarkersNeedsUpdate) = true;
		};
	}];

}] call CBA_fnc_addEventHandler;

["ctab_rangefinder_data", {
	params ["_position", "_distance"];
	"cTabExtension" callExtension ["ActionRangeFinder", _position + [_distance]];
}] call CBA_fnc_addEventHandler;
