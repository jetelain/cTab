#define CTAB_COMPASS controls[] += {QGVARMAIN(compass)}; class GVARMAIN(compass) : GVARMAIN(HorizontalCompass) { };

class RscInGameUI 
{
    class RscUnitInfo;
    class RscOptics_MBT_01_commander : RscUnitInfo {
        CTAB_COMPASS
    };
    class RscOptics_MBT_01_gunner : RscUnitInfo {
        CTAB_COMPASS
    };
    class RscOptics_APC_Wheeled_01_gunner : RscUnitInfo {
        CTAB_COMPASS
    };
    class RscOptics_APC_Wheeled_03_commander : RscUnitInfo {
        CTAB_COMPASS
    };
    class RscOptics_APC_Wheeled_03_gunner : RscUnitInfo {
        CTAB_COMPASS
    };

	/* 
	Add ?
		RscOptics_MBT_02_commander
		RscOptics_MBT_02_gunner
		RscOptics_MBT_03_gunner
		RscOptics_APC_Tracked_01_gunner
		RscOptics_APC_Tracked_03_gunner
	*/

};
