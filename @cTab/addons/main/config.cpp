#include "script_component.hpp"

class CfgPatches {
    class ADDON {
        name = QUOTE(COMPONENT);
        units[] = {};
        weapons[] = {};
        requiredVersion = REQUIRED_VERSION;
        requiredAddons[] = {};
		author = "cTab Authors";
		authors[] = {"Gundy","Riouken","Raspu","GrueArbre"};
		authorUrl = "https://github.com/jetelain/cTab";
        VERSION_CONFIG;
    };
};
