/*
	Name: cTab_fnc_dirTo
	
	Author(s):
		GrueArbre

	Description:
		Returns compass direction (horizontal) from first position to second position
	
	Obsolete:
		Use _pos1 getDir _pos2 instead.

	Parameters:
		0: ARRAY - 2D or 3D position
		1: ARRAY - 2D or 3D position
	
	Returns:
		FLOAT - Distance
	
	Example:
		[getPosATL player, [0,0,0]] call cTab_fnc_dirTo;
*/

params ["_pos1","_pos2"];

_pos1 getDir _pos2