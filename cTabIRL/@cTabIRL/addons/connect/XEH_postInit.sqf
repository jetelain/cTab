#include "script_component.hpp"

// Make Arma load the extension
private _start = diag_tickTime;	
"cTabExtension" callExtension "Warmup";
INFO_1("%1 elapsed in callExtension",diag_tickTime - _start);

// Enable logging to rpt file if debug mode
#ifdef DEBUG_MODE_FULL
	_start = diag_tickTime;	
	INFO("cTabExtension Debug");
	"cTabExtension" callExtension "Debug";
	INFO_1("%1 elapsed in callExtension",diag_tickTime - _start);
#endif

// Connects to server
_start = diag_tickTime;	
"cTabExtension" callExtension ["Connect", ["http://localhost:5000/hub", getPlayerUID player, profileName, "Key"]];
INFO_1("%1 elapsed in callExtension",diag_tickTime - _start);

// Send mission data
_start = diag_tickTime;	
"cTabExtension" callExtension ["StartMission", [worldName, worldSize, date]];
INFO_1("%1 elapsed in callExtension",diag_tickTime - _start);

// Update position 4 times per second
[{
	private _vehicle = vehicle player;
	private _data = getPosASL player;
	_data pushBack direction player;
	_data pushBack date;
	_data pushBack ([group player] call FUNC(getId));
	if ( _vehicle != player ) then {
		_data pushBack ([_vehicle] call FUNC(getId));
	};

	private _start = diag_tickTime;	
	"cTabExtension" callExtension ["UpdatePosition", _data];
	INFO_1("%1 elapsed in callExtension",diag_tickTime - _start);
}, 0.25] call CBA_fnc_addPerFrameHandler;

["ctab_listsUpdated", FUNC(updateMarkers)] call CBA_fnc_addEventHandler;
["ctab_userMarkerListUpdated", FUNC(updateMarkers)] call CBA_fnc_addEventHandler;
