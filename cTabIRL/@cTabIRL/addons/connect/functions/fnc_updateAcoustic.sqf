#include "script_component.hpp"

GVAR(acousticNeedsUpdate) = false;
toFixed 1; // 0.1 meters, 0.1 sec is far enough
"cTabExtension" callExtension ["UpdateAcoustic", [diag_tickTime, ctab_acousticdetector_detectedShots apply { _x select [0,5] }]];
toFixed -1;
