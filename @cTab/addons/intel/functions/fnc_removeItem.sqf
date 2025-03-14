#include "script_component.hpp"
#include "../defines.hpp"

params ['_id'];

[QGVAR(removeIntel), [player,call cTab_fnc_getPlayerEncryptionKey,parseNumber(_id)]] call CBA_fnc_serverEvent;
