#include "script_component.hpp"
params ['_ammo'];
private _cfgAmmo = configFile >> "CfgAmmo";
if (_ammo isKindOf ["BulletBase", _cfgAmmo]) exitWith {
	private _ace_caliber = getNumber (_cfgAmmo >> _ammo >> "ace_caliber");
	if (_ace_caliber == 0) then {
		_ace_caliber = getNumber (_cfgAmmo >> _ammo >> "ace_rearm_caliber");
	};
	if (_ace_caliber == 0) exitWith {
		private _caliber = getNumber (_cfgAmmo >> _ammo >> "caliber");
		if ( _caliber <= 1 ) exitWith {
			CALIBER_556
		};
		if ( _caliber <= 2 ) exitWith {
			CALIBER_762
		};
		if ( _caliber <= 3 ) exitWith {
			CALIBER_1270
		};
		private _hit = getNumber (_cfgAmmo >> _ammo >> "hit");
		if ( _hit > 20 ) exitWith {
			CALIBER_2000
		};
		CALIBER_1450
	};
	if ( _ace_caliber < 6 ) exitWith {
		CALIBER_556
	};
	if ( _ace_caliber < 8 ) exitWith {
		CALIBER_762
	};
	if ( _ace_caliber < 13 ) exitWith {
		CALIBER_1270
	};
	if ( _ace_caliber >= 20 ) exitWith {
		CALIBER_2000
	};
	 CALIBER_1450
}; 
if (_ammo isKindOf ["ShellBase", _cfgAmmo]) exitWith { 
	if (_ammo isKindOf ["ace_explosion_reflection_base", _cfgAmmo]) exitWith { 
		INFO_1("Ammo %1 is IGNORED", _ammo);
		CALIBER_UNSUPPORTED
	};
	private _ace_caliber = getNumber (_cfgAmmo >> _ammo >> "ace_rearm_caliber");
	if (_ace_caliber == 0) exitWith {
		private _caliber = getNumber (_cfgAmmo >> _ammo >> "caliber");
		if ( _caliber < 10 ) exitWith {
			CALIBER_2000
		};
		CALIBER_9000
	};
	if (_ace_caliber < 90) exitWith {
		CALIBER_2000
	};
	CALIBER_9000
};
if (_ammo isKindOf ["RocketBase", _cfgAmmo]) exitWith { 
	CALIBER_ROCKET
};
if (_ammo isKindOf ["MissileBase", _cfgAmmo]) exitWith { 
	CALIBER_MISSILE
};
INFO_1("Ammo %1 is IGNORED", _ammo);
CALIBER_UNSUPPORTED