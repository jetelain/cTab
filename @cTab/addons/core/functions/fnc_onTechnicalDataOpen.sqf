#include "script_component.hpp"
#include "..\ui\technicaldata_defines.hpp"

params ["_display"];

disableserialization;

private _currentGroup = (group cTab_player);

private _callSignEdit = _display displayctrl IDC_TECHNICALDATA_CALLSIGN;
_callSignEdit ctrlSetText (groupId _currentGroup);
