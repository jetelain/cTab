#include "script_component.hpp"

params ["_sidc"];

if ( count _sidc < 20 ) exitWith { [] };

// For SIDC "xxxABBCDDDEEEEEEFFGG" (3 first chars are ignored)
// 
// Render order (1 to 5 images required):
// - A/BB/xDDDxxxxxxxxxx_ca.paa if DDD != "000" && file exists
// - A/BB/xxxxEEEEEExxxx_ca.paa if file exists
// - A/BB/Cxxxxxxxxxxxxx_ca.paa if C   != "0"   && file exists
// - A/BB/xxxxxxxxxxFFxx_ca.paa if FF  != "00"  && file exists
// - A/BB/xxxxxxxxxxxxGG_ca.paa if GG  != "00"  && file exists
// Fail over to "s" if file does not exist (s for shared)

private _result = [];

private _a = _sidc select [3,1];
private _b = _sidc select [4,2];
private _c = _sidc select [6,1];
private _d = _sidc select [7,3];
private _e = _sidc select [10,6];
private _f = _sidc select [16,2];
private _g = _sidc select [18,2];

private _img;

if ( _d != "000") then {
	_img = format [QPATHTOF(data\%1\%2\x%3xxxxxxxxxx_ca.paa),_a,_b,_d];
	if ( fileExists _img ) then {
	  _result pushBack _img;
	} else {
		_img = format [QPATHTOF(data\%1\%2\x%3xxxxxxxxxx_ca.paa),'s',_b,_d];
		if ( fileExists _img ) then {
		  _result pushBack _img;
		};
	};
};

_img = format [QPATHTOF(data\%1\%2\xxxx%3xxxx_ca.paa),_a,_b,_e];
if ( fileExists _img ) then {
  _result pushBack _img;
} else {
	_img = format [QPATHTOF(data\%1\%2\xxxx%3xxxx_ca.paa),'s',_b,_e];
	if ( fileExists _img ) then {
	  _result pushBack _img;
	};
};

if ( _c != "0") then {
	_img = format [QPATHTOF(data\%1\%2\%3xxxxxxxxxxxxx_ca.paa),_a,_b,_c];
	if ( fileExists _img ) then {
	  _result pushBack _img;
	} else {
		_img = format [QPATHTOF(data\%1\%2\%3xxxxxxxxxxxxx_ca.paa),'s',_b,_c];
		if ( fileExists _img ) then {
		  _result pushBack _img;
		};
	};
};
if ( _f != "00") then {
	_img = format [QPATHTOF(data\%1\%2\xxxxxxxxxx%3xx_ca.paa),_a,_b,_f];
	if ( fileExists _img ) then {
	  _result pushBack _img;
	} else {
		_img = format [QPATHTOF(data\%1\%2\xxxxxxxxxx%3xx_ca.paa),'s',_b,_f];
		if ( fileExists _img ) then {
		  _result pushBack _img;
		};
	};
};
if ( _g != "00") then {
	_img = format [QPATHTOF(data\%1\%2\xxxxxxxxxxxx%3_ca.paa),_a,_b,_g];
	if ( fileExists _img ) then {
	  _result pushBack _img;
	} else {
		_img = format [QPATHTOF(data\%1\%2\xxxxxxxxxxxx%3_ca.paa),'s',_b,_g];
		if ( fileExists _img ) then {
		  _result pushBack _img;
		};
	};
};

_result