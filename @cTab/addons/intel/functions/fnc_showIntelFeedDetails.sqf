#include "script_component.hpp"
#include "../defines.hpp"
params ["_display","_intelId"];

private _entry = GVAR(intels) getOrDefault [_intelId, []];

if ( count _entry == 0 ) exitWith { WARNING_1("Intel %1 not found",_intelId); };

(_entry select 4) params ['_uri'];

// Display cannot pass to the spawned function, so we store it in a global variable
uiNamespace setVariable ["ctab_intel_show_display", _display];

GVAR(selectedIntel) = _entry;

[_uri] spawn {  
	params ['_uri'];
	disableSerialization;  
	// Consume variable
	private _display = uiNamespace getVariable "ctab_intel_show_display";
	uiNamespace setVariable ["ctab_intel_show_display", displayNull];
	// Load the HTML
	private _html = _display displayCtrl IDC_INTELFEED_HTMLVIEW;  
	private _targetHeight = ((ctrlPosition _html) select 2);
	private _h = round  (_targetHeight * (getResolution # 5) / pixelH);
	_html htmlLoad format ["%1.html?h=%2",_uri,_h];  
	if (!ctrlHTMLLoaded _html) then {
		WARNING_3("HTML not loaded for %1: %2.html?h=%3",_displayID,_uri,_h);
	};
};
