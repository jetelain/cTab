#include "script_component.hpp"
params ["_text"];
if (_text == "") exitWith {
	[]
};
if ( _text regexMatch "^[ ]*[0-9]{5}[ ]*-[ ]*[0-9]{5}[ ]*$" ) exitWith {
	private _tokens = _text splitString "-";
	[[parseNumber (trim (_tokens select 0)), parseNumber (trim (_tokens select 1))],[1,1]]
};
if ( _text regexMatch "^[ ]*[0-9]{4}[ ]*-[ ]*[0-9]{4}[ ]*$" ) exitWith {
	private _tokens = _text splitString "-";
	[[parseNumber (trim (_tokens select 0)) * 10, parseNumber (trim (_tokens select 1)) * 10],[10,10]]
};
if ( _text regexMatch "^[ ]*[0-9]{3}[ ]*-[ ]*[0-9]{3}[ ]*$" ) exitWith {
	private _tokens = _text splitString "-";
	[[parseNumber (trim (_tokens select 0)) * 100, parseNumber (trim (_tokens select 1)) * 100],[100,100]]
};
(_text call BIS_fnc_gridToPos)