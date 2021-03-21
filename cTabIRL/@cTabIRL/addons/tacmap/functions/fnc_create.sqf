#include "script_component.hpp"
if ( GVAR(global) ) then {
	_this call FUNC(createGlobal);
} else {
	_this call FUNC(createLocal);
};
