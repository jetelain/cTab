#include "script_component.hpp"

params ["_uid", "_messageType", "_title", "_shortTitle", "_href", "_lines"];

if( GVAR(isBuiltinTemplates) ) then {
    GVAR(templates) = [];
    GVAR(isBuiltinTemplates) = false;  
};

private _data = [_uid, _messageType, _title, _shortTitle, _href, _lines];

GVAR(templates) pushBack _data;

[QGVAR(templates)] call CBA_fnc_localEvent;

TRACE_2("Registered message template '%1' with UID '%2'", _title, _uid);
