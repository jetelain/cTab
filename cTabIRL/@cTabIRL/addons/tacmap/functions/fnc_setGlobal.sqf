#include "script_component.hpp"
params ['_value'];

if ( _value isEqualType true ) then {
	// Backward compatibility
	_value == if ( _value ) then { 0 } else { -1 };
};

if (_value != GVAR(channel)) then {
	GVAR(channel) = _value;

	private _previousIcons = GVAR(allIconMarkers);
	private _previousLines = GVAR(allLineMarkers);
	private _previousMetis = GVAR(allMetisMarkers);

	call FUNC(clear); 

	{
		(_y # 1) call FUNC(createIcon);
	} forEach _previousIcons;

	{
		(_y # 1) call FUNC(createLine);
	} forEach _previousLines;

	{
		(_y # 1) call FUNC(createMtis);
	} forEach _previousMetis;

};