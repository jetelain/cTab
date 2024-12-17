#include "script_component.hpp"
params ['_uri','_data'];
systemChat LLSTRING(screenShotTaken);
INFO_2("Uri=%1 Data=%2",_uri,_data);

[_uri,_data] spawn {  
	// For DEBUG purposes
	params ['_uri','_data'];
	disableSerialization;  
	private _html = findDisplay 46 createDisplay "RscCredits" ctrlCreate ["RscHTML", -1];  
	_html ctrlSetBackgroundColor [0,0,0,0.8];  
	_html ctrlSetPosition [safeZoneX, safeZoneY, safeZoneW, safeZoneH];  
	_html ctrlCommit 0;  
	_html htmlLoad (format ["%1.html?w=%2", _uri, round  (safeZoneW * (getResolution # 5) / pixelW)]);  
};