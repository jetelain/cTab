#include "script_component.hpp"

if ( GVAR(global) ) then {
	call FUNC(clearGlobal);
} else {
	call FUNC(clearLocal);
};