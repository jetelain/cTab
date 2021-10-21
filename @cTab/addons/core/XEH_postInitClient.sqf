#include "script_component.hpp"
// cTab - Commander's Tablet with FBCB2 Blue Force Tracking
// Battlefield tablet to access real time intel and blue force tracker.
// By - Riouken
// http://forums.bistudio.com/member.php?64032-Riouken
// You may re-use any of this work as long as you provide credit back to me.

#include "\a3\editor_f\Data\Scripts\dikCodes.h"
#include "\cTab\shared\cTab_gui_macros.hpp"

// Exit if this is machine has no interface, i.e. is a headless client (HC)
if (!hasInterface) exitWith {};

// Get a rsc layer for for our displays
cTabRscLayer = ["cTab"] call BIS_fnc_rscLayer;
cTabRscLayerMailNotification = ["cTab_mailNotification"] call BIS_fnc_rscLayer;

// Set up user markers
cTab_userMarkerTransactionId = -1;
cTab_userMarkerLists = [];
[] call cTab_fnc_getUserMarkerList;

// Set up side specific encryption keys
if (isNil "cTab_encryptionKey_west") then {
    cTab_encryptionKey_west = "b";
};
if (isNil "cTab_encryptionKey_east") then {
    cTab_encryptionKey_east = "o";
};
if (isNil "cTab_encryptionKey_guer") then {
    cTab_encryptionKey_guer = call {
        if (([west,resistance] call BIS_fnc_areFriendly) and {!([east,resistance] call BIS_fnc_areFriendly)}) exitWith {
            "b"
        };
        if (([east,resistance] call BIS_fnc_areFriendly) and {!([west,resistance] call BIS_fnc_areFriendly)}) exitWith {
            "o"
        };
        "i"
    };
};
if (isNil "cTab_encryptionKey_civ") then {
    cTab_encryptionKey_civ = "c";
};

// Set up empty lists
cTabBFTmembers = [];
cTabBFTgroups = [];
cTabBFTvehicles = [];
cTabUAVlist = [];
cTabHcamlist = [];
cTabNotificationCache = [];
cTab_player = objNull;

GVAR(personnelDevices) = ["ItemcTab", "ItemAndroid", "ItemMicroDAGR"];

/*
Figure out the scaling factor based on the current map (island) being played
Requires the scale of the map control to be at 0.001
*/
call {
    private ["_displayName","_display","_mapCtrl","_mapScreenPos","_mapScreenX_left","_mapScreenH","_mapScreenY_top","_mapScreenY_middle","_mapWorldY_top","_mapWorldY_middle"];
    
    _displayName = "cTab_mapSize_dsp";
    cTabRscLayer cutRsc [_displayName,"PLAIN",0, false];
    while {isNull (uiNamespace getVariable _displayName)} do {};
    _display = uiNamespace getVariable _displayName;
    _mapCtrl = _display displayCtrl 1110;

    // get the screen postition of _mapCtrl as [x, y, w, h]
    _mapScreenPos = ctrlPosition _mapCtrl;

    // Find the screen coordinate for the left side
    _mapScreenX_left = _mapScreenPos select 0;
    // Find the screen height
    _mapScreenH = _mapScreenPos select 3;
    // Find the screen coordinate for top Y 
    _mapScreenY_top = _mapScreenPos select 1;
    // Find the screen coordinate for middle Y 
    _mapScreenY_middle = _mapScreenY_top + _mapScreenH / 2;

    // Get world coordinate for top Y in meters
    _mapWorldY_top = (_mapCtrl ctrlMapScreenToWorld [_mapScreenX_left,_mapScreenY_top]) select 1;
    // Get world coordinate for middle Y in meters
    _mapWorldY_middle = (_mapCtrl ctrlMapScreenToWorld [_mapScreenX_left,_mapScreenY_middle]) select 1;

    // calculate the difference between top and middle world coordinates, devide by the screen height and return
    cTabMapScaleFactor = (abs(_mapWorldY_middle - _mapWorldY_top)) / _mapScreenH;

    _display closeDisplay 0;
    uiNamespace setVariable [_displayName, displayNull];
};

cTabMapScaleUAV = 0.8 / cTabMapScaleFactor;
cTabMapScaleHCam = 0.3 / cTabMapScaleFactor;

cTabDisplayPropertyGroups = [
    ["cTab_Tablet_dlg","Tablet"],
    ["cTab_Android_dlg","Android"],
    ["cTab_Android_dsp","Android"],
    ["cTab_FBCB2_dlg","FBCB2"],
    ["cTab_TAD_dsp","TAD"],
    ["cTab_TAD_dlg","TAD"],
    ["cTab_microDAGR_dsp","MicroDAGR"],
    ["cTab_microDAGR_dlg","MicroDAGR"]
];

cTabSettings = [];

[cTabSettings,"COMMON",[
    ["mode","BFT"],
    ["mapScaleMin",0.1],
    ["mapScaleMax",2 ^ round(sqrt(2666 * cTabMapScaleFactor / 1024))]
]] call BIS_fnc_setToPairs;

[cTabSettings,"Main",[
]] call BIS_fnc_setToPairs;

[cTabSettings,"Tablet",[
    ["dlgIfPosition",[]],
    ["mode","DESKTOP"],
    ["showIconText",true],
    ["mapWorldPos",[]],
    ["mapScaleDsp",2],
    ["mapScaleDlg",2],
    ["mapTypes",[["SAT",IDC_CTAB_SCREEN],["TOPO",IDC_CTAB_SCREEN_TOPO]]],
    ["mapType","SAT"],
    ["uavCam",""],
    ["hCam",""],
    ["mapTools",true],
    ["nightMode",2],
    ["brightness",0.9]
]] call BIS_fnc_setToPairs;

[cTabSettings,"Android",[
    ["dlgIfPosition",[]],
    ["dspIfPosition",false],
    ["mode","BFT"],
    ["showIconText",true],
    ["mapWorldPos",[]],
    ["mapScaleDsp",0.4],
    ["mapScaleDlg",0.4],
    ["mapTypes",[["SAT",IDC_CTAB_SCREEN],["TOPO",IDC_CTAB_SCREEN_TOPO]]],
    ["mapType","SAT"],
    ["showMenu",false],
    ["mapTools",true],
    ["nightMode",2],
    ["brightness",0.9]
]] call BIS_fnc_setToPairs;

[cTabSettings,"FBCB2",[
    ["dlgIfPosition",[]],
    ["mapWorldPos",[]],
    ["showIconText",true],
    ["mapScaleDsp",2],
    ["mapScaleDlg",2],
    ["mapTypes",[["SAT",IDC_CTAB_SCREEN],["TOPO",IDC_CTAB_SCREEN_TOPO]]],
    ["mapType","SAT"],
    ["mapTools",true]
]] call BIS_fnc_setToPairs;

/*
TAD setup
*/
// set icon size of own vehicle on TAD
cTabTADownIconBaseSize = 18;
cTabTADownIconScaledSize = cTabTADownIconBaseSize / (0.86 / (safezoneH * 0.8));
// set TAD font colour to neon green
cTabTADfontColour = [57/255, 255/255, 20/255, 1];
// set TAD group colour to purple
cTabTADgroupColour = [255/255, 0/255, 255/255, 1];
// set TAD highlight colour to neon yellow
cTabTADhighlightColour = [243/255, 243/255, 21/255, 1];

[cTabSettings,"TAD",[
    ["dlgIfPosition",[]],
    ["dspIfPosition",false],
    ["mapWorldPos",[]],
    ["showIconText",true],
    ["mapScaleDsp",2],
    ["mapScaleDlg",2],
    ["mapScaleMin",1],
    ["mapTypes",[["SAT",IDC_CTAB_SCREEN],["TOPO",IDC_CTAB_SCREEN_TOPO],["BLK",IDC_CTAB_SCREEN_BLACK]]],
    ["mapType","SAT"],
    ["mapTools",true],
    ["nightMode",0],
    ["brightness",0.8]
]] call BIS_fnc_setToPairs;

/*
microDAGR setup
*/
// set MicroDAGR font colour to neon green
cTabMicroDAGRfontColour = [57/255, 255/255, 20/255, 1];
// set MicroDAGR group colour to purple
cTabMicroDAGRgroupColour = [25/255, 25/255, 112/255, 1];
// set MicroDAGR highlight colour to neon yellow
cTabMicroDAGRhighlightColour = [243/255, 243/255, 21/255, 1];

[cTabSettings,"MicroDAGR",[
    ["dlgIfPosition",[]],
    ["dspIfPosition",false],
    ["mapWorldPos",[]],
    ["showIconText",true],
    ["mapScaleDsp",0.4],
    ["mapScaleDlg",0.4],
    ["mapTypes",[["SAT",IDC_CTAB_SCREEN],["TOPO",IDC_CTAB_SCREEN_TOPO]]],
    ["mapType","SAT"],
    ["mapTools",true],
    ["nightMode",2],
    ["brightness",0.9]
]] call BIS_fnc_setToPairs;

// set base colors from BI -- Helps keep colors matching if user changes colors in options.
_r = profilenamespace getvariable ['Map_BLUFOR_R',0];
_g = profilenamespace getvariable ['Map_BLUFOR_G',0.8];
_b = profilenamespace getvariable ['Map_BLUFOR_B',1];
_a = profilenamespace getvariable ['Map_BLUFOR_A',0.8];
cTabColorBlue = [_r,_g,_b,_a];

_r = profilenamespace getvariable ['Map_OPFOR_R',0];
_g = profilenamespace getvariable ['Map_OPFOR_G',1];
_b = profilenamespace getvariable ['Map_OPFOR_B',1];
_a = profilenamespace getvariable ['Map_OPFOR_A',0.8];
cTabColorRed = [_r,_g,_b,_a];

_r = profilenamespace getvariable ['Map_Independent_R',0];
_g = profilenamespace getvariable ['Map_Independent_G',1];
_b = profilenamespace getvariable ['Map_Independent_B',1];
_a = profilenamespace getvariable ['Map_OPFOR_A',0.8];
cTabColorGreen = [_r,_g,_b,_a];

cTabColorBlack = [0,0,0,1];


// Define Fire-Team colors
// MAIN,RED,GREEN,BLUE,YELLOW
cTabColorTeam = [cTabColorBlue,[200/255,0,0,0.8],[0,199/255,0,0.8],[0,0,200/255,0.8],[225/255,225/255,0,0.8]];

// define vehicles that have FBCB2 monitor
if (isNil "cTab_vehicleClass_has_FBCB2") then {
    if (!isNil "cTab_vehicleClass_has_FBCB2_server") then {
        cTab_vehicleClass_has_FBCB2 = cTab_vehicleClass_has_FBCB2_server;
    } else {
        cTab_vehicleClass_has_FBCB2 = ["MRAP_01_base_F","MRAP_02_base_F","MRAP_03_base_F","Wheeled_APC_F","Tank","Truck_01_base_F","Truck_03_base_F"];
    };
};
// strip list of invalid config names and duplicates to save time checking through them later
_classNames = [];
{
    if (isClass (configfile >> "CfgVehicles" >> _x) && _classNames find _x == -1) then {
        0 = _classNames pushBack _x;
    };
} count cTab_vehicleClass_has_FBCB2;
cTab_vehicleClass_has_FBCB2 = [] + _classNames;

// define vehicles that have TAD
if (isNil "cTab_vehicleClass_has_TAD") then {
    if (!isNil "cTab_vehicleClass_has_TAD_server") then {
        cTab_vehicleClass_has_TAD = cTab_vehicleClass_has_TAD_server;
    } else {
        cTab_vehicleClass_has_TAD = ["Helicopter","Plane"];
    };
};
// strip list of invalid config names and duplicates to save time checking through them later
_classNames = [];
{
    if (isClass (configfile >> "CfgVehicles" >> _x) && _classNames find _x == -1) then {
        0 = _classNames pushBack _x;
    };
} count cTab_vehicleClass_has_TAD;
cTab_vehicleClass_has_TAD = [] + _classNames;

// define items that enable head cam
if (isNil "cTab_helmetClass_has_HCam") then {
    if (!isNil "cTab_helmetClass_has_HCam_server") then {
        cTab_helmetClass_has_HCam = cTab_helmetClass_has_HCam_server;
    } else {
        cTab_helmetClass_has_HCam = ["H_HelmetB_light","H_Helmet_Kerry","H_HelmetSpecB","H_HelmetO_ocamo","BWA3_OpsCore_Fleck_Camera","BWA3_OpsCore_Schwarz_Camera","BWA3_OpsCore_Tropen_Camera"];
    };
};
// strip list of invalid config names and duplicates to save time checking through them later
_classNames = [];
{
    if (isClass (configfile >> "CfgWeapons" >> _x) && _classNames find _x == -1) then {
        0 = _classNames pushBack _x;
    };
} count cTab_helmetClass_has_HCam;
// iterate through all class names and add child classes, so we end up with a list of helmet classes that have the defined helmet classes as parents
{
    _childClasses = "inheritsFrom _x == (configfile >> 'CfgWeapons' >> '" + _x + "')" configClasses (configfile >> "CfgWeapons");
    {
        _childClassName = configName _x;
        if (_classNames find _childClassName == -1) then {
            0 = _classNames pushBack configName _x;
        };
    } count _childClasses;
} forEach _classNames;
cTab_helmetClass_has_HCam = [] + _classNames;

// Beginning text and icon size
cTabTxtFctr = 12;
call cTab_fnc_update_txt_size;
cTabBFTtxt = true;

// Draw Map Tolls (Hook)
cTabDrawMapTools = false;

// Base defines.
cTabUavViewActive = false;
cTabUAVcams = [];
cTabCursorOnMap = false;
cTabMapCursorPos = [0,0];
cTabMapWorldPos = [];
cTabMapScale = 0.5;

// Initialize all uiNamespace variables
uiNamespace setVariable ["cTab_Tablet_dlg", displayNull];
uiNamespace setVariable ["cTab_Android_dlg", displayNull];
uiNamespace setVariable ["cTab_Android_dsp", displayNull];
uiNamespace setVariable ["cTab_FBCB2_dlg", displayNull];
uiNamespace setVariable ["cTab_TAD_dsp", displayNull];
uiNamespace setVariable ["cTab_TAD_dlg", displayNull];
uiNamespace setVariable ["cTab_microDAGR_dsp", displayNull];
uiNamespace setVariable ["cTab_microDAGR_dlg", displayNull];

// Set up the array that will hold text messages.
player setVariable ["ctab_messages",[]];

// cTabIfOpenStart will be set to true while interface is starting and prevent further open attempts
cTabIfOpenStart = false;

// Backward compatibility
cTabOnDrawbft = ctab_fnc_onDrawbft;
cTabOnDrawbftVeh = ctab_fnc_onDrawbftVeh;
cTabOnDrawbftTAD = ctab_fnc_onDrawbftTAD;
cTabOnDrawbftTADdialog = ctab_fnc_onDrawbftTADdialog;
cTabOnDrawbftAndroid = ctab_fnc_onDrawbftAndroid;
cTabOnDrawbftAndroidDsp = ctab_fnc_onDrawbftAndroidDsp;
cTabOnDrawbftmicroDAGRdsp = ctab_fnc_onDrawbftmicroDAGRdsp;
cTabOnDrawbftMicroDAGRdlg = ctab_fnc_onDrawbftMicroDAGRdlg;
cTabOnDrawUAV = ctab_fnc_onDrawUAV;
cTabOnDrawHCam = ctab_fnc_onDrawHCam;
cTab_msg_gui_load = ctab_fnc_msg_gui_load;
cTab_msg_get_mailTxt = ctab_fnc_msg_get_mailTxt;
cTab_msg_Send = ctab_fnc_msg_Send;
cTab_msg_delete_all = ctab_fnc_msg_delete_all;
cTab_Tablet_btnACT = ctab_fnc_tablet_btnACT;

// Setup keybindings
["cTab","ifMain",[LLSTRING(ifMain),LLSTRING(ifMainDetails)],{call cTab_fnc_onIfMainPressed},"",[DIK_H,[false,false,false]],false] call cba_fnc_addKeybind;
["cTab","ifSecondary",[LLSTRING(ifSecondary),LLSTRING(ifSecondaryDetails)],{call cTab_fnc_onIfSecondaryPressed},"",[DIK_H,[false,true,false]],false] call cba_fnc_addKeybind;
["cTab","ifTertiary",[LLSTRING(ifTertiary),LLSTRING(ifTertiaryDetails)],{call cTab_fnc_onIfTertiaryPressed},"",[DIK_H,[false,false,true]],false] call cba_fnc_addKeybind;
["cTab","zoomIn",[LLSTRING(zoomIn),LLSTRING(zoomInDetails)],{call cTab_fnc_onZoomInPressed},"",[DIK_PGUP,[true,true,false]],false] call cba_fnc_addKeybind;
["cTab","zoomOut",[LLSTRING(zoomOut),LLSTRING(zoomOutDetails)],{call cTab_fnc_onZoomOutPressed},"",[DIK_PGDN,[true,true,false]],false] call cba_fnc_addKeybind;
["cTab","toggleIfPosition",[LLSTRING(toggleIfPosition),LLSTRING(toggleIfPositionDetails)],{[] call cTab_fnc_toggleIfPosition},"",[DIK_HOME,[true,true,false]],false] call cba_fnc_addKeybind;

["cTab_msg_receive",
    {
        _msgRecipient = _this select 0;
        _msgTitle = _this select 1;
        _msgBody = _this select 2;
        _msgEncryptionKey = _this select 3;
        _sender = _this select 4;
        _playerEncryptionKey = call cTab_fnc_getPlayerEncryptionKey;
        _msgArray = _msgRecipient getVariable [format ["cTab_messages_%1",_msgEncryptionKey],[]];
        _msgArray pushBack [_msgTitle,_msgBody,0];
        
        _msgRecipient setVariable [format ["cTab_messages_%1",_msgEncryptionKey],_msgArray];
        
        [QGVARMAIN(messagesUpdated)] call CBA_fnc_localEvent;

        if (_msgRecipient == cTab_player && _sender != cTab_player && {_playerEncryptionKey == _msgEncryptionKey} && {[cTab_player,["ItemcTab","ItemAndroid"]] call cTab_fnc_checkGear}) then {
            playSound "cTab_phoneVibrate";
            
            if (!isNil "cTabIfOpen" && {[cTabIfOpen select 1,"mode"] call cTab_fnc_getSettings == "MESSAGE"}) then {
                _nop = [] call cTab_msg_gui_load;
                
                // add a notification
                ["MSG",format [LLSTRING(newMessage),name _sender],6] call cTab_fnc_addNotification;
            } else {
                cTabRscLayerMailNotification cutRsc ["cTab_Mail_ico_disp", "PLAIN"]; //show
            };
        };
    }
] call CBA_fnc_addLocalEventHandler;

["CBA_settingsInitialized", {

    if (GVAR(useAceMicroDagr) && {isClass (configFile >> "CfgWeapons" >> "ACE_microDAGR")}) then {
        ace_microdagr_miniMapDrawHandlers pushBack {
            params ["_ctrl"];
            [_ctrl,false] call cTab_fnc_drawUserMarkers;
            [_ctrl,2] call cTab_fnc_drawBftMarkers;
        };
        GVAR(personnelDevices) pushBack "ACE_microDAGR";
    };
    
    // set current player object in cTab_player and run a check on every frame to see if there is a change
    ["cTab_checkForPlayerChange", "onEachFrame", {
        if !(cTab_player isEqualTo (missionNamespace getVariable ["BIS_fnc_moduleRemoteControl_unit",player])) then {
            cTab_player = missionNamespace getVariable ["BIS_fnc_moduleRemoteControl_unit",player];
            // close any interface that might still be open
            call cTab_fnc_close;
            //prep the arrays that will hold ctab data
            cTabBFTmembers = [];
            cTabBFTgroups = [];
            cTabBFTvehicles = [];
            cTabUAVlist = [];
            cTabHcamlist = [];
            call cTab_fnc_updateLists;
            call cTab_fnc_updateUserMarkerList;
            // remove msg notification
            cTabRscLayerMailNotification cutText ["", "PLAIN"];
        };
    }] call BIS_fnc_addStackedEventHandler;

    // add cTab_updatePulse event handler triggered periodically by the server
    ["cTab_updatePulse",cTab_fnc_updateLists] call CBA_fnc_addEventHandler;

    // setup default map style
    if (GVAR(defMapStyle) == "TOPO") then {
        ["cTab_Tablet_dlg",[["mapType","TOPO"]]] call cTab_fnc_setSettings;
        ["cTab_Android_dlg",[["mapType","TOPO"]]] call cTab_fnc_setSettings;
        ["cTab_FBCB2_dlg",[["mapType","TOPO"]]] call cTab_fnc_setSettings;
        ["cTab_TAD_dlg",[["mapType","TOPO"]]] call cTab_fnc_setSettings;
        ["cTab_MicroDAGR_dlg",[["mapType","TOPO"]]] call cTab_fnc_setSettings;
    };

}] call CBA_fnc_addEventHandler;