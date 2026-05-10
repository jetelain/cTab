#define COMPONENT acousticdetector
#include "\z\ctab\addons\main\script_mod.hpp"

// #define DEBUG_MODE_FULL
// #define DISABLE_COMPILE_CACHE

#ifdef DEBUG_ENABLED_CORE
    #define DEBUG_MODE_FULL
#endif
    #ifdef DEBUG_SETTINGS_OTHER
    #define DEBUG_SETTINGS DEBUG_SETTINGS_CORE
#endif

#include "\z\ctab\addons\main\script_macros.hpp"

#define CALIBER_UNSUPPORTED -1
#define CALIBER_ROCKET       0
#define CALIBER_MISSILE      1
#define CALIBER_556          2
#define CALIBER_762          3
#define CALIBER_1270         4
#define CALIBER_1450         5
#define CALIBER_2000         6
#define CALIBER_9000         7
