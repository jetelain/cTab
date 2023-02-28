#include "script_component.hpp"

private _isActive = false;

if ( GVAR(enable) ) then {

	private _vehicle = vehicle ctab_player;
	if ( _vehicle != ctab_player) then {
		private _cargoIndex = _vehicle getCargoIndex ctab_player;
		if (_cargoIndex == -1) then {
			_isActive = true; // TODO : check vehicle has acoustic detector
		};
	};

};

TRACE_3("updateActiveState", GVAR(enable), _isActive, GVAR(isActive));

if ( _isActive != GVAR(isActive) ) then {
	if ( _isActive ) then {
		if ( GVAR(pfhHandle) == -1 ) then {
			LOG("Acoustic gunshot detector is started");
			GVAR(pfhHandle) = [FUNC(simulatePFH)] call CBA_fnc_addPerFrameHandler;
		};
	}
	else
	{
		if ( GVAR(pfhHandle) != -1 ) then {
			LOG("Acoustic gunshot detector is stopped");
			[GVAR(pfhHandle)] call CBA_fnc_removePerFrameHandler;
			GVAR(pfhHandle) = -1;
		};
    };
};
