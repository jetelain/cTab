#include "script_component.hpp"

class CfgPatches {
    class ADDON {
        name = QUOTE(COMPONENT);
        units[] = {};
        weapons[] = {};
        requiredVersion = REQUIRED_VERSION;
        requiredAddons[] = {"ctab_irl_main", "ctab_core"};
        author = "AUTHOR";
        VERSION_CONFIG;
    };
};


#include "CfgEventHandlers.hpp"

class CfgFontFamilies
{
	class QRFONT
	{
		fonts[]=
		{
			"z\ctab_irl\addons\connect\data\qrfont6",
			"z\ctab_irl\addons\connect\data\qrfont7",
			"z\ctab_irl\addons\connect\data\qrfont8",
			"z\ctab_irl\addons\connect\data\qrfont9",
			"z\ctab_irl\addons\connect\data\qrfont10",
			"z\ctab_irl\addons\connect\data\qrfont11",
			"z\ctab_irl\addons\connect\data\qrfont12",
			"z\ctab_irl\addons\connect\data\qrfont13",
			"z\ctab_irl\addons\connect\data\qrfont14",
			"z\ctab_irl\addons\connect\data\qrfont15",
			"z\ctab_irl\addons\connect\data\qrfont16",
			"z\ctab_irl\addons\connect\data\qrfont17",
			"z\ctab_irl\addons\connect\data\qrfont18",
			"z\ctab_irl\addons\connect\data\qrfont19"
		};
	};
};