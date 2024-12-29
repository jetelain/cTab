#include "script_component.hpp"
#include "../defines.hpp"

params ["_control"];

private _plrList = playableUnits;

if (_plrList isEqualTo []) then {
	// since playableUnits will return an empty array in single player, add the player if array is empty
	_plrList pushBack cTab_player
};

private _validSides = call cTab_fnc_getPlayerSides;

lbClear _control;

{
	if ((side _x in _validSides) && {isPlayer _x} && {[_x, EGVAR(core,leaderDevices)] call cTab_fnc_checkGear}) then {
		private _index = _control lbAdd format ["%1:%2 (%3)", groupId group _x, groupId _x, name _x];
		_control lbSetData [_index,str _x];
	};
} forEach _plrList;

lbSort [_control, "ASC"];
