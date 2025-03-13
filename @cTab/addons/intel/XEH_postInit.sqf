#include "script_component.hpp"
#include "defines.hpp"

if ( isServer ) then {
	GVAR(serverFeeds) = createHashmap;
	GVAR(nextId) = 1;

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

	// A player has connected, and need the existing feed
	[QGVAR(requestFeed), {
		params ['_player', '_key'];
		private _feed = GVAR(serverFeeds) getOrDefault [_key, [], true];
		if ( count _feed > 0 ) then {
			[QGVAR(feedUpdate), [_key, _feed, []], cursorTarget] call CBA_fnc_targetEvent;
		};
	}] call CBA_fnc_addEventHandler;
};

if (hasInterface) then {
	// Server has updated the feed
	[QGVAR(feedUpdate), {
		params ['_key','_added','_removedIds'];
		private _feed = GVAR(feeds) getOrDefault [_key, [], true];
		_feed append _added;
		if ( count _removedIds > 0 ) then {
			_feed = _feed select { !((_x select 0) in _removedIds) };
		};
		[QGVAR(feeds), _key] call CBA_fnc_localEvent;
	}] call CBA_fnc_addEventHandler;

	// Request server to send existing feed
	[QGVAR(requestFeed), [player, call cTab_fnc_getPlayerEncryptionKey]] call CBA_fnc_serverEvent;
};
