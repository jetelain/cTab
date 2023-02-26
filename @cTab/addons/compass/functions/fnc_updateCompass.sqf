#include "script_component.hpp"

params ["_control"];

// INFO('DRAW');
private _ctrlBloc = _control controlsgroupctrl 9000;
private _shouldDisplay = (cameraView in ["GUNNER", "GROUP"]) && GVAR(enable);
if ( ctrlShown _ctrlBloc != _shouldDisplay ) then {
	_ctrlBloc ctrlShow _shouldDisplay;
};
if ( !_shouldDisplay ) exitWith { };

private _ctrlBar1 = _control controlsgroupctrl 9001;
private _ctrlBar2 = _control controlsgroupctrl 9002;
private _dir = positionCameraToWorld [0,0,0] getdir positionCameraToWorld [0,0,10]; // [0;360]

(ctrlposition _ctrlBar1) params ["", "", "_dwidth", "_height"];

// 0   => _bar1X=_dwidth * +1 / 4  0   => _bar2X=_dwidth * -3 / 4 
// 45  => _bar1X=_dwidth * +0 / 4  45  => _bar2X=_dwidth * -4 / 4 
// 90  => _bar1X=_dwidth * -1 / 4  90  => _bar2X=_dwidth * -5 / 4 
// 135 => _bar1X=_dwidth * -2 / 4  135 => _bar2X=_dwidth * +2 / 4  
// 180 => _bar1X=_dwidth * -3 / 4  180 => _bar2X=_dwidth * +1 / 4   
// 225 => _bar1X=_dwidth * -4 / 4  225 => _bar2X=_dwidth * +0 / 4  
// 270 => _bar2X=_dwidth * -5 / 4  270 => _bar2X=_dwidth * -1 / 4  
// 315 => _bar1X=_dwidth * +2 / 4  315 => _bar2X=_dwidth * -2 / 4  
// 360 => _bar1X=_dwidth * +1 / 4  360 => _bar2X=_dwidth * -3 / 4  

// Needs angle expressed between [-45;315[
// -45 => _bar1X=_dwidth * +2 / 4
// 0   => _bar1X=_dwidth * +1 / 4
// 45  => _bar1X=_dwidth * +0 / 4
// 90  => _bar1X=_dwidth * -1 / 4
// 135 => _bar1X=_dwidth * -2 / 4
// 180 => _bar1X=_dwidth * -3 / 4
// 225 => _bar1X=_dwidth * -4 / 4
// 270 => _bar2X=_dwidth * -5 / 4

// Needs angle expressed between [135;495[
// 135 => _bar2X=_dwidth * +2 / 4
// 180 => _bar2X=_dwidth * +1 / 4
// 225 => _bar2X=_dwidth * +0 / 4
// 270 => _bar2X=_dwidth * -1 / 4
// 315 => _bar2X=_dwidth * -2 / 4
// 360 => _bar2X=_dwidth * -3 / 4
// 405 => _bar2X=_dwidth * -4 / 4
// 450 => _bar2X=_dwidth * -5 / 4

private _newBar1X = linearconversion [-45, 270, if ( _dir >= 315 ) then {_dir-360} else {_dir}, _dwidth * 2 / 4, _dwidth * (-5) / 4];
private _newBar2X = linearconversion [135, 450, if ( _dir < 135) then {_dir+360} else {_dir}, _dwidth * 2 / 4, _dwidth * (-5) / 4];

_ctrlBar1 ctrlsetpositionX _newBar1X;
_ctrlBar2 ctrlsetpositionX _newBar2X;

_ctrlBar1 ctrlcommit 0;
_ctrlBar2 ctrlcommit 0;

private _playerPosition = getPosASL ctab_player;

if ( GVAR(markersHaveChanged) || { (GVAR(previousPosition) vectorDistanceSqr _playerPosition) > 0.04 } ) then { 
	// updated every >20cm position change, or when markers have changed

	GVAR(previousPosition) = _playerPosition;
	GVAR(markersHaveChanged) = false;
	
	//private _start = diag_tickTime;
	private _display = ctrlParent _control;
	private _toremove = keys GVAR(markerToIdc);

	{
		_x params ["_mid","_markerData","_markerRawData"];
		_markerRawData params ['_markerPosition', '_markerIcon'];

		if ( _markerIcon < 20 ) then {
			private _distance = _playerPosition vectorDistance _markerPosition;
			private _direction = _playerPosition getDir _markerPosition;
			private _idc = GVAR(markerToIdc) getOrDefault [_mid, -1];
			private _posX1 = (if (_direction >= 315) then {_direction-360} else {_direction}) * _dwidth / 180;
			private _posX2 = (if (_direction < 135) then {_direction+180} else {_direction-180}) * _dwidth / 180;
			if ( _idc == -1 ) then {
				_idc = GVAR(nextIdc);
				GVAR(nextIdc) = _idc + 4;
				[_display, _ctrlBar1, _posX1, _height, _idc, _markerData, _distance] call FUNC(addMarkerBar);
				[_display, _ctrlBar2, _posX2, _height, _idc+2, _markerData, _distance] call FUNC(addMarkerBar);
				GVAR(markerToIdc) set [_mid, _idc];
			} else {
				[_display, _posX1, _height, _idc, _distance] call FUNC(updateMarkerBar);
				[_display, _posX2, _height, _idc+2, _distance] call FUNC(updateMarkerBar);
				_toremove deleteAt (_toremove find _mid);
			};
		};
	} forEach cTabUserMarkerList;

	{
		private _idc = GVAR(markerToIdc) getOrDefault [_x, -1];
		[_display, _idc] call FUNC(deleteMarkerBar);
		[_display, _idc+2] call FUNC(deleteMarkerBar);
		GVAR(markerToIdc) deleteAt _x;
	} forEach _toremove;

	//private _elapsed = diag_tickTime-_start;
	//INFO_1('Markers updated in %1 msec', _elapsed);
};