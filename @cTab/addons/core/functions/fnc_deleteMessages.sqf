#include "script_component.hpp"

params ['_indexes'];

private _playerEncryptionKey = call cTab_fnc_getPlayerEncryptionKey;
private _msgArray = cTab_player getVariable [format ["cTab_messages_%1",_playerEncryptionKey],[]];

// run through the selection backwards as otherwise the indices won't match anymore
for "_i" from (count _indexes) to 0 step -1 do {
    _msgArray deleteAt (_indexes select _i);
};
cTab_player setVariable [format ["cTab_messages_%1",_playerEncryptionKey],_msgArray];

[QGVARMAIN(messagesUpdated)] call CBA_fnc_localEvent;

if (!isNil "cTabIfOpen" && {[cTabIfOpen select 1,"mode"] call cTab_fnc_getSettings == "MESSAGE"}) then {
    call cTab_msg_gui_load;
};