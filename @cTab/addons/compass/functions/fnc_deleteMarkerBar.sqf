#include "script_component.hpp"

params ['_display', '_idc'];

ctrlDelete (_display displayCtrl _idc);
ctrlDelete (_display displayCtrl (_idc+1));
