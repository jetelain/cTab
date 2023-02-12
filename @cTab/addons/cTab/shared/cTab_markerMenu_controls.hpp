class MainSubmenu: cTab_RscControlsGroup
{
	#ifndef cTab_IS_TABLET
		#define cTab_MENU_MAX_ELEMENTS 4
	#else
		#define cTab_MENU_MAX_ELEMENTS 5
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
	#define cTab_MENU_MAX_ELEMENTS 5
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
	#define cTab_MENU_MAX_ELEMENTS 5
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
		class lzbtn: cTab_MenuItem
		{
			idc = IDC_USRMN_LZBTN;
			text = $STR_ctab_core_LZMenu;
			toolTip = $STR_ctab_core_LZHint;
			x = 0;
			y = MENU_elementY(2);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [1,31];[1] call cTab_fnc_userMenuSelect;";
		};


		class dotbtn: cTab_MenuItem
		{
			idc = IDC_USRMN_LZBTN;
			text = $STR_ctab_core_DotMenu;
			x = 0;
			y = MENU_elementY(3);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [1,100];[100] call cTab_fnc_userMenuSelect;";
		};

		class circlebtn: cTab_MenuItem
		{
			idc = IDC_USRMN_LZBTN;
			text = $STR_ctab_core_CircleMenu;
			x = 0;
			y = MENU_elementY(4);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [1,101];[100] call cTab_fnc_userMenuSelect;";
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


class TextMenu: cTab_RscControlsGroup
{
	#define cTab_MENU_MAX_ELEMENTS 8
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
		class nonebtn: cTab_MenuItem
		{
			idc = IDC_USRMN_HQBTN;
			text = $STR_ctab_core_TSMenu;
			x = 0;
			y = MENU_elementY(1);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [2,0];[1] call cTab_fnc_userMenuSelect;";
		};
		class abtn: cTab_MenuItem
		{
			idc = IDC_USRMN_LZBTN;
			text = $STR_ctab_core_AWithTSMenu;
			x = 0;
			y = MENU_elementY(2);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [2,1];[1] call cTab_fnc_userMenuSelect;";
		};
		class bbtn: cTab_MenuItem
		{
			idc = IDC_USRMN_LZBTN;
			text = $STR_ctab_core_BWithTSMenu;
			x = 0;
			y = MENU_elementY(3);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [2,2];[1] call cTab_fnc_userMenuSelect;";
		};
		class cbtn: cTab_MenuItem
		{
			idc = IDC_USRMN_LZBTN;
			text = $STR_ctab_core_CWithTSMenu;
			x = 0;
			y = MENU_elementY(4);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [2,3];[1] call cTab_fnc_userMenuSelect;";
		};
		class dbtn: cTab_MenuItem
		{
			idc = IDC_USRMN_LZBTN;
			text = $STR_ctab_core_DWithTSMenu;
			x = 0;
			y = MENU_elementY(5);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [2,4];[1] call cTab_fnc_userMenuSelect;";
		};
		class ebtn: cTab_MenuItem
		{
			idc = IDC_USRMN_LZBTN;
			text = $STR_ctab_core_EWithTSMenu;
			x = 0;
			y = MENU_elementY(6);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [2,5];[1] call cTab_fnc_userMenuSelect;";
		};
		class fbtn: cTab_MenuItem
		{
			idc = IDC_USRMN_LZBTN;
			text = $STR_ctab_core_FWithTSMenu;
			x = 0;
			y = MENU_elementY(7);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "cTabUserSelIcon set [2,6];[1] call cTab_fnc_userMenuSelect;";
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