#include "script_component.hpp"
#include "..\ui\technicaldata_defines.hpp"

params ["_display","_exitcode"];

if ( _exitcode == 1 ) then {
    disableserialization;

    if ( (leader cTab_player) == cTab_player ) then {
        private _currentGroup = (group cTab_player);
        private _currentCurrentGroupId = (groupId _currentGroup);
        private _callSignEdit = _display displayctrl IDC_TECHNICALDATA_CALLSIGN;
        private _newCurrentGroupId = ctrlText _callSignEdit;
        if ( _newCurrentGroupId != _currentCurrentGroupId ) then {
            _currentGroup setGroupIdGlobal [_newCurrentGroupId];
        };
    };
};