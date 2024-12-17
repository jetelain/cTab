#include "script_component.hpp"

class CfgPatches {
    class ADDON {
        name = QUOTE(COMPONENT);
        units[] = {};
        weapons[] = {};
        requiredVersion = REQUIRED_VERSION;
        requiredAddons[] = {"ctab_main"};
        author = "AUTHOR";
        VERSION_CONFIG;
    };
};

class CfgCommands
{
	allowedHTMLLoadURIs[] +=
	{
		"http://localhost:5000/*",
		"https://ctab.plan-ops.fr/*"
	};
};

#include "CfgEventHandlers.hpp"
#include "CfgVehicles.hpp"
#include "CfgWeapons.hpp"
#include "ui\technicaldata.hpp"