#include "script_component.hpp"
#include "../defines.hpp"

params ["_data", "_recipients"];

_data params ["_title", "_message", "_messageType", "_attachments"];

// Use legacy function to send message (will be replaced sometime in the future)
[_message, _recipients] call EFUNC(core,sendMessage);

// Transform attachement of type grid to markers
{
	_x params ["_attachementType"];
	if ( _attachementType == MSG_ATTACHMENT_GRID) then {
		_x params ["", "", "_center"];
		private _key = call cTab_fnc_getPlayerEncryptionKey;
		private _markerData = [_center,if ( _messageType ==  MSG_TYPE_MEDEVAC) then { 209 } else { 100 },0,0, (call cTab_fnc_currentTime) + " " + _title,cTab_player];
		[_key, _markerData] call cTab_fnc_addUserMarker;
	};
} forEach _attachments;
