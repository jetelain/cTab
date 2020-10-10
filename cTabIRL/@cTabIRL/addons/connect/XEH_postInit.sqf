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


["ctab_listsUpdated", {
	// UpdateMarkers
	private _data = [];

	{
		_data pushBack ['veh', [_x select 0] call FUNC(getId), _x select 1, _x select 2, _x select 3, _x select 4, _x select 5, _x select 6];
	} forEach cTabBFTvehicles;

	{
		private _leader = (_x select 0);
		private _vehicle = vehicle _leader;
		private _vehicleId = '';
		if ( _vehicle != _leader ) then {
			_vehicleId = [vehicle (_x select 0)] call FUNC(getId);
		};
		_data pushBack ['grp', [group _leader] call FUNC(getId), _x select 1, _x select 2, _x select 3, _x select 4, _x select 5, 0, _vehicleId];
	} forEach cTabBFTgroups;

	"cTabExtension" callExtension ["UpdateMarkers", _data];
}] call CBA_fnc_addEventHandler;