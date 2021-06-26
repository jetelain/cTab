class MainSubmenu: cTab_RscControlsGroup
{
	#ifndef cTab_IS_TABLET
		#define cTab_MENU_MAX_ELEMENTS 11
	#else
		#define cTab_MENU_MAX_ELEMENTS 12
	#endif
	idc = 3300;
	x = MENU_X;
	y = MENU_Y;
	w = MENU_W;
	h = MENU_H(cTab_MENU_MAX_ELEMENTS);
	class controls
	{
		class mainbg: cTab_IGUIBack
		{
			idc = IDC_USRMN_MAINBG;
			x = 0;
			y = 0;
			w = MENU_W;
			h = MENU_H(cTab_MENU_MAX_ELEMENTS);
		};
		class op4btn: cTab_MenuItem
		{
			idc = IDC_USRMN_OP4BTN;
			text = $STR_ctab_core_EnemyMenu;
			x = 0;
			y = MENU_elementY(1);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "[11] call cTab_fnc_userMenuSelect;";
		};
		class medbtn: cTab_MenuItem
		{
			idc = IDC_USRMN_MEDBTN;
			text = $STR_ctab_core_MedicalMenu;
			x = 0;
			y = MENU_elementY(2);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "[21] call cTab_fnc_userMenuSelect;";
		};
		class genbtn: cTab_MenuItem
		{
			idc = IDC_USRMN_GENBTN;
			text = $STR_ctab_core_GeneralMenu;
			x = 0;
			y = MENU_elementY(3);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "[31] call cTab_fnc_userMenuSelect;";
		};
		class dangerbtn: cTab_MenuItem
		{
			idc = IDC_USRMN_DANGERBTN;
			text = $STR_ctab_core_DangerMenu;
			x = 0;
			y = MENU_elementY(4);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "[41] call cTab_fnc_userMenuSelect;";
		};
		class armabtn: cTab_MenuItem
		{
			idc = IDC_USRMN_ARMABTN;
			text = $STR_ctab_core_ARMAMenu;
			x = 0;
			y = MENU_elementY(5);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "[51] call cTab_fnc_userMenuSelect;";
		};
		class gocodebtn: cTab_MenuItem
		{
			idc = IDC_USRMN_GOCODEBTN;
			text = $STR_ctab_core_GoMenu;
			x = 0;
			y = MENU_elementY(6);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "[61] call cTab_fnc_userMenuSelect;";
		};
		class landbtn: cTab_MenuItem
		{
			idc = IDC_USRMN_LANDBTN;
			text = $STR_ctab_core_LandMenu;
			x = 0;
			y = MENU_elementY(7);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "[71] call cTab_fnc_userMenuSelect;";
		};
		class airbtn: cTab_MenuItem
		{
			idc = IDC_USRMN_AIRBTN;
			text = $STR_ctab_core_AirMenu;
			x = 0;
			y = MENU_elementY(8);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "[81] call cTab_fnc_userMenuSelect;";
		};
		class navalbtn: cTab_MenuItem
		{
			idc = IDC_USRMN_NAVALBTN;
			text = $STR_ctab_core_NavalMenu;
			x = 0;
			y = MENU_elementY(9);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "[91] call cTab_fnc_userMenuSelect;";
		};
		class zonesbtn: cTab_MenuItem
		{
			idc = IDC_USRMN_ZONESBTN;
			text = $STR_ctab_core_ZonesMenu;
			x = 0;
			y = MENU_elementY(10);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "[101] call cTab_fnc_userMenuSelect;";
		};
		#ifdef cTab_IS_TABLET
			class lockUavCam: cTab_MenuItem
			{
				idc = -1;
				text = $STR_ctab_core_UAVLockMenu;
				toolTip = $STR_ctab_core_UAVLockHint;
				x = 0;
				y = MENU_elementY(4);
				w = MENU_W;
				h = MENU_elementH;
				sizeEx = MENU_sizeEx;
				action = "[2] call cTab_fnc_userMenuSelect;";
			};
		#endif
		class exit: cTab_MenuExit
		{
			idc = -1;
			text = $STR_ctab_core_MenuExit;
			x = 0;
			y = MENU_elementY(cTab_MENU_MAX_ELEMENTS);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "[0] call cTab_fnc_userMenuSelect;";
		};
	};
};

class EnemySub1: cTab_RscControlsGroup
{
	#define cTab_MENU_MAX_ELEMENTS 8
	idc = 3301;
	x = MENU_X;
	y = MENU_Y;
	w = MENU_W;
	h = MENU_H(cTab_MENU_MAX_ELEMENTS);
	class controls
	{
		class IGUIBack_2201: cTab_IGUIBack
		{
			idc = IDC_USRMN_IGUIBACK_2202;
			x = 0;
			y = 0;
			w = MENU_W;
			h = MENU_H(cTab_MENU_MAX_ELEMENTS);
		};
		class infbtn: cTab_MenuItem
		{
			idc = IDC_USRMN_INFBTN;
			text = $STR_ctab_core_InfantryMenu;
			x = 0;
			y = MENU_elementY(1);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [1,0];[12] call cTab_fnc_userMenuSelect;";
		};
		class mecinfbtn: cTab_MenuItem
		{
			idc = IDC_USRMN_MECINFBTN;
			text = $STR_ctab_core_MechInfMenu;
			toolTip = $STR_ctab_core_MechInfHint;
			x = 0;
			y = MENU_elementY(2);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [1,1];[12] call cTab_fnc_userMenuSelect;";
		};

		class motrinfbtn: cTab_MenuItem
		{
			idc = IDC_USRMN_MOTRINFBTN;
			text = $STR_ctab_core_MotoInfMenu;
			toolTip = $STR_ctab_core_MotoInfHint;
			x = 0;
			y = MENU_elementY(3);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [1,2];[12] call cTab_fnc_userMenuSelect;";
		};
		class amrbtn: cTab_MenuItem
		{
			idc = IDC_USRMN_AMRBTN;
			text = $STR_ctab_core_ArmorMenu;
			x = 0;
			y = MENU_elementY(4);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [1,3];[12] call cTab_fnc_userMenuSelect;";
		};
		class helibtn: cTab_MenuItem
		{
			idc = IDC_USRMN_HELIBTN;
			text = $STR_ctab_core_HelicopterMenu;
			x = 0;
			y = MENU_elementY(5);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [1,4];[12] call cTab_fnc_userMenuSelect;";
		};
		class plnbtn: cTab_MenuItem
		{
			idc = IDC_USRMN_PLNBTN;
			text = $STR_ctab_core_PlaneMenu;
			x = 0;
			y = MENU_elementY(6);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [1,5];[12] call cTab_fnc_userMenuSelect;";
		};
		class uknbtn: cTab_MenuItem
		{
			idc = IDC_USRMN_UKNBTN;
			text = $STR_ctab_core_UnknownMenu;
			x = 0;
			y = MENU_elementY(7);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [1,6];[12] call cTab_fnc_userMenuSelect;";
		};
		class exit: cTab_MenuExit
		{
			idc = -1;
			text = $STR_ctab_core_MenuExit;
			x = 0;
			y = MENU_elementY(cTab_MENU_MAX_ELEMENTS);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "[0] call cTab_fnc_userMenuSelect;";
		};
	};
};

class EnemySub2: cTab_RscControlsGroup
{
	#define cTab_MENU_MAX_ELEMENTS 6
	idc = 3303;
	x = MENU_X;
	y = MENU_Y;
	w = MENU_W;
	h = MENU_H(cTab_MENU_MAX_ELEMENTS);
	class controls
	{
		class IGUIBack_2202: cTab_IGUIBack
		{
			idc = IDC_USRMN_IGUIBACK_2202;
			x = 0;
			y = 0;
			w = MENU_W;
			h = MENU_H(cTab_MENU_MAX_ELEMENTS);
		};
		class ftbtn: cTab_MenuItem
		{
			idc = IDC_USRMN_FTBTN;
			text = $STR_ctab_core_SingularMenu;
			x = 0;
			y = MENU_elementY(1);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "[14] call cTab_fnc_userMenuSelect;";
		};
		class patbtn: cTab_MenuItem
		{
			idc = IDC_USRMN_PATBTN;
			text = $STR_ctab_core_TeamMenu;
			toolTip = $STR_ctab_core_TeamHint;
			x = 0;
			y = MENU_elementY(2);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [2,1];[13] call cTab_fnc_userMenuSelect;";
		};
		class sqdbtn: cTab_MenuItem
		{
			idc = IDC_USRMN_SQDBTN;
			text = $STR_ctab_core_SquadMenu;
			toolTip = $STR_ctab_core_SquadHint;
			x = 0;
			y = MENU_elementY(3);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [2,2];[13] call cTab_fnc_userMenuSelect;";
		};
		class sctbtn: cTab_MenuItem
		{
			idc = IDC_USRMN_SCTBTN;
			text = $STR_ctab_core_SectionMenu;
			toolTip = $STR_ctab_core_SectionHint;
			x = 0;
			y = MENU_elementY(4);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [2,3];[13] call cTab_fnc_userMenuSelect;";
		};
		class pltnbtn: cTab_MenuItem
		{
			idc = IDC_USRMN_PLTNBTN;
			text = $STR_ctab_core_PlatoonMenu;
			toolTip = $STR_ctab_core_PlatoonHint;
			x = 0;
			y = MENU_elementY(5);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [2,4];[13] call cTab_fnc_userMenuSelect;";
		};
		class exit: cTab_MenuExit
		{
			idc = -1;
			text = $STR_ctab_core_MenuExit;
			x = 0;
			y = MENU_elementY(cTab_MENU_MAX_ELEMENTS);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "[0] call cTab_fnc_userMenuSelect;";
		};
	};
};

class EnemySub3: cTab_RscControlsGroup
{
	#define cTab_MENU_MAX_ELEMENTS 10
	idc = 3304;
	x = MENU_X;
	y = MENU_Y;
	w = MENU_W;
	h = MENU_H(cTab_MENU_MAX_ELEMENTS);
	class controls
	{
		class IGUIBack_2203: cTab_IGUIBack
		{
			idc = IDC_USRMN_IGUIBACK_2203;
			x = 0;
			y = 0;
			w = MENU_W;
			h = MENU_H(cTab_MENU_MAX_ELEMENTS);
		};
		class stnbtn: cTab_MenuItem
		{
			idc = IDC_USRMN_STNBTN;
			text = $STR_ctab_core_StationaryMenu;
			x = 0;
			y = MENU_elementY(1);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "[1] call cTab_fnc_userMenuSelect;";
		};
		class nthbtn: cTab_MenuItem
		{
			idc = IDC_USRMN_NTHBTN;
			text = $STR_ctab_core_North;
			x = 0;
			y = MENU_elementY(2);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [3,1];[1] call cTab_fnc_userMenuSelect;";
		};
		class nebtn: cTab_MenuItem
		{
			idc = IDC_USRMN_NEBTN;
			text = $STR_ctab_core_NorthEast;
			x = 0;
			y = MENU_elementY(3);
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [3,2];[1] call cTab_fnc_userMenuSelect;";
		};
		class ebtn: cTab_MenuItem
		{
			idc = IDC_USRMN_EBTN;
			text = $STR_ctab_core_East;
			x = 0;
			y = MENU_elementY(4);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [3,3];[1] call cTab_fnc_userMenuSelect;";
		};
		class sebtn: cTab_MenuItem
		{
			idc = IDC_USRMN_SEBTN;
			text = $STR_ctab_core_SouthEast;
			x = 0;
			y = MENU_elementY(5);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [3,4];[1] call cTab_fnc_userMenuSelect;";
		};
		class sbtn: cTab_MenuItem
		{
			idc = IDC_USRMN_SBTN;
			text = $STR_ctab_core_South;
			x = 0;
			y = MENU_elementY(6);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [3,5];[1] call cTab_fnc_userMenuSelect;";
		};
		class swbtn: cTab_MenuItem
		{
			idc = IDC_USRMN_SWBTN;
			text = $STR_ctab_core_SouthWest;
			x = 0;
			y = MENU_elementY(7);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [3,6];[1] call cTab_fnc_userMenuSelect;";
		};
		class wbtn: cTab_MenuItem
		{
			idc = IDC_USRMN_WBTN;
			text = $STR_ctab_core_West;
			x = 0;
			y = MENU_elementY(8);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [3,7];[1] call cTab_fnc_userMenuSelect;";
		};
		class RscText_1022: cTab_MenuItem
		{
			idc = IDC_USRMN_RSCTEXT_1022;
			text = $STR_ctab_core_NorthWest;
			x = 0;
			y = MENU_elementY(9);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [3,8];[1] call cTab_fnc_userMenuSelect;";
		};
		class exit: cTab_MenuExit
		{
			idc = -1;
			text = $STR_ctab_core_MenuExit;
			x = 0;
			y = MENU_elementY(cTab_MENU_MAX_ELEMENTS);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "[0] call cTab_fnc_userMenuSelect;";
		};
	};
};

class EnemySub4: cTab_RscControlsGroup
{
	#define cTab_MENU_MAX_ELEMENTS 8
	idc = 3307;
	x = MENU_X;
	y = MENU_Y;
	w = MENU_W;
	h = MENU_H(cTab_MENU_MAX_ELEMENTS);
	class controls
	{
		class IGUIBack_2202: cTab_IGUIBack
		{
			idc = IDC_USRMN_IGUIBACK_2202;
			x = 0;
			y = 0;
			w = MENU_W;
			h = MENU_H(cTab_MENU_MAX_ELEMENTS);
		};
		class rifle_btn: cTab_MenuItem
		{
			idc = -1;
			text = $STR_ctab_core_RifleMenu;
			x = 0;
			y = MENU_elementY(1);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [1,7];[13] call cTab_fnc_userMenuSelect;";
		};
		class lmg_btn: cTab_MenuItem
		{
			idc = -1;
			text = $STR_ctab_core_MGMenu;
			x = 0;
			y = MENU_elementY(2);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [1,8];[13] call cTab_fnc_userMenuSelect;";
		};
		class at_btn: cTab_MenuItem
		{
			idc = -1;
			text = $STR_ctab_core_ATMenu;
			x = 0;
			y = MENU_elementY(3);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [1,9];[13] call cTab_fnc_userMenuSelect;";
		};
		class mmg_btn: cTab_MenuItem
		{
			idc = -1;
			text = $STR_ctab_core_StaticMGMenu;
			x = 0;
			y = MENU_elementY(4);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [1,10];[13] call cTab_fnc_userMenuSelect;";
		};
		class mat_btn: cTab_MenuItem
		{
			idc = -1;
			text = $STR_ctab_core_StaticATMenu;
			x = 0;
			y = MENU_elementY(5);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [1,11];[13] call cTab_fnc_userMenuSelect;";
		};
		class aa_btn: cTab_MenuItem
		{
			idc = -1;
			text = $STR_ctab_core_StaticAAMenu;
			x = 0;
			y = MENU_elementY(6);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [1,13];[13] call cTab_fnc_userMenuSelect;";
		};
		class mmortar_btn: cTab_MenuItem
		{
			idc = -1;
			text = $STR_ctab_core_MortarMenu;
			x = 0;
			y = MENU_elementY(7);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [1,12];[13] call cTab_fnc_userMenuSelect;";
		};
		class exit: cTab_MenuExit
		{
			idc = -1;
			text = $STR_ctab_core_MenuExit;
			x = 0;
			y = MENU_elementY(cTab_MENU_MAX_ELEMENTS);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "[0] call cTab_fnc_userMenuSelect;";
		};
	};
};
 
class CasulSub1: cTab_RscControlsGroup
{
	#define cTab_MENU_MAX_ELEMENTS 7
	idc = 3305;
	x = MENU_X;
	y = MENU_Y;
	w = MENU_W;
	h = MENU_H(cTab_MENU_MAX_ELEMENTS);
	class controls
	{
		class IGUIBack_2204: cTab_IGUIBack
		{
			idc = IDC_USRMN_IGUIBACK_2204;
			x = 0;
			y = 0;
			w = MENU_W;
			h = MENU_H(cTab_MENU_MAX_ELEMENTS);
		};
		class casltybtn: cTab_MenuItem
		{
			idc = IDC_USRMN_CASLTYBTN;
			text = $STR_ctab_core_CasualtyMenu;
			x = 0;
			y = MENU_elementY(1);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [1,20];[1] call cTab_fnc_userMenuSelect;";
		};
		class ccpbtn: cTab_MenuItem
		{
			idc = IDC_USRMN_CCPBTN;
			text = $STR_ctab_core_CCPMenu;
			toolTip = $STR_ctab_core_CCPHint;
			x = 0;
			y = MENU_elementY(2);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [1,21];[1] call cTab_fnc_userMenuSelect;";
		};
		class basbtn: cTab_MenuItem
		{
			idc = IDC_USRMN_BASBTN;
			text = $STR_ctab_core_BASMenu;
			toolTip = $STR_ctab_core_BASHint;
			x = 0;
			y = MENU_elementY(3);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [1,22];[1] call cTab_fnc_userMenuSelect;";
		};
		// Mass Casualty Incident
		class mcibtn: cTab_MenuItem
		{
			idc = IDC_USRMN_MCIBTN;
			text = $STR_ctab_core_MCIMenu;
			toolTip = $STR_ctab_core_MCIHint;
			x = 0;
			y = MENU_elementY(4);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [1,23];[1] call cTab_fnc_userMenuSelect;";
		};
		class mlzbtn: cTab_MenuItem
		{
			idc = IDC_USRMN_MLZBTN;
			text = $STR_ctab_core_medlzMenu;
			toolTip = $STR_ctab_core_medlzHint;
			x = 0;
			y = MENU_elementY(5);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [1,24];[1] call cTab_fnc_userMenuSelect;";
		};
		class dcnbtn: cTab_MenuItem
		{
			idc = IDC_USRMN_DCNBTN;
			text = $STR_ctab_core_deconMenu;
			toolTip = $STR_ctab_core_deconHint;
			x = 0;
			y = MENU_elementY(6);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [1,25];[1] call cTab_fnc_userMenuSelect;";
		};
		class exit: cTab_MenuExit
		{
			idc = -1;
			text = $STR_ctab_core_MenuExit;
			x = 0;
			y = MENU_elementY(cTab_MENU_MAX_ELEMENTS);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "[0] call cTab_fnc_userMenuSelect;";
		};
	};
};

class GenSub1: cTab_RscControlsGroup
{
	#define cTab_MENU_MAX_ELEMENTS 9
	idc = 3306;
	x = MENU_X;
	y = MENU_Y;
	w = MENU_W;
	h = MENU_H(cTab_MENU_MAX_ELEMENTS);
	class controls
	{
		class IGUIBack_2205: cTab_IGUIBack
		{
			idc = IDC_USRMN_IGUIBACK_2205;
			x = 0;
			y = 0;
			w = MENU_W;
			h = MENU_H(cTab_MENU_MAX_ELEMENTS);
		};
		class hqbtn: cTab_MenuItem
		{
			idc = IDC_USRMN_HQBTN;
			text = $STR_ctab_core_HQMenu;
			toolTip = $STR_ctab_core_HQHint;
			x = 0;
			y = MENU_elementY(1);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [1,30];[1] call cTab_fnc_userMenuSelect;";
		};

		class fobbtn: cTab_MenuItem
		{
			idc = IDC_USRMN_FOBBTN;
			text = $STR_ctab_core_fobMenu;
			toolTip = $STR_ctab_core_fobHint;
			x = 0;
			y = MENU_elementY(2);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [1,31];[1] call cTab_fnc_userMenuSelect;";
		};
		class fsbbtn: cTab_MenuItem
		{
			idc = IDC_USRMN_FSBBTN;
			text = $STR_ctab_core_fsbMenu;
			toolTip = $STR_ctab_core_fsbHint;
			x = 0;
			y = MENU_elementY(3);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [1,32];[1] call cTab_fnc_userMenuSelect;";
		};
		class fmpbtn: cTab_MenuItem
		{
			idc = IDC_USRMN_FMPBTN;
			text = $STR_ctab_core_fmpMenu;
			toolTip = $STR_ctab_core_fmpHint;
			x = 0;
			y = MENU_elementY(4);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [1,33];[1] call cTab_fnc_userMenuSelect;";
		};
		class Circlebtn: cTab_MenuItem
		{
			idc = IDC_USRMN_CIRCLEBTN;
			text = $STR_ctab_core_CircleMenu;
			toolTip = $STR_ctab_core_CircleHint;
			x = 0;
			y = MENU_elementY(5);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [1,34];[1] call cTab_fnc_userMenuSelect;";
		};
		class Dotbtn: cTab_MenuItem
		{
			idc = IDC_USRMN_DOTBTN;
			text = $STR_ctab_core_DotMenu;
			toolTip = $STR_ctab_core_DotHint;
			x = 0;
			y = MENU_elementY(6);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [1,35];[1] call cTab_fnc_userMenuSelect;";
		};
		class Squarebtn: cTab_MenuItem
		{
			idc = IDC_USRMN_SQUAREBTN;
			text = $STR_ctab_core_SquareMenu;
			toolTip = $STR_ctab_core_SquareHint;
			x = 0;
			y = MENU_elementY(7);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [1,36];[1] call cTab_fnc_userMenuSelect;";
		};
		class Trianglebtn: cTab_MenuItem
		{
			idc = IDC_USRMN_TRIANGLEBTN;
			text = $STR_ctab_core_TriangleMenu;
			toolTip = $STR_ctab_core_TriangleHint;
			x = 0;
			y = MENU_elementY(8);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [1,37];[1] call cTab_fnc_userMenuSelect;";
		};
		class exit: cTab_MenuExit
		{
			idc = -1;
			text = $STR_ctab_core_MenuExit;
			x = 0;
			y = MENU_elementY(cTab_MENU_MAX_ELEMENTS);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "[0] call cTab_fnc_userMenuSelect;";
		};
	};
};

///////////////////

class DangerSub1: cTab_RscControlsGroup
{
	#define cTab_MENU_MAX_ELEMENTS 9
	idc = 3308;
	x = MENU_X;
	y = MENU_Y;
	w = MENU_W;
	h = MENU_H(cTab_MENU_MAX_ELEMENTS);
	class controls
	{
		class IGUIBack_2206: cTab_IGUIBack
		{
			idc = IDC_USRMN_IGUIBACK_2206;
			x = 0;
			y = 0;
			w = MENU_W;
			h = MENU_H(cTab_MENU_MAX_ELEMENTS);
		};
		class iedbtn: cTab_MenuItem
		{
			idc = IDC_USRMN_IEDBTN;
			text = $STR_ctab_core_IEDMenu;
			toolTip = $STR_ctab_core_IEDHint;
			x = 0;
			y = MENU_elementY(1);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [1,40];[1] call cTab_fnc_userMenuSelect;";
		};
		class apmbtn: cTab_MenuItem
		{
			idc = IDC_USRMN_APMBTN;
			text = $STR_ctab_core_apmMenu;
			toolTip = $STR_ctab_core_apmHint;
			x = 0;
			y = MENU_elementY(2);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [1,41];[1] call cTab_fnc_userMenuSelect;";
		};
		class atmbtn: cTab_MenuItem
		{
			idc = IDC_USRMN_ATMBTN;
			text = $STR_ctab_core_atmMenu;
			toolTip = $STR_ctab_core_atmHint;
			x = 0;
			y = MENU_elementY(3);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [1,42];[1] call cTab_fnc_userMenuSelect;";
		};
		class apmsbtn: cTab_MenuItem
		{
			idc = IDC_USRMN_APMSBTN;
			text = $STR_ctab_core_apmsMenu;
			toolTip = $STR_ctab_core_apmsHint;
			x = 0;
			y = MENU_elementY(4);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [1,43];[1] call cTab_fnc_userMenuSelect;";
		};
		class atmsbtn: cTab_MenuItem
		{
			idc = IDC_USRMN_ATMSBTN;
			text = $STR_ctab_core_atmsMenu;
			toolTip = $STR_ctab_core_atmsHint;
			x = 0;
			y = MENU_elementY(5);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [1,44];[1] call cTab_fnc_userMenuSelect;";
		};
		class cbrnbtn: cTab_MenuItem
		{
			idc = IDC_USRMN_CBRNBTN;
			text = $STR_ctab_core_cbrnMenu;
			toolTip = $STR_ctab_core_cbrnHint;
			x = 0;
			y = MENU_elementY(6);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [1,45];[1] call cTab_fnc_userMenuSelect;";
		};
		class Warningbtn: cTab_MenuItem
		{
			idc = IDC_USRMN_WARNINGBTN;
			text = $STR_ctab_core_WarningMenu;
			toolTip = $STR_ctab_core_WarningHint;
			x = 0;
			y = MENU_elementY(7);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [1,46];[1] call cTab_fnc_userMenuSelect;";
		};
		class Unknowgbtn: cTab_MenuItem
		{
			idc = IDC_USRMN_UNKNOWBTN;
			text = $STR_ctab_core_UnknowMenu;
			toolTip = $STR_ctab_core_UnknowHint;
			x = 0;
			y = MENU_elementY(8);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [1,47];[1] call cTab_fnc_userMenuSelect;";
		};
		class exit: cTab_MenuExit
		{
			idc = -1;
			text = $STR_ctab_core_MenuExit;
			x = 0;
			y = MENU_elementY(cTab_MENU_MAX_ELEMENTS);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "[0] call cTab_fnc_userMenuSelect;";
		};
	};
};


class armaSub1: cTab_RscControlsGroup
{
	#define cTab_MENU_MAX_ELEMENTS 9
	idc = 3309;
	x = MENU_X;
	y = MENU_Y;
	w = MENU_W;
	h = MENU_H(cTab_MENU_MAX_ELEMENTS);
	class controls
	{
		class IGUIBack_2207: cTab_IGUIBack
		{
			idc = IDC_USRMN_IGUIBACK_2207;
			x = 0;
			y = 0;
			w = MENU_W;
			h = MENU_H(cTab_MENU_MAX_ELEMENTS);
		};
		class startbtn: cTab_MenuItem
		{
			idc = IDC_USRMN_STARTBTN;
			text = $STR_ctab_core_StartMenu;
			toolTip = $STR_ctab_core_StartHint;
			x = 0;
			y = MENU_elementY(1);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [1,50];[1] call cTab_fnc_userMenuSelect;";
		};
		class Endbtn: cTab_MenuItem
		{
			idc = IDC_USRMN_ENDBTN;
			text = $STR_ctab_core_EndMenu;
			toolTip = $STR_ctab_core_EndHint;
			x = 0;
			y = MENU_elementY(2);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [1,51];[1] call cTab_fnc_userMenuSelect;";
		};
		class Joinbtn: cTab_MenuItem
		{
			idc = IDC_USRMN_JOINBTN;
			text = $STR_ctab_core_JoinMenu;
			toolTip = $STR_ctab_core_JoinHint;
			x = 0;
			y = MENU_elementY(3);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [1,52];[1] call cTab_fnc_userMenuSelect;";
		};
		class Pickupbtn: cTab_MenuItem
		{
			idc = IDC_USRMN_PICKUPBTN;
			text = $STR_ctab_core_PickupMenu;
			toolTip = $STR_ctab_core_PickupHint;
			x = 0;
			y = MENU_elementY(4);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [1,53];[1] call cTab_fnc_userMenuSelect;";
		};
		class Markerbtn: cTab_MenuItem
		{
			idc = IDC_USRMN_MARKERBTN;
			text = $STR_ctab_core_MarkerMenu;
			toolTip = $STR_ctab_core_MarkerHint;
			x = 0;
			y = MENU_elementY(5);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [1,54];[1] call cTab_fnc_userMenuSelect;";
		};
		class Objectivebtn: cTab_MenuItem
		{
			idc = IDC_USRMN_OBJECTIVEBTN;
			text = $STR_ctab_core_ObjectiveMenu;
			toolTip = $STR_ctab_core_ObjectiveHint;
			x = 0;
			y = MENU_elementY(6);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [1,55];[1] call cTab_fnc_userMenuSelect;";
		};
		class Flagbtn: cTab_MenuItem
		{
			idc = IDC_USRMN_FLAGBTN;
			text = $STR_ctab_core_FlagMenu;
			toolTip = $STR_ctab_core_FlagHint;
			x = 0;
			y = MENU_elementY(7);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [1,56];[1] call cTab_fnc_userMenuSelect;";
		};
		class Destroybtn: cTab_MenuItem
		{
			idc = IDC_USRMN_DESTROYBTN;
			text = $STR_ctab_core_DestroyMenu;
			toolTip = $STR_ctab_core_DestroyHint;
			x = 0;
			y = MENU_elementY(8);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [1,57];[1] call cTab_fnc_userMenuSelect;";
		};
		class exit: cTab_MenuExit
		{
			idc = -1;
			text = $STR_ctab_core_MenuExit;
			x = 0;
			y = MENU_elementY(cTab_MENU_MAX_ELEMENTS);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "[0] call cTab_fnc_userMenuSelect;";
		};
	};
};


class landSub1: cTab_RscControlsGroup
{
	#define cTab_MENU_MAX_ELEMENTS 9
	idc = 3311;
	x = MENU_X;
	y = MENU_Y;
	w = MENU_W;
	h = MENU_H(cTab_MENU_MAX_ELEMENTS);
	class controls
	{
		class IGUIBack_2208: cTab_IGUIBack
		{
			idc = IDC_USRMN_IGUIBACK_2208;
			x = 0;
			y = 0;
			w = MENU_W;
			h = MENU_H(cTab_MENU_MAX_ELEMENTS);
		};
		class rlybtn: cTab_MenuItem
		{
			idc = IDC_USRMN_RLYBTN;
			text = $STR_ctab_core_RLYMenu;
			toolTip = $STR_ctab_core_RLYtHint;
			x = 0;
			y = MENU_elementY(1);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [1,60];[1] call cTab_fnc_userMenuSelect;";
		};
		class Endbtn: cTab_MenuItem
		{
			idc = IDC_USRMN_ENDBTN;
			text = $STR_ctab_core_EndMenu;
			toolTip = $STR_ctab_core_EndHint;
			x = 0;
			y = MENU_elementY(2);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [1,51];[1] call cTab_fnc_userMenuSelect;";
		};
		class Joinbtn: cTab_MenuItem
		{
			idc = IDC_USRMN_JOINBTN;
			text = $STR_ctab_core_JoinMenu;
			toolTip = $STR_ctab_core_JoinHint;
			x = 0;
			y = MENU_elementY(3);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [1,52];[1] call cTab_fnc_userMenuSelect;";
		};
		class Pickupbtn: cTab_MenuItem
		{
			idc = IDC_USRMN_PICKUPBTN;
			text = $STR_ctab_core_PickupMenu;
			toolTip = $STR_ctab_core_PickupHint;
			x = 0;
			y = MENU_elementY(4);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [1,53];[1] call cTab_fnc_userMenuSelect;";
		};
		class Markerbtn: cTab_MenuItem
		{
			idc = IDC_USRMN_MARKERBTN;
			text = $STR_ctab_core_MarkerMenu;
			toolTip = $STR_ctab_core_MarkerHint;
			x = 0;
			y = MENU_elementY(5);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [1,54];[1] call cTab_fnc_userMenuSelect;";
		};
		class Objectivebtn: cTab_MenuItem
		{
			idc = IDC_USRMN_OBJECTIVEBTN;
			text = $STR_ctab_core_ObjectiveMenu;
			toolTip = $STR_ctab_core_ObjectiveHint;
			x = 0;
			y = MENU_elementY(6);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [1,55];[1] call cTab_fnc_userMenuSelect;";
		};
		class Flagbtn: cTab_MenuItem
		{
			idc = IDC_USRMN_FLAGBTN;
			text = $STR_ctab_core_FlagMenu;
			toolTip = $STR_ctab_core_FlagHint;
			x = 0;
			y = MENU_elementY(7);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [1,56];[1] call cTab_fnc_userMenuSelect;";
		};
		class Destroybtn: cTab_MenuItem
		{
			idc = IDC_USRMN_DESTROYBTN;
			text = $STR_ctab_core_DestroyMenu;
			toolTip = $STR_ctab_core_DestroyHint;
			x = 0;
			y = MENU_elementY(8);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [1,57];[1] call cTab_fnc_userMenuSelect;";
		};
		class exit: cTab_MenuExit
		{
			idc = -1;
			text = $STR_ctab_core_MenuExit;
			x = 0;
			y = MENU_elementY(cTab_MENU_MAX_ELEMENTS);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "[0] call cTab_fnc_userMenuSelect;";
		};
	};
};