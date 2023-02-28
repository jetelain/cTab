#include "script_component.hpp"

if ( !GVAR(isActive) ) exitWith { };

private _now = diag_tickTime;

// Remove outdated detected shots
private _count = count GVAR(detectedShots);
GVAR(detectedShots) = GVAR(detectedShots) select {  _now - (_x select 0) < GVAR(shotsTrackTimeLimit) };
private _changed = _count != count GVAR(detectedShots);

// Process any pending shots
if ( count GVAR(shotsToProcess) > 0 ) then {

	private _shots = GVAR(shotsToProcess);
	GVAR(shotsToProcess) = [];

	private _position = getPosASL vehicle player;
	private _soundSpeed = 340; // meter per seconds

	if (!isNil "ace_weather_currentTemperature") then {
		// ACE if present, can give temperature
		// temperature have an effect on sound speed
		// Approximated to  331.5 + (0.607 * tempInCelcius)
		// better approximation to consider : 20.05 * sqrt(tempInCelcius+273.15) 
		_soundSpeed = 331.5 + (0.607 * (ace_weather_currentTemperature - 0.0065 * (_position select 2)));
	};

	{
		_x params ['_time', '_source', '_caliber'];
		private _distance = _position vectorDistance _source;
		private _limit = GVAR(distanceLimitPerCaliber) select _caliber;
		if ( (_now - _time) > (_distance / _soundSpeed) ) then {
			// sound had time to travel to our position

			if ( _distance < _limit ) then {

				// gunshot is at a detectable distance, let's detect him
				private _detectedDir = (_position getDir _source) + (random [-2, 0, +2]); // +/- 2°
				private _detectedDistance = _distance * (random [0.9, 1, 1.1]); // +/- 10%
				private _point = _position getPos [_detectedDistance, _detectedDir];
				private _pointA = _position getPos [_detectedDistance + (_distance * 0.1), _detectedDir - 2];
				private _pointB = _position getPos [_detectedDistance - (_distance * 0.1), _detectedDir - 2];
				private _pointC = _position getPos [_detectedDistance - (_distance * 0.1), _detectedDir + 2];
				private _pointD = _position getPos [_detectedDistance + (_distance * 0.1), _detectedDir + 2];
				private _radius = _point vectorDistance _pointA;
				private _shotId = GVAR(nextShotId);
				private _data = [ _now, _shotId, _point, _radius, [_pointA, _pointB, _pointC, _pointD], _caliber];
				GVAR(detectedShots) pushBack _data;
				TRACE_1("DetectedShot", _data);
				_changed = true;
				GVAR(nextShotId) = GVAR(nextShotId) + 1;

#ifdef DEBUG_MODE_FULL
				private _marker = createMarker [ format ['_USER_DEFINED #0/shot%1A/0', _shotId], _pointA];
				_marker setMarkerShape 'polyline';
				_marker setMarkerPolyline ((_pointA select [0,2]) + (_pointB select [0,2]) + (_pointC select [0,2]) +(_pointD select [0,2]) +(_pointA select [0,2]));
				_marker setMarkerColor "ColorRed"; 

				_marker = createMarker [format ['_USER_DEFINED #0/shot%1C/0', _shotId], _point];
				_marker setMarkerShape "ELLIPSE";
				_marker setMarkerColor "ColorRed"; 
				_marker setMarkerSize [_radius, _radius];

				_marker = createMarker [format ['_USER_DEFINED #0/shot%1R/0', _shotId], _source];
				_marker setMarkerType "mil_dot";
				_marker setMarkerColor "ColorBlack"; 
#endif
			};

		} else {
			// sound did not have time to get to our position
			if ( _distance < _limit * 1.2 ) then {
				// A ground vehicle speed will always be slower than sound
				// Lets consider vehicle max speed 216km/h => 60m/s (really fast for a ground vehicle)
				// Slowest sound condition, will be be 300m/s (extremely cold weather => -50°C)
				// Even if we drive in the direction of the gunshot, we will never detect him
				// if distance is larger than 20% the kimit (=60/300)
				GVAR(shotsToProcess) pushBack _x;
			};
		};
	} forEach _shots;


};

if ( _changed ) then {
	TRACE_1("Update", GVAR(detectedShots));
	[QGVAR(update)] call CBA_fnc_localEvent;
};
