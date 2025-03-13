#include "script_component.hpp"

params [['_key', '']];

private _playerKey = call cTab_fnc_getPlayerEncryptionKey;
if ( _playerKey == '' ) exitWith {
	// Player is not yet kown
};

if ( _key != '' && _key != _playerKey ) exitWith {
	INFO_2("Ignore Intel Feed change (filter='%1' player='%2')",_key,_playerKey);
};

INFO_2("Update Intel Feed (filter='%1' player='%2')",_key,_playerKey);
private _feed = ctab_intel_feeds getOrDefault [_playerKey, [], true];
"cTabExtension" callExtension ["UpdateSideFeed", _feed];
