#include "script_component.hpp"
if ( !isNil "ace_markers_fnc_initInsertMarker" ) then {
	_this call ace_markers_fnc_initInsertMarker;
} else {
	["onLoad",_this,"RscDisplayInsertMarker",'GUI'] call (uinamespace getvariable 'BIS_fnc_initDisplay');
};

[{
    disableSerialization;
    params ["_display"];

	private _bg = _display displayCtrl 1001; 
	private _pos = ctrlPosition _bg;
	_pos params ["_x", "_y", "_w", "_h"];

	// _w should be 10 * (((safezoneW / safezoneH) min 1.2) / 40)
	// _h should be ((((safezoneW / safezoneH) min 1.2) / 1.2) / 25) (like _fieldH)

	private _grp = _display ctrlCreate ["RscControlsGroupNoScrollbars", 9800];
	_grp ctrlSetPosition [_x + _w + 0.01, _y, (_w * 2) - 0.02, 1];
	_grp ctrlCommit 0;

	_bg = _display ctrlCreate ["RscBackgroundGUITop", -1, _grp];
	_bg ctrlSetPosition [0, 0, (_w * 2) - 0.02, _h];
	_bg ctrlSetText "APP-6D Icon Options";
	_bg ctrlCommit 0;
	
	_bg = _display ctrlCreate ["RscBackgroundGUI", -1, _grp];
	_bg ctrlSetPosition [0, _h + 0.01, (_w * 2) - 0.02, 1 - _h - 0.01];
	_bg ctrlCommit 0;

	GVAR(editingOptions) = GVAR(makers) getOrDefault [ace_markers_editingMarker, []];
	GVAR(editingOptions) params [["_commonIdentifier", ""], ["_higherFormation", ""], ["_uniqueDesignation", ""], ["_reinforcedReduced", 0], ["_dateTimeGroup", ""]];
	GVAR(previewOptions) = +GVAR(editingOptions);
	
	_y = _h + 0.02;

	private _fieldH = ((((safezoneW / safezoneH) min 1.2) / 1.2) / 25);
	private _fieldW = (_w) - 0.02;
	private _labelW = (_w) - 0.02;

	// Standard identity
	private _idW = ((_w * 2) - 0.02) / 7;
	{
		private _ctrl = _display ctrlCreate ["RscPicture", -1, _grp];
		_ctrl ctrlSetPosition [(_x*_idW)-(_idW*0.25), _y-(_idW*0.25), _idW*1.5, _idW*1.5];
		_ctrl ctrlSetText (format [QPATHTOF(data\%1\10\xxxx000000xxxx_ca.paa),_x]);
		_ctrl ctrlCommit 0;
		_ctrl = _display ctrlCreate [QGVAR(valueButton), 9700 + _x, _grp];
		_ctrl ctrlSetPosition [(_x*_idW), _y, _idW, _idW];
		_ctrl ctrlAddEventHandler ["ButtonClick", 
		{
			params ["_ctrl"];
			[format ["%1",ctrlIDC _ctrl - 9700], 3] call FUNC(updateEditorSidc);
		}];
		_ctrl ctrlCommit 0;
	} forEach [0,1,2,3,4,5,6];
	_y = _y + _idW + 0.01;

	// Fields
	private _createDropdownField = {
		params ["_label", "_value", "_idc", "_handler", "_options", ["_optionsData", []]];
		private _ctrl = _display ctrlCreate ["RscText", -1, _grp];
		_ctrl ctrlSetPosition [0, _y, _labelW, _fieldH];
		_ctrl ctrlSetText _label;
		_ctrl ctrlCommit 0;
		_ctrl = _display ctrlCreate ["RscCombo", _idc, _grp];
		{ _ctrl lbAdd _x } forEach _options;
		{ _ctrl lbSetData [_forEachIndex, _x] } forEach _optionsData;
		_ctrl lbSetCurSel _value;
		_ctrl ctrlAddEventHandler ["LBSelChanged", _handler];
		_ctrl ctrlSetPosition [_labelW, _y, _fieldW, _fieldH];
		_ctrl ctrlCommit 0;
		_y = _y + _fieldH + 0.01;
	};

	private _createTextField = {
		params ["_label", "_value", "_idc", "_handler"];
		private _ctrl = _display ctrlCreate ["RscText", -1, _grp];
		_ctrl ctrlSetPosition [0, _y, _labelW, _fieldH];
		_ctrl ctrlSetText _label;
		_ctrl ctrlCommit 0;
		_ctrl = _display ctrlCreate ["RscEdit", _idc, _grp];
		_ctrl ctrlSetText _value;
		_ctrl ctrlAddEventHandler ["KeyUp", _handler];
		_ctrl ctrlSetPosition [_labelW, _y, _fieldW, _fieldH];
		_ctrl ctrlCommit 0;
		_y = _y + _fieldH + 0.01;
	};

	["Status", 0, 9910, {
		params ["_ctrl","_index"];
		[format ["%1",_index], 6] call FUNC(updateEditorSidc);
	}, ['Present','Planned/Anticiped/Supect','Present/Fully capable','Present/Damaged','Present/Destroyed','Present/Full to capacity']] call _createDropdownField;

	["HQ/TF/Dummy", 0, 9911, {
		params ["_ctrl","_index"];
		[format ["%1",_index], 7] call FUNC(updateEditorSidc);
	}, ['n/a','Feint/Dummy','Headquarters','Feint/Dummy Headquarters','Task Force','Feint/Dummy Task Force','Task Force Headquarters','Feint/Dummy Task Force Headquarters']] call _createDropdownField;

	["Common Identifier", _commonIdentifier, 9900, FUNC(updatePreviewOptions)] call _createTextField;
	["Higher Formation", _higherFormation, 9901, FUNC(updatePreviewOptions)] call _createTextField;
	["Unique Designation", _uniqueDesignation, 9902, FUNC(updatePreviewOptions)] call _createTextField;
	["Reinforced/Reduced", _reinforcedReduced, 9903, FUNC(updatePreviewOptions), ["n/a","(+)","(-)","(±)"]] call _createDropdownField;
	["Date Time Group", _dateTimeGroup, 9904, FUNC(updatePreviewOptions)] call _createTextField;


	(_display displayCtrl 1220 /*IDC_ACE_INSERT_MARKER_SHAPE*/) ctrlAddEventHandler ["LBSelChanged", {
		params ["_ctrl","_index"];
		private _data = _ctrl lbValue _index;
		private _config = (configFile >> "CfgMarkers") select _data;
		GVAR(editorActive) = (configName _config == "ctab_app6d_generic");
    	((findDisplay 54) displayCtrl 9800) ctrlShow GVAR(editorActive);
	}];

	GVAR(editorActive) = (ace_markers_currentMarkerConfigName == "ctab_app6d_generic");
	_grp ctrlShow GVAR(editorActive);

	// Setup preview on map
	GVAR(mapEH) = ((displayParent _display) displayCtrl 51 /*IDC_MAP*/)  ctrlAddEventHandler ["Draw", {
		params ["_mapCtrl"];
		if (!GVAR(editorActive)) exitWith {};
		private _idc = ctrlText ((findDisplay 54) displayCtrl 101);
		if ( count _idc == 0 ) then { _sidc = DEFAULT_SIDC; };
		[_mapCtrl, _idc, ace_markers_currentMarkerPosition, GVAR(previewOptions), BASELINE_ICON_SIZE * ace_markers_currentMarkerScale] call FUNC(drawMilsymbol);
	}]; 

}, _this] call CBA_fnc_execNextFrame;