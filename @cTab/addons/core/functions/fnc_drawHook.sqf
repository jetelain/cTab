#include "script_component.hpp"
/*
 	Name: cTab_fnc_drawHook

 	Author(s):
		Gundy, modified by Bamse

 	Description:
		Calculate and draw hook distance, direction, grid, elevation and arrow

	Parameters:
		0: OBJECT  - Display used to write hook direction, distance and grid to
		0: OBJECT  - Map control to draw arrow on
		2: ARRAY   - Position A, format PositionASL
		3: ARRAY   - Position B, format Position2D
		4: INTEGER - Mode, 0 = Reference is A, 1 = Reference is B
		5: BOOLEAN - TAD, TRUE = TAD

 	Returns:
		BOOLEAN - Always TRUE

 	Example:
		[_display,_ctrlScreen,_playerPos,cTabMapCursorPos,0,false] call cTab_fnc_drawHook;
*/

#include "\cTab\shared\cTab_gui_macros.hpp"

params ["_display","_ctrlScreen","_pos","_secondPos"];

// draw arrow from current position to map centre
private _dirToSecondPos =
	if (_this select 4 == 0) then {
		_ctrlScreen drawArrow [_pos,_secondPos,cTabMicroDAGRhighlightColour];
		_pos getDir _secondPos
	} else {
		_ctrlScreen drawArrow [_secondPos,_pos,cTabMicroDAGRhighlightColour];
		_secondPos getDir _pos
	};

// Distance have been historically computed at terrain level
// Terrain had a little impact on computed distance, due to potential elevation difference
// We may introduce an option to have elevation used
private _dstToSecondPos = _pos distance2D _secondPos;

call {
	// Call this if we are drawing for a TAD
	if (_this select 5) exitWith {
		(_display displayCtrl IDC_CTAB_OSD_HOOK_GRID) ctrlSetText format ["%1",mapGridPosition _secondPos];
		(_display displayCtrl IDC_CTAB_OSD_HOOK_ELEVATION) ctrlSetText format ["%1m",[round getTerrainHeightASL _secondPos,3] call CBA_fnc_formatNumber];
		(_display displayCtrl IDC_CTAB_OSD_HOOK_DIR) ctrlSetText format ["%1/%2m",[_dirToSecondPos] call FUNC(formatHeading), [_dstToSecondPos,1] call CBA_fnc_formatNumber];
	};
	(_display displayCtrl IDC_CTAB_OSD_HOOK_GRID) ctrlSetText ([_secondPos] call FUNC(gridPosition));
	(_display displayCtrl IDC_CTAB_OSD_HOOK_ELEVATION) ctrlSetText format ["%1m",round getTerrainHeightASL _secondPos];
	(_display displayCtrl IDC_CTAB_OSD_HOOK_DIR) ctrlSetText format ["%1 %2",[_dirToSecondPos] call FUNC(formatHeading), [_dirToSecondPos] call cTab_fnc_degreeToOctant];
	(_display displayCtrl IDC_CTAB_OSD_HOOK_DST) ctrlSetText format ["%1m",[_dstToSecondPos,1] call CBA_fnc_formatNumber];
};

true