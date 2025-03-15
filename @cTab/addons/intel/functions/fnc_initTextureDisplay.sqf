#include "script_component.hpp"

params ["_display"];

private _displayID = displayUniqueName _display; 
private _intelId = parseNumber(_displayID select [count GVAR(texturePrefix)]);
private _idx = GVAR(feed) findIf { (_x select 0) == _intelId };

INFO_3("Intel %1 found at index %2, display %3",_intelId,_idx,_displayID);

if ( _idx == -1 ) exitWith { INFO_2("Intel %1 not found: %2",_intelId,GVAR(feed)); };

(GVAR(feed) select _idx) params ["","","","","_data"];
_data params ['_uri'];

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
