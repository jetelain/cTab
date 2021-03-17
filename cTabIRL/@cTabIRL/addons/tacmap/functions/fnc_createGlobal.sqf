#include "script_component.hpp"
params ['_type', '_data'];
switch (_type) do {
	case "icon": { _data call FUNC(createIconGlobal); };
	case "line": { _data call FUNC(createLineGlobal); };
	case "mtis": { _data call FUNC(createMtisGlobal); };
};