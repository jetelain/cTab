#include "script_component.hpp"
ADDON = false;
#include "XEH_PREP.hpp"
ADDON = true;
GVAR(cache) = createHashMap;
GVAR(reinforcedReducedIcons) = ["", QPATHTOF(data\o\plus_ca.paa), QPATHTOF(data\o\minus_ca.paa), QPATHTOF(data\o\plusminus_ca.paa)];