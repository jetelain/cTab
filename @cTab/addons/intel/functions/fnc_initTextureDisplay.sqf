#include "script_component.hpp"

params ["_display"];

private _displayID = displayUniqueName _display; 
private _intelId = parseNumber(_displayID select [count GVAR(texturePrefix)]);
private _entry = GVAR(intels) getOrDefault [_intelId, []];

if ( count _entry == 0 ) exitWith { WARNING_2("Intel %1 not found (display %2)",_intelId,_displayID); };

INFO_3("Intel %1: %2 (display %3)",_intelId,_entry,_displayID);
(_entry select 4) params ['_uri'];

[_displayID, _uri] spawn {  
	params ['_displayID', '_uri'];
	disableSerialization;  
	private _display = findDisplay _displayID;
	private _h = round  (1 * (getResolution # 5) / pixelH);
	private _html = _display ctrlCreate ["RscHTML", -1];  
	_html ctrlSetBackgroundColor [0,0,0,0.8];  
	_html ctrlSetPosition [0, 0, 1, 1];  
	_html ctrlCommit 0;  
	_html htmlLoad format ["%1.html?h=%2",_uri,_h];  
	if (!ctrlHTMLLoaded _html) then {
		WARNING_3("HTML not loaded for %1: %2.html?h=%3",_displayID,_uri,_h);
	};
	displayUpdate _display;
};
