#include "script_component.hpp"
params ['_type', '_data'];
switch (_type) do {
	case "icon": { _data call FUNC(createIconLocal); };
	case "poly": { _data call FUNC(createLineLocal); };
	case "mtis": { _data call FUNC(createMtisLocal); };
	default { WARNING_1("Unknown marker type %1", _type); };
};
