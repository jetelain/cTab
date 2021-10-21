#define GUI_GRID_X (0)
#define GUI_GRID_Y (0)
#define GUI_GRID_W (0.025)
#define GUI_GRID_H (0.04)
#define GUI_GRID_WAbs (1)
#define GUI_GRID_HAbs (1)

#include "technicaldata_defines.hpp"

class RscText;
class IGUIBack;
class RscEdit;
class RscCombo;
class RscCheckbox;
class RscButton;

class cTab_TechnicalData_dlg
{
    idd = IDD_TECHNICALDATA;
    onLoad = QUOTE(_this call FUNC(onTechnicalDataOpen););
    onUnload = QUOTE(_this call FUNC(onTechnicalDataClose););
    objects[] = {};
    class controlsBackground 
    {
        class Frame: IGUIBack
        {
            idc = IDC_TECHNICALDATA_FRAME;
            x = 2.5 * GUI_GRID_W + GUI_GRID_X;
            y = 7.5 * GUI_GRID_H + GUI_GRID_Y;
            w = 35 * GUI_GRID_W;
            h = 10 * GUI_GRID_H;
            colorBackground[] = {0,0,0,0.5};
        };
    };
    class controls 
    {
        class Title: RscText
        {
            idc = IDC_TECHNICALDATA_TITLE;
            text = $STR_ctab_core_techData;
            x = 3.5 * GUI_GRID_W + GUI_GRID_X;
            y = 8.5 * GUI_GRID_H + GUI_GRID_Y;
            w = 33 * GUI_GRID_W;
            h = 1 * GUI_GRID_H;
        };

        class CallsignLabel: RscText
        {
            idc = IDC_TECHNICALDATA_CALLSIGNLABEL;
            text = $STR_ctab_core_callSign;
            x = 3.5 * GUI_GRID_W + GUI_GRID_X;
            y = 10.5 * GUI_GRID_H + GUI_GRID_Y;
            w = 11 * GUI_GRID_W;
            h = 1 * GUI_GRID_H;
        };
        class Callsign: RscEdit
        {
            idc = IDC_TECHNICALDATA_CALLSIGN;
            text = "";
            x = 15.5 * GUI_GRID_W + GUI_GRID_X;
            y = 10.5 * GUI_GRID_H + GUI_GRID_Y;
            w = 21 * GUI_GRID_W;
            h = 1 * GUI_GRID_H;
        };

        class OkButton: RscButton
        {
            idc = IDC_TECHNICALDATA_OKBUTTON;
            text = $STR_DISP_OK;
            x = 28.5 * GUI_GRID_W + GUI_GRID_X;
            y = 16 * GUI_GRID_H + GUI_GRID_Y;
            w = 8 * GUI_GRID_W;
            h = 1 * GUI_GRID_H;
        };
        class CancelButton: RscButton
        {
            idc = IDC_TECHNICALDATA_CANCELBUTTON;
            text = $STR_DISP_CANCEL;
            x = 3.5 * GUI_GRID_W + GUI_GRID_X;
            y = 16 * GUI_GRID_H + GUI_GRID_Y;
            w = 8 * GUI_GRID_W;
            h = 1 * GUI_GRID_H;
        };
    };
};