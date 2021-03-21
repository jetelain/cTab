#include "script_component.hpp"
params ['_value'];

if (_value != GVAR(global)) then {

	GVAR(global) = _value;
	if ( _value ) then {
		// Transform to GLOBAL
		call FUNC(clearLocal); // FIXME: this is a placeholder, we should transform markers
	}
	else {
		//  Transform to LOCAL
		call FUNC(clearGlobal); // FIXME: this is a placeholder, we should transform markers
	};

};