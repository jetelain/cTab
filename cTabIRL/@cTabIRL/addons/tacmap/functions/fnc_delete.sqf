#include "script_component.hpp"

if ( GVAR(global) ) then {
	_this call FUNC(deleteGlobal);
}
else { 
	_this call FUNC(deleteLocal);
};