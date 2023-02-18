/*
 	Name: cTab_fnc_checkGear
 	
 	Author(s):
		GrueArbre

 	Description:
		Check a units gear for certain items
	
	Parameters:
		0: OBJECT - Unit object to check gear on
		1: ARRAY  - Array of item names to search for
 	
 	Returns:
		BOOLEAN - If at least one of the items passed in parameter 1 was found or not
 	
 	Example:
		_playerHasCtabItem = [player,["ItemcTab","ItemAndroid","ItemMicroDAGR"]] call cTab_fnc_checkGear;
*/
params ["_unit","_items"]; 
count ((assignedItems _unit) arrayIntersect _items) > 0 || { 
  count ((items _unit) arrayIntersect _items) > 0
} 