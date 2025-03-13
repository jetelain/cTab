#include "script_component.hpp"
#include "../defines.hpp"

params ['_uri','_data'];

INFO_2("Uri=%1 Data=%2",_uri,_data);

private _entryData = [
	INTEL_TYPE_IMG,
	_data select 0, // "Cross air" pointed position
	_data select 1, // In-game date time
	[
		_uri, // Image URI
		_data select 2, // Image can be diplayed on map without significant distortion
		_data select 3, // Image direction
		_data select 4, // Image size
		_data select 5, // Area on map (to be able to draw polygon)
		_data select 6  // Camera position
	]
];

INFO_1("Add %1",_entryData);

[QGVAR(addIntel), [player, call cTab_fnc_getPlayerEncryptionKey,_entryData]] call CBA_fnc_serverEvent;
