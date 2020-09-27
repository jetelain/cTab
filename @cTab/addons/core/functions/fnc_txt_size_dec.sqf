/*
	fnc to decrease icon and text size

	(previously in player_init.sqf)
 */
if (cTabTxtFctr > 1) then {cTabTxtFctr = cTabTxtFctr - 1};
call cTab_fnc_update_txt_size;