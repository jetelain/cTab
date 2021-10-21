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
    
    Parameters:
        0: ARRAY - Marker data
    
    Returns:
        ARRAY - Translated marker data
    
    Example:
        [[1714.35,5716.82],0,0,0,"12:00"] call cTab_fnc_translateUserMarker;
*/

private ["_pos","_markerIcon","_texture1","_markerSize","_texture2","_markerDir","_dir","_text","_align"];

_pos = _this select 0;

_color = cTabColorRed;
_markerIcon = _this select 1;
_texture1 = call {
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

    if (_markerIcon == 200) exitWith {"\cTab\img\ILZ.paa"};
    if (_markerIcon == 201) exitWith {"\cTab\img\ELZ.paa"};
    if (_markerIcon == 202) exitWith {"\cTab\img\DZ.paa"};
    if (_markerIcon == 203) exitWith {"\cTab\img\FLZ.paa"};

    _color = cTabColorGreen;
    if (_markerIcon == 210) exitWith {"\cTab\img\ILZ.paa"};
    if (_markerIcon == 211) exitWith {"\cTab\img\ELZ.paa"};
    if (_markerIcon == 212) exitWith {"\cTab\img\DZ.paa"};
    if (_markerIcon == 213) exitWith {"\cTab\img\FLZ.paa"};

    _color = cTabColorBlue;
    if (_markerIcon == 20) exitWith {"\A3\ui_f\data\map\markers\military\dot_CA.paa"};
    if (_markerIcon == 21) exitWith {"\cTab\img\ccp.paa"};
    if (_markerIcon == 22) exitWith {"\A3\ui_f\data\map\mapcontrol\Hospital_CA.paa"};
    if (_markerIcon == 23) exitWith {"\A3\ui_f\data\map\markers\military\circle_CA.paa"};
    if (_markerIcon == 24) exitWith {"\A3\ui_f\data\map\markers\military\pickup_CA.paa"};
    if (_markerIcon == 25) exitWith {"\cTab\img\DCN.paa"};
    if (_markerIcon == 30) exitWith {"\A3\ui_f\data\map\markers\nato\b_hq.paa"};
    if (_markerIcon == 31) exitWith {"\A3\ui_f\data\map\markers\nato\b_installation.paa"};
    if (_markerIcon == 32) exitWith {"\A3\ui_f\data\map\markers\nato\b_support.paa"};
    if (_markerIcon == 33) exitWith {"\A3\ui_f\data\map\markers\nato\b_maint.paa"};
    if (_markerIcon == 34) exitWith {"\A3\ui_f\data\map\markers\military\circle_CA.paa"};
    if (_markerIcon == 35) exitWith {"\A3\ui_f\data\map\markers\military\dot_CA.paa"};
    if (_markerIcon == 36) exitWith {"\A3\ui_f\data\map\markers\military\box_CA.paa"};
    if (_markerIcon == 37) exitWith {"\A3\ui_f\data\map\markers\military\triangle_CA.paa"};

    if (_markerIcon == 40) exitWith {"\cTab\img\IED.paa"};
    if (_markerIcon == 41) exitWith {"\cTab\img\apm.paa"};
    if (_markerIcon == 42) exitWith {"\cTab\img\atm.paa"};
    if (_markerIcon == 43) exitWith {"\cTab\img\apms.paa"};
    if (_markerIcon == 44) exitWith {"\cTab\img\atms.paa"};
    if (_markerIcon == 45) exitWith {"\cTab\img\cbrn.paa"};
    if (_markerIcon == 46) exitWith {"\A3\ui_f\data\map\markers\military\warning_CA.paa"};
    if (_markerIcon == 47) exitWith {"\A3\ui_f\data\map\markers\military\unknown_CA.paa"};

    if (_markerIcon == 50) exitWith {"\A3\ui_f\data\map\markers\military\start_CA.paa"};
    if (_markerIcon == 51) exitWith {"\A3\ui_f\data\map\markers\military\end_CA.paa"};
    if (_markerIcon == 52) exitWith {"\A3\ui_f\data\map\markers\military\join_CA.paa"};
    if (_markerIcon == 53) exitWith {"\A3\ui_f\data\map\markers\military\pickup_CA.paa"};
    if (_markerIcon == 54) exitWith {"\A3\ui_f\data\map\markers\military\marker_CA.paa"};
    if (_markerIcon == 55) exitWith {"\A3\ui_f\data\map\markers\military\objective_CA.paa"};
    if (_markerIcon == 56) exitWith {"\A3\ui_f\data\map\markers\military\flag_CA.paa"};
    if (_markerIcon == 57) exitWith {"\A3\ui_f\data\map\markers\military\destroy_CA.paa"};

    if (_markerIcon == 60) exitWith {"\cTab\img\rly.paa"};
    if (_markerIcon == 61) exitWith {"\cTab\img\ckp.paa"};
    if (_markerIcon == 62) exitWith {"\cTab\img\pp.paa"};
    if (_markerIcon == 63) exitWith {"\cTab\img\wp.paa"};
    if (_markerIcon == 64) exitWith {"\cTab\img\rp.paa"};
    if (_markerIcon == 65) exitWith {"\cTab\img\sp.paa"};
    if (_markerIcon == 66) exitWith {"\cTab\img\ap.paa"};

    if (_markerIcon == 70) exitWith {"\cTab\img\aap.paa"};
    if (_markerIcon == 71) exitWith {"\cTab\img\abp.paa"};
    if (_markerIcon == 72) exitWith {"\cTab\img\acp.paa"};
    if (_markerIcon == 73) exitWith {"\cTab\img\orbit.paa"};
    if (_markerIcon == 74) exitWith {"\cTab\img\aep.paa"};
    if (_markerIcon == 75) exitWith {"\cTab\img\aip.paa"};
    if (_markerIcon == 76) exitWith {"\cTab\img\pup.paa"};
    if (_markerIcon == 77) exitWith {"\cTab\img\ackp.paa"};
    if (_markerIcon == 78) exitWith {"\cTab\img\down.paa"};

    if (_markerIcon == 80) exitWith {"\cTab\img\nrp.paa"};
    if (_markerIcon == 81) exitWith {"\cTab\img\nsp.paa"};
    if (_markerIcon == 82) exitWith {"\cTab\img\nnp.paa"};
    if (_markerIcon == 83) exitWith {"\cTab\img\dive.paa"};
    if (_markerIcon == 84) exitWith {"\cTab\img\surface.paa"};
    if (_markerIcon == 85) exitWith {"\cTab\img\land.paa"};

    if (_markerIcon == 220) exitWith {"\cTab\img\ILZ.paa"};
    if (_markerIcon == 221) exitWith {"\cTab\img\ELZ.paa"};
    if (_markerIcon == 222) exitWith {"\cTab\img\DZ.paa"};
    if (_markerIcon == 223) exitWith {"\cTab\img\FLZ.paa"};

    _color = cTabColorBlack;
    if (_markerIcon == 230) exitWith {"\cTab\img\ILZ.paa"};
    if (_markerIcon == 231) exitWith {"\cTab\img\ELZ.paa"};
    if (_markerIcon == 232) exitWith {"\cTab\img\DZ.paa"};
    if (_markerIcon == 233) exitWith {"\cTab\img\FLZ.paa"};

    if (_markerIcon == 100) exitWith {"\cTab\img\Budweiser.paa"};
    if (_markerIcon == 101) exitWith {"\cTab\img\Coors.paa"};
    if (_markerIcon == 102) exitWith {"\cTab\img\Corona.paa"};
    if (_markerIcon == 103) exitWith {"\cTab\img\Guinness.paa"};
    if (_markerIcon == 104) exitWith {"\cTab\img\Hamms.paa"};
    if (_markerIcon == 105) exitWith {"\cTab\img\Heineken.paa"};
    if (_markerIcon == 106) exitWith {"\cTab\img\Keystone.paa"};
    if (_markerIcon == 107) exitWith {"\cTab\img\Miller.paa"};
    if (_markerIcon == 108) exitWith {"\cTab\img\Pabst.paa"};
    if (_markerIcon == 109) exitWith {"\cTab\img\Schlitz.paa"};

    if (_markerIcon == 110) exitWith {"\cTab\img\Argon.paa"};
    if (_markerIcon == 111) exitWith {"\cTab\img\Boron.paa"};
    if (_markerIcon == 112) exitWith {"\cTab\img\Carbon.paa"};
    if (_markerIcon == 113) exitWith {"\cTab\img\Gold.paa"};
    if (_markerIcon == 114) exitWith {"\cTab\img\Iron.paa"};
    if (_markerIcon == 115) exitWith {"\cTab\img\Lead.paa"};
    if (_markerIcon == 116) exitWith {"\cTab\img\Neon.paa"};
    if (_markerIcon == 117) exitWith {"\cTab\img\Silver.paa"};
    if (_markerIcon == 118) exitWith {"\cTab\img\Tin.paa"};
    if (_markerIcon == 119) exitWith {"\cTab\img\Zinc.paa"};

    if (_markerIcon == 120) exitWith {"\cTab\img\Amazon.paa"};
    if (_markerIcon == 121) exitWith {"\cTab\img\Congo.paa"};
    if (_markerIcon == 122) exitWith {"\cTab\img\Mekong.paa"};
    if (_markerIcon == 123) exitWith {"\cTab\img\Nile.paa"};
    if (_markerIcon == 124) exitWith {"\cTab\img\Rio.paa"};
    if (_markerIcon == 125) exitWith {"\cTab\img\Rom.paa"};
    if (_markerIcon == 126) exitWith {"\cTab\img\Volga.paa"};
    if (_markerIcon == 127) exitWith {"\cTab\img\Yangtze.paa"};
    if (_markerIcon == 128) exitWith {"\cTab\img\Yellow.paa"};
    if (_markerIcon == 129) exitWith {"\cTab\img\Yukon.paa"};

    if (_markerIcon == 130) exitWith {"\cTab\img\Batman.paa"};
    if (_markerIcon == 131) exitWith {"\cTab\img\Daffy.paa"};
    if (_markerIcon == 132) exitWith {"\cTab\img\Goofy.paa"};
    if (_markerIcon == 133) exitWith {"\cTab\img\Homer.paa"};
    if (_markerIcon == 134) exitWith {"\cTab\img\Jerry.paa"};
    if (_markerIcon == 135) exitWith {"\cTab\img\Rocky.paa"};
    if (_markerIcon == 136) exitWith {"\cTab\img\Scobby.paa"};
    if (_markerIcon == 137) exitWith {"\cTab\img\tom.paa"};
    if (_markerIcon == 138) exitWith {"\cTab\img\Woody.paa"};
    if (_markerIcon == 139) exitWith {"\cTab\img\Yogi.paa"};
    ""
};

_markerSize = _this select 2;
_texture2 = call {
    if (_markerSize == 0) exitWith {""};
    if (_markerSize == 1) exitWith {"\A3\ui_f\data\map\markers\nato\group_0.paa"};
    if (_markerSize == 2) exitWith {"\A3\ui_f\data\map\markers\nato\group_1.paa"};
    if (_markerSize == 3) exitWith {"\A3\ui_f\data\map\markers\nato\group_2.paa"};
    if (_markerSize == 4) exitWith {"\A3\ui_f\data\map\markers\nato\group_3.paa"};
    ""
};

_markerDir = _this select 3;
_dir = call {
    if (_markerDir == 0) exitWith {400};
    if (_markerDir == 1) exitWith {0};
    if (_markerDir == 2) exitWith {45};
    if (_markerDir == 3) exitWith {90};
    if (_markerDir == 4) exitWith {135};
    if (_markerDir == 5) exitWith {180};
    if (_markerDir == 6) exitWith {225};
    if (_markerDir == 7) exitWith {270};
    if (_markerDir == 8) exitWith {315};
    700
};

_text = _this select 4;
_align = if ((_dir > 0) && (_dir < 180)) then {"left"} else {"right"};

[_pos,_texture1,_texture2,_dir,_color,_text,_align]