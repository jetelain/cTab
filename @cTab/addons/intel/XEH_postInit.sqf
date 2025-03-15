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
		private _playerKey = call cTab_fnc_getPlayerEncryptionKey;
		if ( _playerKey != _key ) exitWith {};

		if ( count _added > 0 ) then {
			GVAR(feed) append _added;
			// DEBUG
			debugboard setObjectTexture [0, ([(_added select -1) select 0] call FUNC(getTextureDisplay))];
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
