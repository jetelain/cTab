#include "script_component.hpp"
#include "defines.hpp"

if ( isServer ) then {
	GVAR(serverFeeds) = createHashmap;
	GVAR(nextId) = 1;

	// DEBUG
	// GVAR(serverFeeds) set ["b", [[1,1,[6333.07,707.478,5],[2035,5,28,13,37],["http://localhost:5000/Image/76561198081226363xbBq-zdmC-AHoXCPN5fahonsqF1n-fmYz",true,353.075,[109.237,79.3909],[[6254.34,748.076,5],[6399.78,765.741,5],[6379.86,683.347,5],[6293.41,672.846,5]],[6338.61,661.818,72.8007]]],[2,1,[6286.53,717.896,5],[2035,5,28,13,37],["http://localhost:5000/Image/76561198081226363xdGl1I62zQO1BOFMhVFYKKj2siZbylW2n",true,317.128,[136.299,140.814],[[6131.77,710.948,5],[6304.94,871.714,5],[6349.26,720.713,5],[6279.07,655.55,5]],[6338.58,661.826,72.801]]]]];
	// GVAR(nextId) = 10;

	// A player has taken a photo
	[QGVAR(addIntel), {
		params ['_player', '_key', '_entryData'];
		private _feed = GVAR(serverFeeds) getOrDefault [_key, [], true];
		private _entry = [GVAR(nextId)];
		_entry append _entryData;
		_feed pushBack _entry;
		GVAR(nextId) = GVAR(nextId) + 1;
		[QGVAR(feedUpdate), [_key, [_entry], []]] call CBA_fnc_globalEvent;
	}] call CBA_fnc_addEventHandler;

	// A player has requeted to remove an intel
	[QGVAR(removeIntel), {
		params ['_player', '_key', '_id'];
		private _feed = GVAR(serverFeeds) getOrDefault [_key, [], true];
		private _idx = _feed findIf { (_x select 0) == _id };
		if ( _idx != -1 ) then {
			_feed deleteAt _idx;
			[QGVAR(feedUpdate), [_key, [], [_id]]] call CBA_fnc_globalEvent;
		};
	}] call CBA_fnc_addEventHandler;

	// A player has connected, and need the existing feed
	[QGVAR(requestFeed), {
		params ['_player', '_key'];
		private _feed = GVAR(serverFeeds) getOrDefault [_key, [], true];
		[QGVAR(feedUpdate), [_key, _feed, []], _player] call CBA_fnc_targetEvent;
	}] call CBA_fnc_addEventHandler;
};

if (hasInterface) then {
	// Server has updated the feed
	[QGVAR(feedUpdate), {
		params ['_key','_added','_removedIds'];
		// Keep track of all intels (to allow texture to work for all players)
		{
			GVAR(intels) set [(_x select 0), _x];
		} forEach _added;

		private _playerKey = call cTab_fnc_getPlayerEncryptionKey;
		if ( _playerKey != _key ) exitWith {};

		if ( count _added > 0 ) then {
			GVAR(feed) append _added;
			// DEBUG
			// debugboard setObjectTexture [0, ([(_added select -1) select 0] call FUNC(getTextureDisplay))];
		};
		if ( count _removedIds > 0 ) then {
			{
				private _id = _x;
				private _idx = GVAR(feed) findIf { (_x select 0) == _id };
				if ( _idx != -1 ) then {
					GVAR(feed) deleteAt _idx;
				};
			} forEach _removedIds;
		};
		[QGVAR(feed)] call CBA_fnc_localEvent;
	}] call CBA_fnc_addEventHandler;

	// Request server to send existing feed
	[QGVAR(requestFeed), [player, call cTab_fnc_getPlayerEncryptionKey]] call CBA_fnc_serverEvent;

	// Controlled unit changed, get unit side feed
	["ctab_player", {
		GVAR(feed) = [];
		[QGVAR(requestFeed), [player, call cTab_fnc_getPlayerEncryptionKey]] call CBA_fnc_serverEvent;
	}] call CBA_fnc_addEventHandler;
};
