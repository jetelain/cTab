#include "script_component.hpp"

params ["_uid", "_messageType", "_title", "_href", "_lines"];

if ( _uid in GVAR(templatesByUid) ) exitWith {
    WARNING_1("Template with UID '%1' already exists", _uid);
};

private _data = [_uid, _messageType, _title, _href, _lines];
// lines => [[ "title", "description", [[ "field title", "field description", "field type" ]] ]]

GVAR(templatesByUid) set [_uid, _data];
GVAR(templates) pushBack _data;

[QGVAR(templates)] call CBA_fnc_localEvent;

