#include "script_component.hpp"
params ["_id", ["_resolution", 1024]];

format ['#(rgb,%1,%1,1)ui("%2","%3%4")', _resolution, QGVAR(intelTextureDisplay),GVAR(texturePrefix), _id]