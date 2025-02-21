#include "script_component.hpp"

class CfgPatches {
    class ADDON {
        name = QUOTE(COMPONENT);
        units[] = {};
        weapons[] = {};
        requiredVersion = REQUIRED_VERSION;
        requiredAddons[] = {"ctab_main", "ctab_core"};
        author = "GrueArbre";
        VERSION_CONFIG;
    };
};

#include "CfgEventHandlers.hpp"

class CfgFontFamilies
{
	class TahomaBLineOne
	{
		fonts[]=
		{
			"z\ctab\addons\app6d\data\fonts\TahomaBLineOne8",
			"z\ctab\addons\app6d\data\fonts\TahomaBLineOne16",
			"z\ctab\addons\app6d\data\fonts\TahomaBLineOne32"
		};
	};
	class TahomaBLineTwo
	{
		fonts[]=
		{
			"z\ctab\addons\app6d\data\fonts\TahomaBLineTwo8",
			"z\ctab\addons\app6d\data\fonts\TahomaBLineTwo16",
			"z\ctab\addons\app6d\data\fonts\TahomaBLineTwo32"
		};
	};
	class TahomaBLineThree
	{
		fonts[]=
		{
			"z\ctab\addons\app6d\data\fonts\TahomaBLineThree8",
			"z\ctab\addons\app6d\data\fonts\TahomaBLineThree16",
			"z\ctab\addons\app6d\data\fonts\TahomaBLineThree32"
		};
	};
};