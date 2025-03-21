#include "script_component.hpp"

class CfgPatches {
    class ADDON {
        name = QUOTE(COMPONENT);
        units[] = {};
        weapons[] = {};
        requiredVersion = REQUIRED_VERSION;
        requiredAddons[] = {"ctab_irl_main", "HATE_DSLR"};
        author = "AUTHOR";
        VERSION_CONFIG;
        skipWhenMissingDependencies = 1;
    };
};

#include "CfgEventHandlers.hpp"

// Override HATE DSLR functions
class CfgFunctions
{
    class HATE
    {
        tag="HATE";
        class CAMERA
        {
            file="\z\ctab_irl\addons\camera\functions";
            class addAction
            {
                postInit=1;
            };
            class takePicture{};
        };
    };
};
