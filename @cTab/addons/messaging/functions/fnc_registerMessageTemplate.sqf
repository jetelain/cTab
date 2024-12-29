#include "script_component.hpp"

params ["_uid", "_messageType", "_title", "_href", "_lines"];

if ( _uid in GVAR(templatesByUid) ) exitWith {
    WARNING_1("Template with UID '%1' already exists", _uid);
};

private _data = [_uid, _messageType, _title, _href, _lines];

GVAR(templatesByUid) set [_uid, _data];
GVAR(templates) pushBack _data;

[QGVAR(templates)] call CBA_fnc_localEvent;

TRACE_2("Registered message template '%1' with UID '%2'", _title, _uid);
