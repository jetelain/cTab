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