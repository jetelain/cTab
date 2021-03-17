#include "script_component.hpp"
params ['_type', '_data'];
_data params ['_id'];
switch (_type) do {
	case "icon": { deleteMarkerLocal format ['_USER_DEFINED #0/tacmap%1/-1', _id]; };
	case "poly": { deleteMarkerLocal format ['_USER_DEFINED #0/tacmap%1/-1', _id]; };
	case "mtis": { };
	default { WARNING_1("Unknown marker type %1", _type); };
};