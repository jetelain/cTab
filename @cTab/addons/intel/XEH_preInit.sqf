#include "script_component.hpp"
ADDON = false;
#include "XEH_PREP.hpp"
ADDON = true;

GVAR(feed) = [];
GVAR(texturePrefix) = format ["CTABINTEL_%1_", round (random 1000000)];