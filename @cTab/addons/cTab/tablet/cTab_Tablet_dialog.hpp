// cTab - Commander's Tablet with FBCB2 Blue Force Tracking
// Battlefield tablet to access real time intel and blue force tracker.
// By - Riouken
// http://forums.bistudio.com/member.php?64032-Riouken
// You may re-use any of this work as long as you provide credit back to me.


//--- cTab
#define GUI_GRID_H	(safezoneH * 1.2)
#define GUI_GRID_W	(GUI_GRID_H * 3/4)
// since the actual map position is not in the center, we correct for it by shifting it right
// (GUI_GRID_PX_W - cTab_GUI_tablet_MAP_W) / 2 - cTab_GUI_tablet_MAP_X
// is 96.5, that is the pixel amount we have to shift by, devided by GUI_GRID_PX_W
// to make it a ratio that we can apply to GUI_GRID_W in order to get a screen value to shift by
#define GUI_GRID_X	(safezoneX + (safezoneW - GUI_GRID_W) / 2 + (GUI_GRID_W * 96.5 / 2048))
#define GUI_GRID_Y	(safezoneY + (safezoneH - GUI_GRID_H) / 2)

#include <\cTab\tablet\cTab_Tablet_controls.hpp>

#define MENU_sizeEx pxToScreen_H_Value(cTab_GUI_tablet_OSD_TEXT_STD_SIZE)
#include "\cTab\shared\cTab_markerMenu_macros.hpp"

class cTab_Tablet_dlg {
	idd = 1775154;
	movingEnable = 1;
	onLoad = "_this call cTab_fnc_onIfOpen;";
	onUnload = "[] call cTab_fnc_onIfclose;";
	onKeyDown = "_this call cTab_fnc_onIfKeyDown;";
	objects[] = {};
	class controlsBackground {
		class windowsBG: cTab_RscPicture
		{
			idc = IDC_CTAB_WIN_BACK;
			text= "\cTab\img\desktop_co.paa";
			//text= "\cTab\img\desktop_1ergtd_co.paa";
			x = pxToScreen_X(cTab_GUI_tablet_MAP_X);
			y = pxToScreen_Y(cTab_GUI_tablet_MAP_Y);
			w = pxToScreen_W(cTab_GUI_tablet_MAP_W);
			h = pxToScreen_H(cTab_GUI_tablet_MAP_H);
		};
		class MiniMapBG: cTab_Tablet_window_back_BL
		{
			idc = IDC_CTAB_MINIMAPBG;
		};
		class cTabUavMap: cTab_Tablet_RscMapControl
		{
			idc = IDC_CTAB_CTABUAVMAP;
			x = pxToScreen_X(cTab_GUI_tablet_WINDOW_CONTENT_L_X);
			y = pxToScreen_Y(cTab_GUI_tablet_WINDOW_CONTENT_B_Y);
			w = pxToScreen_W(cTab_GUI_tablet_WINDOW_CONTENT_W);
			h = pxToScreen_H(cTab_GUI_tablet_WINDOW_CONTENT_H);
			onDraw = "nop = _this call cTabOnDrawUAV;";
			onMouseButtonDblClick = "";
		};
		class cTabHcamMap: cTabUavMap
		{
			idc = IDC_CTAB_CTABHCAMMAP;
			onDraw = "nop = _this call cTabOnDrawHCam;";
		};
		class screen: cTab_Tablet_RscMapControl
		{
			idc = IDC_CTAB_SCREEN;
			onDraw = "nop = _this call cTabOnDrawbft;";
			onMouseButtonDblClick = "_ok = [3300,_this] execVM '\cTab\shared\cTab_markerMenu_load.sqf';";
			onMouseMoving = "cTabCursorOnMap = _this select 3;cTabMapCursorPos = _this select 0 ctrlMapScreenToWorld [_this select 1,_this select 2];";
		};
		class screenTopo: screen
		{
			idc = IDC_CTAB_SCREEN_TOPO;
			maxSatelliteAlpha = 0;
		};
	};
	class controls {
		class header: cTab_tablet_header {};
		class battery: cTab_Tablet_OSD_battery {};
		class time: cTab_Tablet_OSD_time {};
		class signalStrength: cTab_Tablet_OSD_signalStrength {};
		class satellite: cTab_Tablet_OSD_satellite {};
		class dirDegree: cTab_Tablet_OSD_dirDegree {};
		class grid: cTab_Tablet_OSD_grid {};
		class dirOctant: cTab_Tablet_OSD_dirOctant {};
		class hookGrid: cTab_Tablet_OSD_hookGrid {};
		class hookElevation: cTab_Tablet_OSD_hookElevation {};
		class hookDst: cTab_Tablet_OSD_hookDst {};
		class hookDir: cTab_Tablet_OSD_hookDir {};
		// ---------- DESKTOP -----------
		class Desktop: cTab_RscControlsGroup
		{
			idc = IDC_CTAB_GROUP_DESKTOP;
			x = pxToScreen_X(cTab_GUI_tablet_SCREEN_CONTENT_X);
			y = pxToScreen_Y(cTab_GUI_tablet_SCREEN_CONTENT_Y);
			w = pxToScreen_W(cTab_GUI_tablet_SCREEN_CONTENT_W);
			h = pxToScreen_H(cTab_GUI_tablet_SCREEN_CONTENT_H);
			class VScrollbar {};
			class HScrollbar {};
			class Scrollbar {};
			class controls
			{
				class actBFTtxt: cTab_ActiveText
				{
					style = ST_PICTURE;
					idc = IDC_CTAB_ACTBFTTXT;
					text = "\cTab\img\icon_bft_ca.paa";
					//text = "\cTab\img\icon_map_ca.paa";
					x = pxToGroup_X(cTab_GUI_tablet_SCREEN_CONTENT_X + cTab_GUI_tablet_DESKTOP_ICON_OFFSET_X);
					y = pxToGroup_Y(cTab_GUI_tablet_SCREEN_CONTENT_Y + cTab_GUI_tablet_DESKTOP_ICON_OFFSET_Y);
					w = pxToScreen_W(cTab_GUI_tablet_DESKTOP_ICON_W);
					h = pxToScreen_H(cTab_GUI_tablet_DESKTOP_ICON_H);
					action = "['cTab_Tablet_dlg',[['mode','BFT']]] call cTab_fnc_setSettings;";
					toolTip = "$STR_ctab_core_BFTIconHint";
				};
				class actUAVtxt: actBFTtxt
				{
					idc = IDC_CTAB_ACTUAVTXT;
					text = "\cTab\img\icon_uav_ca.paa";
					y = pxToGroup_Y(cTab_GUI_tablet_SCREEN_CONTENT_Y + cTab_GUI_tablet_DESKTOP_ICON_OFFSET_Y * 2 + cTab_GUI_tablet_DESKTOP_ICON_H);
					action = "['cTab_Tablet_dlg',[['mode','UAV']]] call cTab_fnc_setSettings;";
					toolTip = "$STR_ctab_core_UAVIconHint";
				};
				class actVIDtxt: actBFTtxt
				{
					idc = IDC_CTAB_ACTVIDTXT;
					text = "\cTab\img\icon_helmetcam_ca.paa";
					y = pxToGroup_Y(cTab_GUI_tablet_SCREEN_CONTENT_Y + cTab_GUI_tablet_DESKTOP_ICON_OFFSET_Y * 3 + cTab_GUI_tablet_DESKTOP_ICON_H * 2);
					action = "['cTab_Tablet_dlg',[['mode','HCAM']]] call cTab_fnc_setSettings;";
					toolTip = "$STR_ctab_core_HelmetCamIconHint";
				};
				class actMSGtxt: actBFTtxt
				{
					idc = IDC_CTAB_ACTMSGTXT;
					text = "\cTab\img\icon_mail_ca.paa" ;
					y = pxToGroup_Y(cTab_GUI_tablet_SCREEN_CONTENT_Y + cTab_GUI_tablet_DESKTOP_ICON_OFFSET_Y * 4 + cTab_GUI_tablet_DESKTOP_ICON_H * 3);
					action = "['cTab_Tablet_dlg',[['mode','MESSAGE']]] call cTab_fnc_setSettings;";
					toolTip = "$STR_ctab_core_MessageIconHint";
				};
				class actINTELtxt: actBFTtxt
				{
					idc = IDC_CTAB_ACTMSGTXT;
					text = "z\ctab\addons\intel\data\feed_ca.paa" ;
					y = pxToGroup_Y(cTab_GUI_tablet_SCREEN_CONTENT_Y + cTab_GUI_tablet_DESKTOP_ICON_OFFSET_Y * 5 + cTab_GUI_tablet_DESKTOP_ICON_H * 4);
					action = "['cTab_Tablet_dlg',[['mode','INTELFEED']]] call cTab_fnc_setSettings;";
					toolTip = "$STR_ctab_intel_IntelFeed";
				};
			};
		};
		// ---------- UAV -----------
		class UAV: cTab_RscControlsGroup
		{
			idc = IDC_CTAB_GROUP_UAV;
			x = pxToScreen_X(cTab_GUI_tablet_SCREEN_CONTENT_X);
			y = pxToScreen_Y(cTab_GUI_tablet_SCREEN_CONTENT_Y);
			w = pxToScreen_W(cTab_GUI_tablet_SCREEN_CONTENT_W);
			h = pxToScreen_H(cTab_GUI_tablet_SCREEN_CONTENT_H);
			class VScrollbar {};
			class HScrollbar {};
			class Scrollbar {};
			class controls
			{
				class UAVListBG: cTab_Tablet_window_back_TL
				{
					idc=9;
					x = pxToGroup_X(cTab_GUI_tablet_WINDOW_BACK_L_X);
					y = pxToGroup_Y(cTab_GUI_tablet_WINDOW_BACK_T_Y);
				};
				class HUAVListTL : cTab_RscText_WindowTitle 
				{
					x = pxToGroup_X(cTab_GUI_tablet_WINDOW_BACK_L_X);
					y = pxToGroup_Y(cTab_GUI_tablet_WINDOW_BACK_T_Y);
					text = "List of UAV";
				};
				class UAVVidBG1: cTab_Tablet_window_back_TR
				{
					idc=10;
					x = pxToGroup_X(cTab_GUI_tablet_WINDOW_BACK_R_X);
					y = pxToGroup_Y(cTab_GUI_tablet_WINDOW_BACK_T_Y);
				};
				class UAVVidTL1 : cTab_RscText_WindowTitle 
				{
					x = pxToGroup_X(cTab_GUI_tablet_WINDOW_BACK_R_X);
					y = pxToGroup_Y(cTab_GUI_tablet_WINDOW_BACK_T_Y);
					text = "Video stream - Normal";
				};
				class UAVVidBG2: cTab_Tablet_window_back_BR
				{
					idc=11;
					x = pxToGroup_X(cTab_GUI_tablet_WINDOW_BACK_R_X);
					y = pxToGroup_Y(cTab_GUI_tablet_WINDOW_BACK_B_Y);
				};
				class UAVVidTL2 : cTab_RscText_WindowTitle 
				{
					x = pxToGroup_X(cTab_GUI_tablet_WINDOW_BACK_R_X);
					y = pxToGroup_Y(cTab_GUI_tablet_WINDOW_BACK_B_Y);
					text = "Video stream - Thermal";
				};
				class cTabUAVlist: cTab_RscListBox_Tablet
				{
					idc = IDC_CTAB_CTABUAVLIST;
					x = pxToGroup_X(cTab_GUI_tablet_WINDOW_CONTENT_L_X);
					y = pxToGroup_Y(cTab_GUI_tablet_WINDOW_CONTENT_T_Y);
					w = pxToScreen_W(cTab_GUI_tablet_WINDOW_CONTENT_W);
					h = pxToScreen_H(cTab_GUI_tablet_WINDOW_CONTENT_H);
					onLBSelChanged = "if (!cTabIfOpenStart && (_this select 1 != -1)) then {['cTab_Tablet_dlg',[['uavCam',(_this select 0) lbData (_this select 1)]]] call cTab_fnc_setSettings;};";
				};
				class cTabUAVdisplay: cTab_RscPicture
				{
					idc = IDC_CTAB_CTABUAVDISPLAY;
					text = "#(argb,512,512,1)r2t(rendertarget8,1.1896551724)";
					x = pxToGroup_X(cTab_GUI_tablet_WINDOW_CONTENT_R_X);
					y = pxToGroup_Y(cTab_GUI_tablet_WINDOW_CONTENT_T_Y);
					w = pxToScreen_W(cTab_GUI_tablet_WINDOW_CONTENT_W);
					h = pxToScreen_H(cTab_GUI_tablet_WINDOW_CONTENT_H);
				};
				class cTabUAV2nddisplay: cTab_RscPicture
				{
					idc = IDC_CTAB_CTABUAV2NDDISPLAY;
					text = "#(argb,512,512,1)r2t(rendertarget9,1.1896551724)";
					x = pxToGroup_X(cTab_GUI_tablet_WINDOW_CONTENT_R_X);
					y = pxToGroup_Y(cTab_GUI_tablet_WINDOW_CONTENT_B_Y);
					w = pxToScreen_W(cTab_GUI_tablet_WINDOW_CONTENT_W);
					h = pxToScreen_H(cTab_GUI_tablet_WINDOW_CONTENT_H);
				};
			};
		};
		// ---------- HELMET CAM -----------
		class HCAM: cTab_RscControlsGroup
		{
			idc = IDC_CTAB_GROUP_HCAM;
			x = pxToScreen_X(cTab_GUI_tablet_SCREEN_CONTENT_X);
			y = pxToScreen_Y(cTab_GUI_tablet_SCREEN_CONTENT_Y);
			w = pxToScreen_W(cTab_GUI_tablet_SCREEN_CONTENT_W);
			h = pxToScreen_H(cTab_GUI_tablet_SCREEN_CONTENT_H);
			class VScrollbar {};
			class HScrollbar {};
			class Scrollbar {};
			class controls
			{
				class HcamListBG: cTab_Tablet_window_back_TL
				{
					idc=12;
					x = pxToGroup_X(cTab_GUI_tablet_WINDOW_BACK_L_X);
					y = pxToGroup_Y(cTab_GUI_tablet_WINDOW_BACK_T_Y);
				};
				class HcamListTL : cTab_RscText_WindowTitle 
				{
					x = pxToGroup_X(cTab_GUI_tablet_WINDOW_BACK_L_X);
					y = pxToGroup_Y(cTab_GUI_tablet_WINDOW_BACK_T_Y);
					text = "List of helmet cameras";
				};
				class HcamVidBG: cTab_Tablet_window_back_TR
				{
					idc=13;
					x = pxToGroup_X(cTab_GUI_tablet_WINDOW_BACK_R_X);
					y = pxToGroup_Y(cTab_GUI_tablet_WINDOW_BACK_T_Y);
				};
				class HcamVidTL : cTab_RscText_WindowTitle 
				{
					x = pxToGroup_X(cTab_GUI_tablet_WINDOW_BACK_R_X);
					y = pxToGroup_Y(cTab_GUI_tablet_WINDOW_BACK_T_Y);
					text = "Video stream";
				};
				class cTabHcamList: cTab_RscListBox_Tablet
				{
					idc = IDC_CTAB_CTABHCAMLIST;
					x = pxToGroup_X(cTab_GUI_tablet_WINDOW_CONTENT_L_X);
					y = pxToGroup_Y(cTab_GUI_tablet_WINDOW_CONTENT_T_Y);
					w = pxToScreen_W(cTab_GUI_tablet_WINDOW_CONTENT_W);
					h = pxToScreen_H(cTab_GUI_tablet_WINDOW_CONTENT_H);
					onLBSelChanged = "if (!cTabIfOpenStart && (_this select 1 != -1)) then {['cTab_Tablet_dlg',[['hCam',(_this select 0) lbData (_this select 1)]]] call cTab_fnc_setSettings;};";
				};
				class cTabHcamDisplay: cTab_RscPicture
				{
					idc = IDC_CTAB_CTABHCAMDISPLAY;
					text = "#(argb,512,512,1)r2t(rendertarget12,1.1896551724)";
					x = pxToGroup_X(cTab_GUI_tablet_WINDOW_CONTENT_R_X);
					y = pxToGroup_Y(cTab_GUI_tablet_WINDOW_CONTENT_T_Y);
					w = pxToScreen_W(cTab_GUI_tablet_WINDOW_CONTENT_W);
					h = pxToScreen_H(cTab_GUI_tablet_WINDOW_CONTENT_H);
				};
			};
		};
		// ---------- MESSAGING -----------
		class MESSAGE: cTab_RscControlsGroup
		{
			idc = IDC_CTAB_GROUP_MESSAGE;
			x = pxToScreen_X(cTab_GUI_tablet_SCREEN_CONTENT_X);
			y = pxToScreen_Y(cTab_GUI_tablet_SCREEN_CONTENT_Y);
			w = pxToScreen_W(cTab_GUI_tablet_SCREEN_CONTENT_W);
			h = pxToScreen_H(cTab_GUI_tablet_SCREEN_CONTENT_H - cTab_GUI_tablet_TASKBAR_H);
			class VScrollbar {};
			class HScrollbar {};
			class Scrollbar {};
			class controls
			{
				class msgframe: cTab_RscFrame
				{
					idc=14;
					text = "$STR_ctab_core_InboxTitle";
					x = pxToGroup_X(cTab_GUI_tablet_MESSAGE_MESSAGETEXT_FRAME_X);
					y = pxToGroup_Y(cTab_GUI_tablet_MESSAGE_MESSAGETEXT_FRAME_Y);
					w = pxToScreen_W(cTab_GUI_tablet_MESSAGE_MESSAGETEXT_FRAME_W);
					h = pxToScreen_H(cTab_GUI_tablet_MESSAGE_MESSAGETEXT_FRAME_H);
				};
				class msgListbox: cTab_RscListBox_Tablet
				{
					idc = IDC_CTAB_MSG_LIST;
					style = LB_MULTI;
					x = pxToGroup_X(cTab_GUI_tablet_MESSAGE_MESSAGELIST_X);
					y = pxToGroup_Y(cTab_GUI_tablet_MESSAGE_MESSAGELIST_Y);
					w = pxToScreen_W(cTab_GUI_tablet_MESSAGE_MESSAGELIST_W);
					h = pxToScreen_H(cTab_GUI_tablet_MESSAGE_MESSAGELIST_H);
					onLBSelChanged = "_this call cTab_msg_get_mailTxt;";
				};
				class msgTxt: cTab_RscEdit_Tablet
				{
					idc = IDC_CTAB_MSG_CONTENT;
					htmlControl = 1;
					style = ST_MULTI;
					lineSpacing = 0.2;
					text = "$STR_ctab_core_NoMessageSelected";
					x = pxToGroup_X(cTab_GUI_tablet_MESSAGE_MESSAGETEXT_X);
					y = pxToGroup_Y(cTab_GUI_tablet_MESSAGE_MESSAGETEXT_Y);
					w = pxToScreen_W(cTab_GUI_tablet_MESSAGE_MESSAGETEXT_W);
					h = pxToScreen_H(cTab_GUI_tablet_MESSAGE_MESSAGETEXT_H);
					canModify = 0;
				};
				class composeFrame: cTab_RscFrame
				{
					idc=15;
					text = "$STR_ctab_core_ComposeMessageHint";
					x = pxToGroup_X(cTab_GUI_tablet_MESSAGE_COMPOSE_FRAME_X);
					y = pxToGroup_Y(cTab_GUI_tablet_MESSAGE_COMPOSE_FRAME_Y);
					w = pxToScreen_W(cTab_GUI_tablet_MESSAGE_COMPOSE_FRAME_W);
					h = pxToScreen_H(cTab_GUI_tablet_MESSAGE_COMPOSE_FRAME_H);
				};
				class playerlistbox: cTab_RscListBox_Tablet
				{
					idc = IDC_CTAB_MSG_RECIPIENTS;
					style = LB_MULTI;
					x = pxToGroup_X(cTab_GUI_tablet_MESSAGE_PLAYERLIST_X);
					y = pxToGroup_Y(cTab_GUI_tablet_MESSAGE_PLAYERLIST_Y);
					w = pxToScreen_W(cTab_GUI_tablet_MESSAGE_PLAYERLIST_W);
					h = pxToScreen_H(cTab_GUI_tablet_MESSAGE_PLAYERLIST_H);
				};
				class deletebtn: cTab_RscButton_Tablet_Danger
				{
					idc = IDC_CTAB_MSG_BTNDELETE;
					text = "$STR_ctab_core_DeleteMessage";
					tooltip = "$STR_ctab_core_DeleteMessageHint";
					x = pxToGroup_X(cTab_GUI_tablet_MESSAGE_BUTTON_DELETE_X);
					y = pxToGroup_Y(cTab_GUI_tablet_MESSAGE_BUTTON_DELETE_Y);
					w = pxToScreen_W(cTab_GUI_tablet_MESSAGE_BUTTON_W);
					h = pxToScreen_H(cTab_GUI_tablet_MESSAGE_BUTTON_H);
					action = "['cTab_Tablet_dlg'] call cTab_fnc_onMsgBtnDelete;";
				};
				class sendbtn: cTab_RscButton_Tablet
				{
					idc = IDC_CTAB_MSG_BTNSEND;
					text = "$STR_ctab_core_SendMessage";
					x = pxToGroup_X(cTab_GUI_tablet_MESSAGE_BUTTON_SEND_X);
					y = pxToGroup_Y(cTab_GUI_tablet_MESSAGE_BUTTON_SEND_Y);
					w = pxToScreen_W(cTab_GUI_tablet_MESSAGE_BUTTON_W);
					h = pxToScreen_H(cTab_GUI_tablet_MESSAGE_BUTTON_H);
					action = "call cTab_msg_Send;";
				};
				
				class MedEvacBtn: cTab_RscButton_Tablet
				{
					idc = IDC_CTAB_MSG_MEDEVAC;
					text = "$STR_ctab_messaging_MEDEVAC";
					x = pxToGroup_X(cTab_GUI_tablet_MESSAGE_MESSAGETEXT_X + cTab_GUI_tablet_MESSAGE_BUTTON_W + cTab_GUI_tablet_MESSAGE_MARGIN_INNER);
					y = pxToGroup_Y(cTab_GUI_tablet_MESSAGE_BUTTON_SEND_Y);
					w = pxToScreen_W(cTab_GUI_tablet_MESSAGE_BUTTON_W);
					h = pxToScreen_H(cTab_GUI_tablet_MESSAGE_BUTTON_H);
					onButtonClick = QUOTE(_this call EFUNC(messaging,btnShowMedevacTemplate););
				};			
				class TemplatesBtn: cTab_RscButton_Tablet
				{
					idc = IDC_CTAB_MSG_TEMPLATES;
					text = "$STR_ctab_messaging_Templates";
					x = pxToGroup_X(cTab_GUI_tablet_MESSAGE_MESSAGETEXT_X);
					y = pxToGroup_Y(cTab_GUI_tablet_MESSAGE_BUTTON_SEND_Y);
					w = pxToScreen_W(cTab_GUI_tablet_MESSAGE_BUTTON_W);
					h = pxToScreen_H(cTab_GUI_tablet_MESSAGE_BUTTON_H);
					onButtonClick = QUOTE(_this call EFUNC(messaging,btnShowTemplates););
				};

				class edittxtbox: cTab_RscEdit_Tablet
				{
					idc = IDC_CTAB_MSG_COMPOSE;
					htmlControl = 1;
					style = ST_MULTI;
					lineSpacing = 0.2;
					text = "";
					x = pxToGroup_X(cTab_GUI_tablet_MESSAGE_COMPOSE_TEXT_X);
					y = pxToGroup_Y(cTab_GUI_tablet_MESSAGE_COMPOSE_TEXT_Y);
					w = pxToScreen_W(cTab_GUI_tablet_MESSAGE_COMPOSE_TEXT_W);
					h = pxToScreen_H(cTab_GUI_tablet_MESSAGE_COMPOSE_TEXT_H);
				};
			};
		};
		// ---------- FULLSCREEN HCAM -----------
		class cTabHcamFull: cTab_RscPicture
		{
			idc = IDC_CTAB_HCAM_FULL;
			text = "#(argb,512,512,1)r2t(rendertarget13,1.3096153846)";
			x = pxToScreen_X(cTab_GUI_tablet_SCREEN_CONTENT_X);
			y = pxToScreen_Y(cTab_GUI_tablet_SCREEN_CONTENT_Y);
			w = pxToScreen_W(cTab_GUI_tablet_SCREEN_CONTENT_W);
			h = pxToScreen_H(cTab_GUI_tablet_SCREEN_CONTENT_H);
		};

		/*
			### Overlays ###
		*/
		// ---------- NOTIFICATION ------------
		class notification: cTab_Tablet_notification {};
		// ---------- LOADING ------------
		class loadingtxt: cTab_Tablet_loadingtxt {};
		// ---------- BRIGHTNESS ------------
		class brightness: cTab_Tablet_brightness {};
		// ---------- USER MARKERS ------------
		#define cTab_IS_TABLET
		#include "\cTab\shared\cTab_markerMenu_controls.hpp"
		#undef cTab_IS_TABLET
		// ---------- BACKGROUND ------------
		class background: cTab_Tablet_background {};
		// ---------- MOVING HANDLEs ------------
		class movingHandle_T: cTab_Tablet_movingHandle_T {};
		class movingHandle_B: cTab_Tablet_movingHandle_B {};
		class movingHandle_L: cTab_Tablet_movingHandle_L {};
		class movingHandle_R: cTab_Tablet_movingHandle_R {};

		/*
			### PHYSICAL BUTTONS ###
		*/
		class btnF1: cTab_Tablet_btnF1
		{
			idc = IDC_CTAB_BTNF1;
			tooltip = "$STR_ctab_core_BFTF1Hint";
			action = "['cTab_Tablet_dlg',[['mode','BFT']]] call cTab_fnc_setSettings;";
		};
		class btnF2: cTab_Tablet_btnF2
		{
			idc = IDC_CTAB_BTNF2;
			tooltip = "$STR_ctab_core_UAVF2Hint";
			action = "['cTab_Tablet_dlg',[['mode','UAV']]] call cTab_fnc_setSettings;";
		};
		class btnF3: cTab_Tablet_btnF3
		{
			idc = IDC_CTAB_BTNF3;
			tooltip = "$STR_ctab_core_HelmetCamF3Hint";
			action = "['cTab_Tablet_dlg',[['mode','HCAM']]] call cTab_fnc_setSettings;";
		};
		class btnF4: cTab_Tablet_btnF4
		{
			idc = IDC_CTAB_BTNF4;
			tooltip = "$STR_ctab_core_MessageF4Hint";
			action = "['cTab_Tablet_dlg',[['mode','MESSAGE']]] call cTab_fnc_setSettings;";
		};
		class btnF5: cTab_Tablet_btnF5
		{
			idc = IDC_CTAB_BTNF5;
			tooltip = "$STR_ctab_core_MapToolsHint";
			action = "['cTab_Tablet_dlg'] call cTab_fnc_toggleMapTools;";
		};
		class btnF6: cTab_Tablet_btnF6
		{
			idc = IDC_CTAB_BTNF6;
			tooltip = "$STR_ctab_core_MapTexturesHint";
			action = "['cTab_Tablet_dlg'] call cTab_fnc_mapType_toggle;";
		};
		class btnF7: cTab_Tablet_btnTrackpad
		{
			idc=16;
			action = "['cTab_Tablet_dlg'] call cTab_fnc_centerMapOnPlayerPosition;";
			tooltip = "$STR_ctab_core_CenterMapHint";
		};
		class btnMain: cTab_Tablet_btnHome
		{
			idc = IDC_CTAB_BTNMAIN;
			tooltip = "$STR_ctab_core_HomeTabletHint";
			action = "['cTab_Tablet_dlg',[['mode','DESKTOP']]] call cTab_fnc_setSettings;";
		};
		class btnFN: cTab_Tablet_btnFn
		{
			idc = IDC_CTAB_BTNFN;
			action = "['cTab_Tablet_dlg'] call cTab_fnc_iconText_toggle;";
			tooltip = "$STR_ctab_core_TextOnOffHint";
		};
		class btnOFF: cTab_Tablet_btnPower
		{
			idc = IDC_CTAB_BTNOFF;
			action = "closeDialog 0;";
			tooltip = "$STR_ctab_core_CloseInterfaceHint";
		};
		class btnUP: cTab_Tablet_btnBrtUp
		{
			idc = IDC_CTAB_BTNUP;
			action = "call cTab_fnc_txt_size_inc;";
			tooltip = "$STR_ctab_core_IncreaseFontHint";
		};
		class btnDWN: cTab_Tablet_btnBrtDn
		{
			idc = IDC_CTAB_BTNDWN;
			action = "call cTab_fnc_txt_size_dec;";
			tooltip = "$STR_ctab_core_DecreaseFontHint";
		};
		class btnACT: cTab_Tablet_btnMouse
		{
			idc = IDC_CTAB_BTNACT;
			action = "_null = [] call cTab_Tablet_btnACT;";
			tooltip = "";
		};
	};
};