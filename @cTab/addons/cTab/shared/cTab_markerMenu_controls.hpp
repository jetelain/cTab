#define CTAB_MENU_ENTRY(label,index,pos,value,next) \
		class btn##index: cTab_MenuItem \
		{ \
			idc = -1; text = label; \
			x = 0; y = MENU_elementY(index); w = MENU_W; h = MENU_elementH; sizeEx = MENU_sizeEx; \
			action = QUOTE(cTabUserSelIcon set [ARR_2(pos,value)];[next] call cTab_fnc_userMenuSelect;); \
		};

#define CTAB_MENU_ENTRY_ICON(label,index,pos,value,next,ICON) \
		class btn##index: cTab_MenuItem \
		{ \
			idc = -1; text = label; \
			x = 0; y = MENU_elementY(index); w = MENU_W; h = MENU_elementH; sizeEx = MENU_sizeEx; \
			action = QUOTE(cTabUserSelIcon set [ARR_2(pos,value)];[next] call cTab_fnc_userMenuSelect;); \
			textureNoShortcut =  QUOTE(\ctab\img\menu\ICON.paa);\
		};

#define CTAB_MENU_ENTRY_NAV(label,index,next) \
		class btn##index: cTab_MenuItem \
		{ \
			idc = -1; text = label; \
			x = 0; y = MENU_elementY(index); w = MENU_W; h = MENU_elementH; sizeEx = MENU_sizeEx; \
			action = QUOTE([next] call cTab_fnc_userMenuSelect;); \
		};

#define CTAB_MENU_ENTRY_NAVICON(label,index,next,ICON) \
		class btn##index: cTab_MenuItem \
		{ \
			idc = -1; text = label; \
			x = 0; y = MENU_elementY(index); w = MENU_W; h = MENU_elementH; sizeEx = MENU_sizeEx; \
			action = QUOTE([next] call cTab_fnc_userMenuSelect;); \
			textureNoShortcut =  QUOTE(\ctab\img\menu\ICON.paa);\
		};

#define CTAB_MENU_ENTRY_EXIT(index) CTAB_MENU_ENTRY_NAV($STR_ctab_core_MenuExit,index,0)

#define CTAB_MENU_BACKGROUND(size) \
		class background : cTab_IGUIBack \
		{ \
			idc = -1; \
			x = 0; y = 0; w = MENU_W; h = MENU_H(size); \
		};

class MainSubmenu: cTab_RscControlsGroup
{
	#ifndef cTab_IS_TABLET
		#define cTab_MENU_MAX_ELEMENTS 7
	#else
		#define cTab_MENU_MAX_ELEMENTS 8
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
			textureNoShortcut = "\ctab\img\menu\10061000000000000000.paa";
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
			textureNoShortcut = "\ctab\img\menu\mil_join.paa";
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
			textureNoShortcut = "\ctab\img\menu\mil_dot_blue.paa";
		};
		CTAB_MENU_ENTRY_NAVICON($STR_ctab_core_ControlPointMenu,4,101,10032500001301000000)
		CTAB_MENU_ENTRY_NAVICON($STR_ctab_core_ManoeuvreMenu,5,102,10032500001602050000)
		CTAB_MENU_ENTRY_NAVICON($STR_ctab_core_SustainmentMenu,6,103,10032500003211000000)
		#ifdef cTab_IS_TABLET
			class lockUavCam: cTab_MenuItem
			{
				idc = -1;
				text = $STR_ctab_core_UAVLockMenu;
				toolTip = $STR_ctab_core_UAVLockHint;
				x = 0;
				y = MENU_elementY(7);
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
	#define cTab_MENU_MAX_ELEMENTS 10
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
			textureNoShortcut = "\ctab\img\menu\10061000001211000000.paa";
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
			textureNoShortcut = "\ctab\img\menu\10061000001211020000.paa";
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
			textureNoShortcut = "\ctab\img\menu\10061000001211040000.paa";
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
			textureNoShortcut = "\ctab\img\menu\10061000001205000000.paa";
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
			textureNoShortcut = "\ctab\img\menu\10061000001206000000.paa";
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
			textureNoShortcut = "\ctab\img\menu\10061000001208000000.paa";
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
			textureNoShortcut = "\ctab\img\menu\10061000000000000000.paa";
		};
		CTAB_MENU_ENTRY_ICON($STR_ctab_core_Mine,8,1,80,1,10061500002101000000)
		CTAB_MENU_ENTRY_ICON($STR_ctab_core_IED,9,1,81,1,10061500002104000000)
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
			textureNoShortcut = "\ctab\img\menu\size11.paa";
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
			textureNoShortcut = "\ctab\img\menu\size12.paa";
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
			textureNoShortcut = "\ctab\img\menu\size13.paa";
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
			textureNoShortcut = "\ctab\img\menu\size14.paa";
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
	#define cTab_MENU_MAX_ELEMENTS 6
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


		#define CTAB_MENU_ENTRY_COMPASS(label,ROW,COL,value,ICON) \
			class btn##ICON: cTab_MenuItem \
			{ \
				idc = -1; text = label; \
				x = MENU_W*COL/3; y = MENU_elementY(ROW); w = MENU_W/3; h = MENU_elementH; sizeEx = MENU_sizeEx; \
				action = QUOTE(cTabUserSelIcon set [ARR_2(3,value)];[1] call cTab_fnc_userMenuSelect;); \
				textureNoShortcut =  QUOTE(\ctab\img\menu\ICON.paa);\
			};
		class prompt: cTab_RscText
		{
			idc = -1;
			text = $STR_ctab_core_WhereIsTheUnitMoving;
			x = 0;
			y = MENU_elementY(1);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
		};

		class stnbtn: cTab_MenuItem
		{
			idc = IDC_USRMN_STNBTN;
			text = $STR_ctab_core_StationaryMenu;
			x = 0;
			y = MENU_elementY(2);
			w = MENU_W;
			h = MENU_elementH;
			sizeEx = MENU_sizeEx;
			action = "[1] call cTab_fnc_userMenuSelect;";
			textureNoShortcut = "\ctab\img\menu\nope.paa";
		};

		CTAB_MENU_ENTRY_COMPASS($STR_ctab_core_NorthWest,3,0,8,NW)
		CTAB_MENU_ENTRY_COMPASS($STR_ctab_core_North,3,1,1,N)
		CTAB_MENU_ENTRY_COMPASS($STR_ctab_core_NorthEast,3,2,2,NE)

		CTAB_MENU_ENTRY_COMPASS($STR_ctab_core_West,4,0,7,W)
		CTAB_MENU_ENTRY_COMPASS($STR_ctab_core_StationaryMenu,4,1,0,nope)
		CTAB_MENU_ENTRY_COMPASS($STR_ctab_core_East,4,2,3,E)

		CTAB_MENU_ENTRY_COMPASS($STR_ctab_core_SouthWest,5,0,6,SW)
		CTAB_MENU_ENTRY_COMPASS($STR_ctab_core_South,5,1,5,S)
		CTAB_MENU_ENTRY_COMPASS($STR_ctab_core_SouthEast,5,2,4,SE)

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
			textureNoShortcut = "\ctab\img\menu\10061500001100000000.paa";
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
			textureNoShortcut = "\ctab\img\menu\10062700001103010000.paa";
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
			textureNoShortcut = "\ctab\img\menu\10062700001103160000.paa";
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
			textureNoShortcut = "\ctab\img\menu\10062700001103030000.paa";
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
			textureNoShortcut = "\ctab\img\menu\10062700001103070000.paa";
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
			textureNoShortcut = "\ctab\img\menu\10061500001111000000.paa";
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
			textureNoShortcut = "\ctab\img\menu\10062700001103140000.paa";
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
			textureNoShortcut = "\ctab\img\menu\mil_join.paa";
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
			textureNoShortcut = "\ctab\img\menu\mil_circle.paa";
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
			textureNoShortcut = "\ctab\img\menu\10032000001122020000.paa";
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
			textureNoShortcut = "\ctab\img\menu\mil_warning.paa";
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
			textureNoShortcut = "\ctab\img\menu\10031002000000000000.paa";
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
			textureNoShortcut = "\ctab\img\menu\mil_end.paa";
		};
		CTAB_MENU_ENTRY_ICON($STR_ctab_core_DotMenu,3,1,100,100,mil_dot_blue)
		CTAB_MENU_ENTRY_ICON($STR_ctab_core_CircleMenu,4,1,101,100,mil_circle_blue)
		CTAB_MENU_ENTRY_EXIT(cTab_MENU_MAX_ELEMENTS)
	};
};

class MenuCustomText: cTab_RscControlsGroup
{
	#define cTab_MENU_MAX_ELEMENTS 8
	idc = 3308;
	x = MENU_X;
	y = MENU_Y;
	w = MENU_W;
	h = MENU_H(cTab_MENU_MAX_ELEMENTS);
	class controls
	{
		CTAB_MENU_BACKGROUND(cTab_MENU_MAX_ELEMENTS)
		CTAB_MENU_ENTRY($STR_ctab_core_TSMenu,1,2,0,1)
		CTAB_MENU_ENTRY($STR_ctab_core_AWithTSMenu,2,2,1,1)
		CTAB_MENU_ENTRY($STR_ctab_core_BWithTSMenu,3,2,2,1)
		CTAB_MENU_ENTRY($STR_ctab_core_CWithTSMenu,4,2,3,1)
		CTAB_MENU_ENTRY($STR_ctab_core_DWithTSMenu,5,2,4,1)
		CTAB_MENU_ENTRY($STR_ctab_core_EWithTSMenu,6,2,5,1)
		CTAB_MENU_ENTRY($STR_ctab_core_FWithTSMenu,7,2,6,1)
		CTAB_MENU_ENTRY_EXIT(cTab_MENU_MAX_ELEMENTS)
	};
};

class MenuControlPoint: cTab_RscControlsGroup
{
	#define cTab_MENU_MAX_ELEMENTS 9
	idc = 3309;
	x = MENU_X;
	y = MENU_Y;
	w = MENU_W;
	h = MENU_H(cTab_MENU_MAX_ELEMENTS);
	class controls
	{
		CTAB_MENU_BACKGROUND(cTab_MENU_MAX_ELEMENTS)
		CTAB_MENU_ENTRY_ICON($STR_ctab_core_PointUnspec,1,1,200,1,10032500001301000000)
		CTAB_MENU_ENTRY_ICON($STR_ctab_core_PointContact,2,1,102,1,10032500001305000000)
		CTAB_MENU_ENTRY_ICON($STR_ctab_core_PointCoord,3,1,103,1,10032500001306000000)
		CTAB_MENU_ENTRY_ICON($STR_ctab_core_PointCKP,4,1,201,1,10032500001303000000)
		CTAB_MENU_ENTRY_ICON($STR_ctab_core_PointSOS,5,1,202,1,10032500001308000000)
		CTAB_MENU_ENTRY_ICON($STR_ctab_core_PointEC,6,1,203,1,10032500001309000000)
		CTAB_MENU_ENTRY_ICON($STR_ctab_core_PointRLY,7,1,204,1,10032500001314000000)
		CTAB_MENU_ENTRY_ICON($STR_ctab_core_PointSP,8,1,205,1,10032500001316000000)
		CTAB_MENU_ENTRY_EXIT(cTab_MENU_MAX_ELEMENTS)
	};
};

class MenuManoeuvre: cTab_RscControlsGroup
{
	#define cTab_MENU_MAX_ELEMENTS 5
	idc = 3310;
	x = MENU_X;
	y = MENU_Y;
	w = MENU_W;
	h = MENU_H(cTab_MENU_MAX_ELEMENTS);
	class controls
	{
		CTAB_MENU_BACKGROUND(cTab_MENU_MAX_ELEMENTS)
		CTAB_MENU_ENTRY_ICON($STR_ctab_core_Outpost,1,1,104,1,10032500001601000000)
		CTAB_MENU_ENTRY_ICON($STR_ctab_core_CombatOutpost,2,1,105,1,10032500001602050000)
		CTAB_MENU_ENTRY_ICON($STR_ctab_core_PointTarget,3,1,106,1,10032500001603000000)
		CTAB_MENU_ENTRY_ICON($STR_ctab_core_PointPD,4,1,206,1,10032500001604000000)
		CTAB_MENU_ENTRY_EXIT(cTab_MENU_MAX_ELEMENTS)
	};
};


class MenuSustainment: cTab_RscControlsGroup
{
	#define cTab_MENU_MAX_ELEMENTS 5
	idc = 3311;
	x = MENU_X;
	y = MENU_Y;
	w = MENU_W;
	h = MENU_H(cTab_MENU_MAX_ELEMENTS);
	class controls
	{
		CTAB_MENU_BACKGROUND(cTab_MENU_MAX_ELEMENTS)
		CTAB_MENU_ENTRY_ICON($STR_ctab_core_PointCCP,1,1,207,1,10032500003205000000)
		CTAB_MENU_ENTRY_ICON($STR_ctab_core_PointDET,2,1,208,1,10032500003207000000)
		CTAB_MENU_ENTRY_ICON($STR_ctab_core_PointMED,3,1,209,1,10032500003211000000)
		CTAB_MENU_ENTRY_ICON($STR_ctab_core_PointR3P,4,1,210,1,10032500003212000000)
		CTAB_MENU_ENTRY_EXIT(cTab_MENU_MAX_ELEMENTS)
	};
};