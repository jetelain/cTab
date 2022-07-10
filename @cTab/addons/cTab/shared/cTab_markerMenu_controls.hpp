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
                y = MENU_elementY(11);
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
    #define cTab_MENU_MAX_ELEMENTS 8
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
            toolTip = $STR_ctab_core_RLYHint;
            x = 0;
            y = MENU_elementY(1);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,60];[1] call cTab_fnc_userMenuSelect;";
        };
        class CKPbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_CKPBTN;
            text = $STR_ctab_core_CKPMenu;
            toolTip = $STR_ctab_core_CKPHint;
            x = 0;
            y = MENU_elementY(2);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,61];[1] call cTab_fnc_userMenuSelect;";
        };
        class ppbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_PPBTN;
            text = $STR_ctab_core_PPMenu;
            toolTip = $STR_ctab_core_PPHint;
            x = 0;
            y = MENU_elementY(3);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,62];[1] call cTab_fnc_userMenuSelect;";
        };
        class wpbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_WPBTN;
            text = $STR_ctab_core_WPMenu;
            toolTip = $STR_ctab_core_WPHint;
            x = 0;
            y = MENU_elementY(4);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,63];[1] call cTab_fnc_userMenuSelect;";
        };
        class rpbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_RPBTN;
            text = $STR_ctab_core_RPMenu;
            toolTip = $STR_ctab_core_RPHint;
            x = 0;
            y = MENU_elementY(5);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,64];[1] call cTab_fnc_userMenuSelect;";
        };
        class spbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_SPBTN;
            text = $STR_ctab_core_SPMenu;
            toolTip = $STR_ctab_core_SPHint;
            x = 0;
            y = MENU_elementY(6);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,65];[1] call cTab_fnc_userMenuSelect;";
        };
        class apbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_APBTN;
            text = $STR_ctab_core_APMenu;
            toolTip = $STR_ctab_core_APHint;
            x = 0;
            y = MENU_elementY(7);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,66];[1] call cTab_fnc_userMenuSelect;";
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


class airSub1: cTab_RscControlsGroup
{
    #define cTab_MENU_MAX_ELEMENTS 10
    idc = 3312;
    x = MENU_X;
    y = MENU_Y;
    w = MENU_W;
    h = MENU_H(cTab_MENU_MAX_ELEMENTS);
    class controls
    {
        class IGUIBack_2209: cTab_IGUIBack
        {
            idc = IDC_USRMN_IGUIBACK_2209;
            x = 0;
            y = 0;
            w = MENU_W;
            h = MENU_H(cTab_MENU_MAX_ELEMENTS);
        };
        class aapbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_AAPBTN;
            text = $STR_ctab_core_AAPMenu;
            toolTip = $STR_ctab_core_AAPHint;
            x = 0;
            y = MENU_elementY(1);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,70];[1] call cTab_fnc_userMenuSelect;";
        };
        class abpbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_ABPBTN;
            text = $STR_ctab_core_ABPMenu;
            toolTip = $STR_ctab_core_ABPHint;
            x = 0;
            y = MENU_elementY(2);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,71];[1] call cTab_fnc_userMenuSelect;";
        };
        class acpbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_ACPBTN;
            text = $STR_ctab_core_ACPMenu;
            toolTip = $STR_ctab_core_ACPHint;
            x = 0;
            y = MENU_elementY(3);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,72];[1] call cTab_fnc_userMenuSelect;";
        };
        class Orbitbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_ORBITBTN;
            text = $STR_ctab_core_OrbitMenu;
            toolTip = $STR_ctab_core_OrbitHint;
            x = 0;
            y = MENU_elementY(4);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,73];[1] call cTab_fnc_userMenuSelect;";
        };
        class aepbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_AEPBTN;
            text = $STR_ctab_core_AEPMenu;
            toolTip = $STR_ctab_core_AEPHint;
            x = 0;
            y = MENU_elementY(5);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,74];[1] call cTab_fnc_userMenuSelect;";
        };
        class aipbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_AIPBTN;
            text = $STR_ctab_core_AIPMenu;
            toolTip = $STR_ctab_core_AIPHint;
            x = 0;
            y = MENU_elementY(6);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,75];[1] call cTab_fnc_userMenuSelect;";
        };
        class pupbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_PUPBTN;
            text = $STR_ctab_core_PUPMenu;
            toolTip = $STR_ctab_core_PUPHint;
            x = 0;
            y = MENU_elementY(7);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,76];[1] call cTab_fnc_userMenuSelect;";
        };
        class ACKPbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_ACKPBTN;
            text = $STR_ctab_core_ACKPMenu;
            toolTip = $STR_ctab_core_ACKPHint;
            x = 0;
            y = MENU_elementY(8);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,77];[1] call cTab_fnc_userMenuSelect;";
        };
        class downPbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_DOWNBTN;
            text = $STR_ctab_core_DownMenu;
            toolTip = $STR_ctab_core_DownHint;
            x = 0;
            y = MENU_elementY(9);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,78];[1] call cTab_fnc_userMenuSelect;";
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


class navalSub1: cTab_RscControlsGroup
{
    #define cTab_MENU_MAX_ELEMENTS 7
    idc = 3313;
    x = MENU_X;
    y = MENU_Y;
    w = MENU_W;
    h = MENU_H(cTab_MENU_MAX_ELEMENTS);
    class controls
    {
        class IGUIBack_2210: cTab_IGUIBack
        {
            idc = IDC_USRMN_IGUIBACK_2210;
            x = 0;
            y = 0;
            w = MENU_W;
            h = MENU_H(cTab_MENU_MAX_ELEMENTS);
        };
        class nrpybtn: cTab_MenuItem
        {
            idc = IDC_USRMN_NRPBTN;
            text = $STR_ctab_core_NRPMenu;
            toolTip = $STR_ctab_core_NRPHint;
            x = 0;
            y = MENU_elementY(1);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,80];[1] call cTab_fnc_userMenuSelect;";
        };
        class nspbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_NSPBTN;
            text = $STR_ctab_core_NSPMenu;
            toolTip = $STR_ctab_core_NSPHint;
            x = 0;
            y = MENU_elementY(2);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,81];[1] call cTab_fnc_userMenuSelect;";
        };
        class nnpbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_NNPBTN;
            text = $STR_ctab_core_NNPMenu;
            toolTip = $STR_ctab_core _NNPHint;
            x = 0;
            y = MENU_elementY(3);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,82];[1] call cTab_fnc_userMenuSelect;";
        };
        class ndpbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_NDPBTN;
            text = $STR_ctab_core_NDPMenu;
            toolTip = $STR_ctab_core_NDPHint;
            x = 0;
            y = MENU_elementY(4);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,83];[1] call cTab_fnc_userMenuSelect;";
        };
        class Surfacepbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_SurfaceBTN;
            text = $STR_ctab_core_SurfaceMenu;
            toolTip = $STR_ctab_core_SurfaceHint;
            x = 0;
            y = MENU_elementY(5);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,84];[1] call cTab_fnc_userMenuSelect;";
        };
        class NLPbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_NLPBTN;
            text = $STR_ctab_core_NLPMenu;
            toolTip = $STR_ctab_core_NLPHint;
            x = 0;
            y = MENU_elementY(6);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,85];[1] call cTab_fnc_userMenuSelect;";
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


class zonesSub1: cTab_RscControlsGroup
{
    #define cTab_MENU_MAX_ELEMENTS 17
    idc = 3314;
    x = MENU_X;
    y = MENU_Y;
    w = MENU_W;
    h = MENU_H(cTab_MENU_MAX_ELEMENTS);
    class controls
    {
        class IGUIBack_2211: cTab_IGUIBack
        {
            idc = IDC_USRMN_IGUIBACK_2211;
            x = 0;
            y = 0;
            w = MENU_W;
            h = MENU_H(cTab_MENU_MAX_ELEMENTS);
        };
        class lzirbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_LZIRBTN;
            text = $STR_ctab_core_LZIRMenu;
            toolTip = $STR_ctab_core_LZIRHint;
            x = 0;
            y = MENU_elementY(1);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,200];[1] call cTab_fnc_userMenuSelect;";
        };
        class lzerbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_LZERBTN;
            text = $STR_ctab_core_LZERMenu;
            toolTip = $STR_ctab_core_LZERHint;
            x = 0;
            y = MENU_elementY(2);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,201];[1] call cTab_fnc_userMenuSelect;";
        };
        class dzrbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_DZRBTN;
            text = $STR_ctab_core_DZRMenu;
            toolTip = $STR_ctab_core_DZRHint;
            x = 0;
            y = MENU_elementY(3);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,202];[1] call cTab_fnc_userMenuSelect;";
        };
        class flzrbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_FLZRBTN;
            text = $STR_ctab_core_FLZRMenu;
            toolTip = $STR_ctab_core_FLZRHint;
            x = 0;
            y = MENU_elementY(4);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,203];[1] call cTab_fnc_userMenuSelect;";
        };
        class lzigbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_LZIGBTN;
            text = $STR_ctab_core_LZIGMenu;
            toolTip = $STR_ctab_core_LZIGHint;
            x = 0;
            y = MENU_elementY(5);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,210];[1] call cTab_fnc_userMenuSelect;";
        };
        class lzegbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_LZEGBTN;
            text = $STR_ctab_core_LZEGMenu;
            toolTip = $STR_ctab_core_LZEGHint;
            x = 0;
            y = MENU_elementY(6);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,211];[1] call cTab_fnc_userMenuSelect;";
        };
        class dzgbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_DZGBTN;
            text = $STR_ctab_core_DZGMenu;
            toolTip = $STR_ctab_core_DZGHint;
            x = 0;
            y = MENU_elementY(7);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,212];[1] call cTab_fnc_userMenuSelect;";
        };
        class flzgbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_FLZGBTN;
            text = $STR_ctab_core_FLZGMenu;
            toolTip = $STR_ctab_core_FLZGHint;
            x = 0;
            y = MENU_elementY(8);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,213];[1] call cTab_fnc_userMenuSelect;";
        };
        class lzibbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_LZIBBTN;
            text = $STR_ctab_core_LZIBMenu;
            toolTip = $STR_ctab_core_LZIBHint;
            x = 0;
            y = MENU_elementY(9);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,220];[1] call cTab_fnc_userMenuSelect;";
        };
        class lzebbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_LZEBBTN;
            text = $STR_ctab_core_LZEBMenu;
            toolTip = $STR_ctab_core_LZEBHint;
            x = 0;
            y = MENU_elementY(10);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,221];[1] call cTab_fnc_userMenuSelect;";
        };
        class dzbbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_DZBBTN;
            text = $STR_ctab_core_DZBMenu;
            toolTip = $STR_ctab_core_DZBHint;
            x = 0;
            y = MENU_elementY(11);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,222];[1] call cTab_fnc_userMenuSelect;";
        };
        class flzbbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_FLZBBTN;
            text = $STR_ctab_core_FLZBMenu;
            toolTip = $STR_ctab_core_FLZBHint;
            x = 0;
            y = MENU_elementY(12);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,223];[1] call cTab_fnc_userMenuSelect;";
        };
        class lzibtn: cTab_MenuItem
        {
            idc = IDC_USRMN_LZIBTN;
            text = $STR_ctab_core_LZIMenu;
            toolTip = $STR_ctab_core_LZIHint;
            x = 0;
            y = MENU_elementY(13);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,230];[1] call cTab_fnc_userMenuSelect;";
        };
        class lzebtn: cTab_MenuItem
        {
            idc = IDC_USRMN_LZEBTN;
            text = $STR_ctab_core_LZEMenu;
            toolTip = $STR_ctab_core_LZEHint;
            x = 0;
            y = MENU_elementY(14);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,231];[1] call cTab_fnc_userMenuSelect;";
        };
        class dzbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_DZBTN;
            text = $STR_ctab_core_DZMenu;
            toolTip = $STR_ctab_core_DZHint;
            x = 0;
            y = MENU_elementY(15);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,232];[1] call cTab_fnc_userMenuSelect;";
        };
        class flzbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_FLZBTN;
            text = $STR_ctab_core_FLZMenu;
            toolTip = $STR_ctab_core_FLZHint;
            x = 0;
            y = MENU_elementY(16);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,233];[1] call cTab_fnc_userMenuSelect;";
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

class goSub1: cTab_RscControlsGroup
{
    #define cTab_MENU_MAX_ELEMENTS 5
    idc = 3310;
    x = MENU_X;
    y = MENU_Y;
    w = MENU_W;
    h = MENU_H(cTab_MENU_MAX_ELEMENTS);
    class controls
    {
        class IGUIBack_2212: cTab_IGUIBack
        {
            idc = IDC_USRMN_IGUIBACK_2212;
            x = 0;
            y = 0;
            w = MENU_W;
            h = MENU_H(cTab_MENU_MAX_ELEMENTS);
        };
        class beerbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_BEERBTN;
            text = $STR_ctab_core_BeerMenu;
            x = 0;
            y = MENU_elementY(1);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "[62] call cTab_fnc_userMenuSelect;";
        };
        class elementsbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_ELEMBTN;
            text = $STR_ctab_core_ElementsMenu;
            x = 0;
            y = MENU_elementY(2);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "[63] call cTab_fnc_userMenuSelect;";
        };
        class riversbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_RIVERBTN;
            text = $STR_ctab_core_RiversMenu;
            x = 0;
            y = MENU_elementY(3);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "[64] call cTab_fnc_userMenuSelect;";
        };
        class toonbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_TOONBTN;
            text = $STR_ctab_core_ToonMenu;
            x = 0;
            y = MENU_elementY(4);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "[65] call cTab_fnc_userMenuSelect;";
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



class beerSub1: cTab_RscControlsGroup
{
    #define cTab_MENU_MAX_ELEMENTS 11
    idc = 3315;
    x = MENU_X;
    y = MENU_Y;
    w = MENU_W;
    h = MENU_H(cTab_MENU_MAX_ELEMENTS);
    class controls
    {
        class IGUIBack_2213: cTab_IGUIBack
        {
            idc = IDC_USRMN_IGUIBACK_2213;
            x = 0;
            y = 0;
            w = MENU_W;
            h = MENU_H(cTab_MENU_MAX_ELEMENTS);
        };
        class Budweiserbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_BUDWEISERBTN;
            text = "Budweiser";
            x = 0;
            y = MENU_elementY(1);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,100];[1] call cTab_fnc_userMenuSelect;";
        };
        class Coorsbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_COORSBTN;
            text = "Coors";
            x = 0;
            y = MENU_elementY(2);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,101];[1] call cTab_fnc_userMenuSelect;";
        };
        class Coronabtn: cTab_MenuItem
        {
            idc = IDC_USRMN_CORONABTN;
            text = "Corona";
            x = 0;
            y = MENU_elementY(3);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,102];[1] call cTab_fnc_userMenuSelect;";
        };
        class Guinnessbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_GUINNESSBTN;
            text = "Guinness";
            x = 0;
            y = MENU_elementY(4);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,103];[1] call cTab_fnc_userMenuSelect;";
        };
        class Hammsbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_HAMMSBTN;
            text = "Hamms";
            x = 0;
            y = MENU_elementY(5);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,104];[1] call cTab_fnc_userMenuSelect;";
        };
        class Heinekenbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_HEINEKENBTN;
            text = "Heineken";
            x = 0;
            y = MENU_elementY(6);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,105];[1] call cTab_fnc_userMenuSelect;";
        };
        class Keystonebtn: cTab_MenuItem
        {
            idc = IDC_USRMN_KEYSTRONEBTN;
            text = "Keystone";
            x = 0;
            y = MENU_elementY(7);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,106];[1] call cTab_fnc_userMenuSelect;";
        };
        class Millerbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_MILLERBTN;
            text = "Miller";
            x = 0;
            y = MENU_elementY(8);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,107];[1] call cTab_fnc_userMenuSelect;";
        };
        class Pabstbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_PABSTBTN;
            text = "Pabst";
            x = 0;
            y = MENU_elementY(9);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,108];[1] call cTab_fnc_userMenuSelect;";
        };
        class Schlitzbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_SCHLITZBTN;
            text = "Schlitz";
            x = 0;
            y = MENU_elementY(10);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,109];[1] call cTab_fnc_userMenuSelect;";
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


class elementsSub1: cTab_RscControlsGroup
{
    #define cTab_MENU_MAX_ELEMENTS 11
    idc = 3316;
    x = MENU_X;
    y = MENU_Y;
    w = MENU_W;
    h = MENU_H(cTab_MENU_MAX_ELEMENTS);
    class controls
    {
        class IGUIBack_2214: cTab_IGUIBack
        {
            idc = IDC_USRMN_IGUIBACK_2214;
            x = 0;
            y = 0;
            w = MENU_W;
            h = MENU_H(cTab_MENU_MAX_ELEMENTS);
        };
        class Argonbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_ARGONBTN;
            text = "Argon";
            x = 0;
            y = MENU_elementY(1);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,110];[1] call cTab_fnc_userMenuSelect;";
        };
        class Boronbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_BORONBTN;
            text = "Boron";
            x = 0;
            y = MENU_elementY(2);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,111];[1] call cTab_fnc_userMenuSelect;";
        };
        class Carbonbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_CARBONBTN;
            text = "Carbon";
            x = 0;
            y = MENU_elementY(3);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,112];[1] call cTab_fnc_userMenuSelect;";
        };
        class Goldbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_GOLDBTN;
            text = "Gold";
            x = 0;
            y = MENU_elementY(4);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,113];[1] call cTab_fnc_userMenuSelect;";
        };
        class Ironbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_IRONBTN;
            text = "Iron";
            x = 0;
            y = MENU_elementY(5);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,114];[1] call cTab_fnc_userMenuSelect;";
        };
        class Leadbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_LEADBTN;
            text = "Lead";
            x = 0;
            y = MENU_elementY(6);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,115];[1] call cTab_fnc_userMenuSelect;";
        };
        class Neonbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_NEONBTN;
            text = "Neon";
            x = 0;
            y = MENU_elementY(7);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,116];[1] call cTab_fnc_userMenuSelect;";
        };
        class silverbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_SILVERBTN;
            text = "Silver";
            x = 0;
            y = MENU_elementY(8);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,117];[1] call cTab_fnc_userMenuSelect;";
        };
        class tinbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_TINBTN;
            text = "Tin";
            x = 0;
            y = MENU_elementY(9);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,118];[1] call cTab_fnc_userMenuSelect;";
        };
        class Zincbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_ZINCBTN;
            text = "Zinc";
            x = 0;
            y = MENU_elementY(10);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,119];[1] call cTab_fnc_userMenuSelect;";
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


class riversSub1: cTab_RscControlsGroup
{
    #define cTab_MENU_MAX_ELEMENTS 11
    idc = 3317;
    x = MENU_X;
    y = MENU_Y;
    w = MENU_W;
    h = MENU_H(cTab_MENU_MAX_ELEMENTS);
    class controls
    {
        class IGUIBack_2215: cTab_IGUIBack
        {
            idc = IDC_USRMN_IGUIBACK_2215;
            x = 0;
            y = 0;
            w = MENU_W;
            h = MENU_H(cTab_MENU_MAX_ELEMENTS);
        };
        class Amazonbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_AMAZONBTN;
            text = "Amazon";
            x = 0;
            y = MENU_elementY(1);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,120];[1] call cTab_fnc_userMenuSelect;";
        };
        class Congobtn: cTab_MenuItem
        {
            idc = IDC_USRMN_CONGOBTN;
            text = "Congo";
            x = 0;
            y = MENU_elementY(2);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,121];[1] call cTab_fnc_userMenuSelect;";
        };
        class Mekongbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_MEKONGBTN;
            text = "Mekong";
            x = 0;
            y = MENU_elementY(3);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,122];[1] call cTab_fnc_userMenuSelect;";
        };
        class Nilebtn: cTab_MenuItem
        {
            idc = IDC_USRMN_NILEBTN;
            text = "Nile";
            x = 0;
            y = MENU_elementY(4);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,123];[1] call cTab_fnc_userMenuSelect;";
        };
        class Riobtn: cTab_MenuItem
        {
            idc = IDC_USRMN_RIOBTN;
            text = "Rio";
            x = 0;
            y = MENU_elementY(5);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,124];[1] call cTab_fnc_userMenuSelect;";
        };
        class Rombtn: cTab_MenuItem
        {
            idc = IDC_USRMN_ROMBTN;
            text = "Rom";
            x = 0;
            y = MENU_elementY(6);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,125];[1] call cTab_fnc_userMenuSelect;";
        };
        class Volgabtn: cTab_MenuItem
        {
            idc = IDC_USRMN_VOLGABTN;
            text = "Volga";
            x = 0;
            y = MENU_elementY(7);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,126];[1] call cTab_fnc_userMenuSelect;";
        };
        class Yangtzebtn: cTab_MenuItem
        {
            idc = IDC_USRMN_YANGTZEBTN;
            text = "Yangtze";
            x = 0;
            y = MENU_elementY(8);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,127];[1] call cTab_fnc_userMenuSelect;";
        };
        class Yellowbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_YELLOWBTN;
            text = "Yellow";
            x = 0;
            y = MENU_elementY(9);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,128];[1] call cTab_fnc_userMenuSelect;";
        };
        class Yukonbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_YUKONBTN;
            text = "Yukon";
            x = 0;
            y = MENU_elementY(10);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,129];[1] call cTab_fnc_userMenuSelect;";
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


class toonSub1: cTab_RscControlsGroup
{
    #define cTab_MENU_MAX_ELEMENTS 11
    idc = 3318;
    x = MENU_X;
    y = MENU_Y;
    w = MENU_W;
    h = MENU_H(cTab_MENU_MAX_ELEMENTS);
    class controls
    {
        class IGUIBack_2216: cTab_IGUIBack
        {
            idc = IDC_USRMN_IGUIBACK_2216;
            x = 0;
            y = 0;
            w = MENU_W;
            h = MENU_H(cTab_MENU_MAX_ELEMENTS);
        };
        class Batmanbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_BATMANBTN;
            text = "Batman";
            x = 0;
            y = MENU_elementY(1);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,130];[1] call cTab_fnc_userMenuSelect;";
        };
        class Daffybtn: cTab_MenuItem
        {
            idc = IDC_USRMN_DAFFYBTN;
            text = "Daffy";
            x = 0;
            y = MENU_elementY(2);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,131];[1] call cTab_fnc_userMenuSelect;";
        };
        class Goofybtn: cTab_MenuItem
        {
            idc = IDC_USRMN_GOOFYBTN;
            text = "Goofy";
            x = 0;
            y = MENU_elementY(3);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,132];[1] call cTab_fnc_userMenuSelect;";
        };
        class Homerbtn: cTab_MenuItem
        {
            idc = IDC_USRMN_HOMERBTN;
            text = "Homer";
            x = 0;
            y = MENU_elementY(4);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,133];[1] call cTab_fnc_userMenuSelect;";
        };
        class Jerrybtn: cTab_MenuItem
        {
            idc = IDC_USRMN_JERRYBTN;
            text = "Jerry";
            x = 0;
            y = MENU_elementY(5);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,134];[1] call cTab_fnc_userMenuSelect;";
        };
        class Rockybtn: cTab_MenuItem
        {
            idc = IDC_USRMN_ROCKYBTN;
            text = "Rocky";
            x = 0;
            y = MENU_elementY(6);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,135];[1] call cTab_fnc_userMenuSelect;";
        };
        class Scobbybtn: cTab_MenuItem
        {
            idc = IDC_USRMN_SCOBBYBTN;
            text = "Scobby";
            x = 0;
            y = MENU_elementY(7);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,136];[1] call cTab_fnc_userMenuSelect;";
        };
        class tombtn: cTab_MenuItem
        {
            idc = IDC_USRMN_TOMBTN;
            text = "tom";
            x = 0;
            y = MENU_elementY(8);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,137];[1] call cTab_fnc_userMenuSelect;";
        };
        class Woodybtn: cTab_MenuItem
        {
            idc = IDC_USRMN_WOODYBTN;
            text = "Woody";
            x = 0;
            y = MENU_elementY(9);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,138];[1] call cTab_fnc_userMenuSelect;";
        };
        class Yogibtn: cTab_MenuItem
        {
            idc = IDC_USRMN_YOGIBTN;
            text = "Yogi";
            x = 0;
            y = MENU_elementY(10);
            w = MENU_W;
            h = MENU_elementH;
            sizeEx = MENU_sizeEx;
            action = "cTabUserSelIcon set [1,139];[1] call cTab_fnc_userMenuSelect;";
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