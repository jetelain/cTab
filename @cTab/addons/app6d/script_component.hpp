#define COMPONENT app6d
#include "\z\ctab\addons\main\script_mod.hpp"

#define BASELINE_ICON_SIZE 96
#define DEFAULT_SIDC "10001000000000000000"

 #define DEBUG_MODE_FULL
 #define DISABLE_COMPILE_CACHE

#ifdef DEBUG_ENABLED_CORE
    #define DEBUG_MODE_FULL
#endif
    #ifdef DEBUG_SETTINGS_OTHER
    #define DEBUG_SETTINGS DEBUG_SETTINGS_CORE
#endif

#include "\z\ctab\addons\main\script_macros.hpp"
