#include "script_component.hpp"
params ['_type', '_data'];
switch (_type) do {
	case "icon": { _data call FUNC(createIconLocal); };
	case "line": { _data call FUNC(createLineLocal); };
	case "mtis": { _data call FUNC(createMtisLocal); };
};