#include "script_component.hpp"

class CfgPatches {
    class ADDON {
        name = QUOTE(COMPONENT);
        units[] = {};
        weapons[] = {};
        requiredVersion = REQUIRED_VERSION;
        requiredAddons[] = {"ctab_main", "ctab_core", "ace_markers"};
        author = "GrueArbre";
        VERSION_CONFIG;
    };
};

#include "CfgEventHandlers.hpp"
class CfgMarkerColors
{
    class Transparent
    {
		name="APP-6D Icon";
        color[]={0, 0, 0, 0};
        scope=0;
    };
};
class CfgMarkers
{
    class b_unknown;
    class ctab_app6d_generic : b_unknown
    {
		scope=2;
        author="GrueArbre";
		name="APP-6D Icon";
        icon="\A3\ui_f\data\map\Markers\System\dummy_ca.paa";
        size=64;
		showEditorMarkerColor = 0;
		color[] = { 0, 0, 0, 0 };
    };
};


class CfgFontFamilies
{	
	class TahomaBLineZero
	{
		fonts[]=
		{
			"z\ctab\addons\app6d\data\fonts\TahomaBLineZero8",
			"z\ctab\addons\app6d\data\fonts\TahomaBLineZero16",
			"z\ctab\addons\app6d\data\fonts\TahomaBLineZero32"
		};
	};
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

class RscDisplayInsertMarker {
    onLoad = QUOTE(_this call FUNC(initInsertMarker));
    onUnload = QUOTE(_this call FUNC(placeMarker));
};
class RscButton;
class GVAR(valueButton): RscButton {
    colorText[] = {0, 0, 0, 0};
    colorBackground[] = {0, 0, 0, 0};
    colorFocused[] = {0, 0, 0, 0};
    colorShadow[] = {0, 0, 0, 0};
    colorBorder[] = {0, 0, 0, 0};
    colorBackgroundActive[] = {0, 0, 0, 0};
    colorDisabled[] = {0, 0, 0, 0};
    colorBackgroundDisabled[] = {0, 0, 0, 0};
};