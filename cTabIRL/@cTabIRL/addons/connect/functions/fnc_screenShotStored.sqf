#include "script_component.hpp"

params ['_uri','_data'];

playSound ["cTab_cameraShutter", 2];

_this call ctab_intel_fnc_addPhoto;
