#include "script_component.hpp"

params ["_control"];

INFO('INIT');

GVAR(nextIdc) = 10000;
GVAR(markerToIdc) = createHashMap;
GVAR(markersHaveChanged) = true;
GVAR(acousticChanged) = EGVAR(acousticdetector,isActive);
GVAR(previousPosition) = [0,0,0];

private _bg1 = (_control controlsgroupctrl 9001) controlsGroupCtrl 9101;
private _bg2 = (_control controlsgroupctrl 9002) controlsGroupCtrl 9101;
_bg1 ctrlSetText ([QPATHTOF(data\ns_deg_ca.paa),QPATHTOF(data\ns_ca.paa)] select ctab_core_useMils);
_bg2 ctrlSetText ([QPATHTOF(data\sn_deg_ca.paa),QPATHTOF(data\sn_ca.paa)] select ctab_core_useMils);
_bg1 ctrlcommit 0;
_bg2 ctrlcommit 0;

private _blocs = [];
private _bloc = controlNull;
for "_idc" from 9200 to 9271 do {
	_blocs pushBack (_control controlsGroupCtrl _idc);
};
uiNamespace setVariable [QGVAR(blocs), _blocs];
