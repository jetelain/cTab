/*
	Function to toggle text next to BFT icons
	Parameter 0: String of uiNamespace variable for which to toggle showIconText for
	Returns TRUE

	(previously in player_init.sqf)
*/
_displayName = _this select 0;
if (cTabBFTtxt) then {cTabBFTtxt = false} else {cTabBFTtxt = true};
[_displayName,[["showIconText",cTabBFTtxt]]] call cTab_fnc_setSettings;
true