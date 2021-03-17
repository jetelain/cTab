#include "script_component.hpp"
params ['_type', '_data'];
_data params ['_id'];
switch (_type) do {
	case "icon": { deleteMarkerLocal format ['_USER_DEFINED #0/tacmap%1/-1', _id]; };
	case "line": { deleteMarkerLocal format ['_USER_DEFINED #0/tacmap%1/-1', _id]; };
	case "mtis": { };
};