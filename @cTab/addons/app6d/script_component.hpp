#define COMPONENT app6d
#include "\z\ctab\addons\main\script_mod.hpp"

#define BASELINE_ICON_SIZE 96
#define DEFAULT_SIDC "10031000000000000000"

#define IDC_LB_SET 9905
#define IDC_LB_AMP 9906
#define IDC_LB_MOD1 9908
#define IDC_LB_MOD2 9909
#define IDC_LB_ICON 9907

 #define DEBUG_MODE_FULL
 #define DISABLE_COMPILE_CACHE

#ifdef DEBUG_ENABLED_CORE
    #define DEBUG_MODE_FULL
#endif
    #ifdef DEBUG_SETTINGS_OTHER
    #define DEBUG_SETTINGS DEBUG_SETTINGS_CORE
#endif

#include "\z\ctab\addons\main\script_macros.hpp"
