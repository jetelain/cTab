#include "script_component.hpp"
#include "../defines.hpp"

params ["_fieldType","_messsageType"];

private _result = switch ( _fieldType ) do {
	case MSG_FIELD_TYPE_CALLSIGN: {
		(groupId group player)
	};
	case MSG_FIELD_TYPE_MARKER: {
		[getPosASL player] call EFUNC(core,gridPosition)
	};
	case MSG_FIELD_TYPE_GRID: {
		[getPosASL player] call EFUNC(core,gridPosition)
	};
	default {
		""
	};
};
_result