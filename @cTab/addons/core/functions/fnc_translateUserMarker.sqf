/*
	Name: cTab_fnc_translateUserMarker
	
	Author(s):
		Gundy

	Description:
		Take the condensed user marker data and translate it so that it can be drawn much quicker
		
		Received marker data format:
			Index 0: ARRAY   - 2D marker position
			Index 1: INTEGER - number of icon
			Index 2: INTEGER - size type
			Index 3: INTEGER - octant of reported movement
			Index 4: STRING  - marker time
			Index 5: OBJECT  - marker creator
			
		Translated marker data format:
			Index 0: ARRAY  - marker position
			Index 1: STRING - path to marker icon
			Index 2: STRING - path to marker size icon
			Index 3: STRING - direction of reported movement
			Index 4: ARRAY  - marker color
			Index 5: STRING - marker time
			Index 6: STRING - text alignment
			Index 7: NUMBER - draw size factor (1 by default)
	
	Parameters:
		0: ARRAY - Marker data
	
	Returns:
		ARRAY - Translated marker data
	
	Example:
		[[1714.35,5716.82],0,0,0,"12:00"] call cTab_fnc_translateUserMarker;
*/

private ["_pos","_markerIcon","_texture1","_markerSize","_texture2","_markerDir","_dir","_text","_align"];

_pos = _this select 0;
_text = _this select 4;
_color = cTabColorRed;
_markerIcon = _this select 1;
_drawSize = 1;
_texture1 = call {
	// RED
	if (_markerIcon == 0) exitWith {"\A3\ui_f\data\map\markers\nato\o_inf.paa"};
	if (_markerIcon == 1) exitWith {"\A3\ui_f\data\map\markers\nato\o_mech_inf.paa"};
	if (_markerIcon == 2) exitWith {"\A3\ui_f\data\map\markers\nato\o_motor_inf.paa"};
	if (_markerIcon == 3) exitWith {"\A3\ui_f\data\map\markers\nato\o_armor.paa"};
	if (_markerIcon == 4) exitWith {"\A3\ui_f\data\map\markers\nato\o_air.paa";};
	if (_markerIcon == 5) exitWith {"\A3\ui_f\data\map\markers\nato\o_plane.paa"};
	if (_markerIcon == 6) exitWith {"\A3\ui_f\data\map\markers\nato\o_unknown.paa"};
	if (_markerIcon == 7) exitWith {"\cTab\img\o_inf_rifle.paa"};
	if (_markerIcon == 8) exitWith {"\cTab\img\o_inf_mg.paa"};
	if (_markerIcon == 9) exitWith {"\cTab\img\o_inf_at.paa"};
	if (_markerIcon == 10) exitWith {"\cTab\img\o_inf_mmg.paa"};
	if (_markerIcon == 11) exitWith {"\cTab\img\o_inf_mat.paa"};
	if (_markerIcon == 12) exitWith {"\cTab\img\o_inf_mmortar.paa"};
	if (_markerIcon == 13) exitWith {"\cTab\img\o_inf_aa.paa"};
	if (_markerIcon == 80) exitWith {"\cTab\img\mil\10061500002101000000.paa"}; // ENI Mine
	if (_markerIcon == 81) exitWith {"\cTab\img\mil\10061500002104000000.paa"}; // ENI IED
	if (_markerIcon == 90) exitWith {_drawSize = 1.5; "\cTab\img\tic.paa"}; // Troop in contact

	// GREEN
	_color = cTabColorGreen;
	if (_markerIcon == 20) exitWith {"\A3\ui_f\data\map\markers\military\join_CA.paa"};
	if (_markerIcon == 21) exitWith {"\A3\ui_f\data\map\markers\military\circle_CA.paa"};
	if (_markerIcon == 22) exitWith {"\A3\ui_f\data\map\mapcontrol\Hospital_CA.paa"};
	if (_markerIcon == 23) exitWith {"\A3\ui_f\data\map\markers\military\warning_CA.paa"};

	// BLUE
	_color = cTabColorBlue;
	if (_markerIcon == 30) exitWith {"\A3\ui_f\data\map\markers\nato\b_hq.paa"};
	if (_markerIcon == 31) exitWith {"\A3\ui_f\data\map\markers\military\end_CA.paa"};
	
	// Generic icons with label A-F (label is encoded in _markerSize)
	if (_markerIcon == 100) exitWith {"\A3\ui_f\data\map\markers\military\dot_CA.paa"};
	if (_markerIcon == 101) exitWith {"\cTab\img\m_circle.paa"};

	// BLACK due to image, text will appear blue

	// Control measure markers
	// At scale
	if (_markerIcon == 102) exitWith {"\cTab\img\mil\10032500001305000000.paa"}; // Contact
	if (_markerIcon == 103) exitWith {"\cTab\img\mil\10032500001306000000.paa"}; // Coordinating
	if (_markerIcon == 104) exitWith {"\cTab\img\mil\10032500001601000000.paa"}; // Outpost
	if (_markerIcon == 105) exitWith {"\cTab\img\mil\10032500001602050000.paa"}; // Combat outpost
	if (_markerIcon == 106) exitWith {"\cTab\img\mil\10032500001603000000.paa"}; // Target reference point
	// Needs x2 scale
	_drawSize = 2;
	if (_markerIcon == 200) exitWith {"\cTab\img\mil\10032500001301000000.paa"}; // Generic
	if (_markerIcon == 201) exitWith {"\cTab\img\mil\10032500001303000000.paa"}; // CKP
	if (_markerIcon == 202) exitWith {"\cTab\img\mil\10032500001308000000.paa"}; // SOS
	if (_markerIcon == 203) exitWith {"\cTab\img\mil\10032500001309000000.paa"}; // EC
	if (_markerIcon == 204) exitWith {"\cTab\img\mil\10032500001314000000.paa"}; // RLY
	if (_markerIcon == 205) exitWith {"\cTab\img\mil\10032500001316000000.paa"}; // SP
	if (_markerIcon == 206) exitWith {"\cTab\img\mil\10032500001604000000.paa"}; // PD
	if (_markerIcon == 207) exitWith {"\cTab\img\mil\10032500003205000000.paa"}; // CCP
	if (_markerIcon == 208) exitWith {"\cTab\img\mil\10032500003207000000.paa"}; // DET
	if (_markerIcon == 209) exitWith {"\cTab\img\mil\10032500003211000000.paa"}; // MED
	if (_markerIcon == 210) exitWith {"\cTab\img\mil\10032500003212000000.paa"}; // R3P
	""
};

_markerSize = _this select 2;
if ( _markerIcon >= 100 ) then {
	_texture2 = "";
	if ( _markerSize > 0 ) then {
		_text = format ["%1-%2", ["A","B","C","D","E","F"] select (_markerSize - 1), _text];
	};
}
else {
	_texture2 = call {
		if (_markerSize == 0) exitWith {""};
		if (_markerSize == 1) exitWith {"\A3\ui_f\data\map\markers\nato\group_0.paa"};
		if (_markerSize == 2) exitWith {"\A3\ui_f\data\map\markers\nato\group_1.paa"};
		if (_markerSize == 3) exitWith {"\A3\ui_f\data\map\markers\nato\group_2.paa"};
		if (_markerSize == 4) exitWith {"\A3\ui_f\data\map\markers\nato\group_3.paa"};
		""
	};
};

_markerDir = _this select 3;
_dir = if (_markerDir == 0) then { 400 } else { (_markerDir - 1) * 45 };

_align = if ((_dir > 0) && (_dir < 180)) then {"left"} else {"right"};

[_pos,_texture1,_texture2,_dir,_color,_text,_align,_drawSize]