/*
    Function to execute the correct action when btnACT is pressed on Tablet
    No Parameters
    Returns TRUE
*/
_mode = ["cTab_Tablet_dlg","mode"] call cTab_fnc_getSettings;
call {
    if (_mode == "UAV") exitWith {[] call cTab_fnc_remoteControlUav;};
    if (_mode == "HCAM") exitWith {["cTab_Tablet_dlg",[["mode","HCAM_FULL"]]] call cTab_fnc_setSettings;};
    if (_mode == "HCAM_FULL") exitWith {["cTab_Tablet_dlg",[["mode","HCAM"]]] call cTab_fnc_setSettings;};
};
true