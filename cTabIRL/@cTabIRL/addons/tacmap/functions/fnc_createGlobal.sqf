#include "script_component.hpp"
params ['_type', '_data'];
switch (_type) do {
	case "icon": { _data call FUNC(createIconGlobal); };
	case "poly": { _data call FUNC(createLineGlobal); };
	case "mtis": { _data call FUNC(createMtisGlobal); };
	default { WARNING_1("Unknown marker type %1", _type); };
};
