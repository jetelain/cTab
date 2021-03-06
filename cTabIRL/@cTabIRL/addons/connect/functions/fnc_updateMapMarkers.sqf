#include "script_component.hpp"

private _simple = [];
private _poly = [];

{
	private _markerName = _x;
	private _markerAlpha = markerAlpha _markerName;
	if ( _markerAlpha >  0 ) then {
		private _markerShape = markerShape _markerName;
		if (_markerShape isEqualTo "POLYLINE") then {
			_poly pushBack [
				_markerName,
				markerPolyline _markerName,
				markerBrush _markerName,
				markerColor _markerName,
				_markerAlpha
			];
		}else {
			_simple pushBack [
				_markerName,
				markerPos [_markerName, false],
				markerType _markerName,
				_markerShape,
				markerSize _markerName,
				markerDir _markerName,
				markerBrush _markerName,
				markerColor _markerName,
				markerText _markerName,
				_markerAlpha
			];
		};
	};
} forEach allMapMarkers;

"cTabExtension" callExtension ["UpdateMapMarkers", [_simple, _poly]];
