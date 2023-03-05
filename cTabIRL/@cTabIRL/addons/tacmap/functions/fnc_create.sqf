#include "script_component.hpp"
params ['_type', '_data'];
switch (_type) do {
	case "icon": { _data call FUNC(createIcon); };
	case "poly": { _data call FUNC(createLine); };
	case "mtis": { _data call FUNC(createMtis); };
	default { WARNING_1("Unknown marker type %1", _type); };
};
