// Background definition
#define GUI_GRID_PX_W 2048 // width in pixels
#define GUI_GRID_PX_H 2048 // hight in pixels

// Base bacros to convert pixel space to screen space
#define pxToScreen_X(PIXEL) QUOTE((PIXEL) / GUI_GRID_PX_W * GUI_GRID_W + GUI_GRID_X)
#define pxToScreen_Y(PIXEL) QUOTE((PIXEL) / GUI_GRID_PX_H * GUI_GRID_H + GUI_GRID_Y)
#define pxToScreen_W(PIXEL) QUOTE((PIXEL) / GUI_GRID_PX_W * GUI_GRID_W)
#define pxToScreen_H_Value(PIXEL) (PIXEL) / GUI_GRID_PX_H * GUI_GRID_H
#define pxToScreen_H(PIXEL) QUOTE(pxToScreen_H_Value(PIXEL))

// Map position within background, pixel based
#define cTab_GUI_microDAGR_MAP_X (573)
#define cTab_GUI_microDAGR_MAP_Y (317)
#define cTab_GUI_microDAGR_MAP_W (942)
#define cTab_GUI_microDAGR_MAP_H (1296)

// Height of header and footer OSD elements
#define cTab_GUI_microDAGR_OSD_HEADER_H (94)
#define cTab_GUI_microDAGR_OSD_FOOTER_H (cTab_GUI_microDAGR_OSD_HEADER_H)

// Screen content (the stuff that changes, so map area - header and footer)
#define cTab_GUI_microDAGR_SCREEN_CONTENT_X (cTab_GUI_microDAGR_MAP_X)
#define cTab_GUI_microDAGR_SCREEN_CONTENT_Y (cTab_GUI_microDAGR_MAP_Y + cTab_GUI_microDAGR_OSD_HEADER_H)
#define cTab_GUI_microDAGR_SCREEN_CONTENT_W (cTab_GUI_microDAGR_MAP_W)
#define cTab_GUI_microDAGR_SCREEN_CONTENT_H (cTab_GUI_microDAGR_MAP_H - cTab_GUI_microDAGR_OSD_HEADER_H - cTab_GUI_microDAGR_OSD_FOOTER_H)

// On-screen edge positions (left, right, top, bottom)
#define cTab_GUI_microDAGR_OSD_MARGIN (24)
#define cTab_GUI_microDAGR_OSD_EDGE_L (cTab_GUI_microDAGR_OSD_MARGIN + cTab_GUI_microDAGR_MAP_X)
#define cTab_GUI_microDAGR_OSD_EDGE_R (-cTab_GUI_microDAGR_OSD_MARGIN + cTab_GUI_microDAGR_MAP_X + cTab_GUI_microDAGR_MAP_W)
#define cTab_GUI_microDAGR_OSD_EDGE_T (cTab_GUI_microDAGR_OSD_MARGIN + cTab_GUI_microDAGR_MAP_Y)
#define cTab_GUI_microDAGR_OSD_EDGE_B (-cTab_GUI_microDAGR_OSD_FOOTER_H + cTab_GUI_microDAGR_MAP_Y + cTab_GUI_microDAGR_MAP_H)

// On-screen element base width and height
#define cTab_GUI_microDAGR_OSD_ELEMENT_STD_W ((cTab_GUI_microDAGR_MAP_W - cTab_GUI_microDAGR_OSD_MARGIN * 4) / 3)
#define cTab_GUI_microDAGR_OSD_ELEMENT_STD_H (cTab_GUI_microDAGR_OSD_HEADER_H - cTab_GUI_microDAGR_OSD_MARGIN)

// On-screen element X-coord for left, center and right elements
#define cTab_GUI_microDAGR_OSD_LEFT_X (cTab_GUI_microDAGR_OSD_EDGE_L)
#define cTab_GUI_microDAGR_OSD_CENTER_X (cTab_GUI_microDAGR_OSD_EDGE_L + cTab_GUI_microDAGR_OSD_MARGIN + (cTab_GUI_microDAGR_OSD_ELEMENT_STD_W*0.75))
#define cTab_GUI_microDAGR_OSD_RIGHT_X (cTab_GUI_microDAGR_OSD_EDGE_R - cTab_GUI_microDAGR_OSD_ELEMENT_STD_W)

// On-screen text sizes, hight in pixels
// Standard text elements
#define cTab_GUI_microDAGR_OSD_TEXT_STD_SIZE (60)
#define cTab_GUI_microDAGR_OSD_ICON_STD_SIZE (50)

// On-screen map centre cursor
#define cTab_GUI_microDAGR_CURSOR (38)

class cTab_microDAGR_header: cTab_RscPicture
{
	idc=1;
	text = "#(argb,8,8,3)color(0,0,0,1)";
	x = pxToScreen_X(cTab_GUI_microDAGR_MAP_X);
	y = pxToScreen_Y(cTab_GUI_microDAGR_MAP_Y);
	w = pxToScreen_W(cTab_GUI_microDAGR_MAP_W);
	h = pxToScreen_H(cTab_GUI_microDAGR_OSD_HEADER_H);
};
class cTab_microDAGR_footer: cTab_microDAGR_header
{
	idc=2;
	y = pxToScreen_Y(cTab_GUI_microDAGR_MAP_Y + cTab_GUI_microDAGR_MAP_H - cTab_GUI_microDAGR_OSD_FOOTER_H);
	h = pxToScreen_H(cTab_GUI_microDAGR_OSD_FOOTER_H);
};
class cTab_RscButton_microDAGR_LeftUp: cTab_RscButtonInv
{
	x = pxToScreen_X(373);
	y = pxToScreen_Y(400);
	w = pxToScreen_W(26);
	h = pxToScreen_H(171);
};
class cTab_RscButton_microDAGR_LeftDown: cTab_RscButton_microDAGR_LeftUp
{
	y = pxToScreen_Y(571);
};
class cTab_RscButton_microDAGR_LeftBtn: cTab_RscButtonInv
{
	x = pxToScreen_X(368);
	y = pxToScreen_Y(829);
	w = pxToScreen_W(31);
	h = pxToScreen_H(184);
};
class cTab_RscButton_microDAGR_RightUp: cTab_RscButton_microDAGR_LeftUp
{
	x = pxToScreen_X(1689);
};
class cTab_RscButton_microDAGR_RightDown: cTab_RscButton_microDAGR_LeftDown
{
	x = pxToScreen_X(1689);
};
class cTab_RscButton_microDAGR_RightBtn: cTab_RscButton_microDAGR_LeftBtn
{
	x = pxToScreen_X(1689);
};

class cTab_RscText_microDAGR: cTab_RscText
{
	style = ST_CENTER;
	w = pxToScreen_W(cTab_GUI_microDAGR_OSD_ELEMENT_STD_W);
	h = pxToScreen_H(cTab_GUI_microDAGR_OSD_ELEMENT_STD_H);
	font = QUOTE(GUI_FONT_MONO);
	colorText[] = COLOR_WHITE;
	sizeEx = pxToScreen_H(cTab_GUI_microDAGR_OSD_TEXT_STD_SIZE);
	colorBackground[] = COLOR_TRANSPARENT;
	shadow = 0;
};
class cTab_microDAGR_RscMapControl: cTab_RscMapControl
{
	idc = IDC_CTAB_SCREEN;
	text = "#(argb,8,8,3)color(1,1,1,1)";
	x = pxToScreen_X(cTab_GUI_microDAGR_SCREEN_CONTENT_X);
	y = pxToScreen_Y(cTab_GUI_microDAGR_SCREEN_CONTENT_Y);
	w = pxToScreen_W(cTab_GUI_microDAGR_SCREEN_CONTENT_W);
	h = pxToScreen_H(cTab_GUI_microDAGR_SCREEN_CONTENT_H);
	//type = CT_MAP;
	// allow to zoom out further (defines the maximum map scale, usually 1)
	scaleMax = 1000;
	// turn on satellite map information (defines the map scale of when to switch to topographical)
	maxSatelliteAlpha = 10000;
	alphaFadeStartScale = 10;
	alphaFadeEndScale = 10;

	// Rendering density coefficients
	ptsPerSquareSea = QUOTE(8 / cTab_microDAGR_DLGtoDSP_fctr);		// seas
	ptsPerSquareTxt = QUOTE(8 / cTab_microDAGR_DLGtoDSP_fctr);		// textures
	ptsPerSquareCLn = QUOTE(8 / cTab_microDAGR_DLGtoDSP_fctr);		// count-lines
	ptsPerSquareExp = QUOTE(8 / cTab_microDAGR_DLGtoDSP_fctr);		// exposure
	ptsPerSquareCost = QUOTE(8 / cTab_microDAGR_DLGtoDSP_fctr);		// cost

	// Rendering thresholds
	ptsPerSquareFor = QUOTE(3 / cTab_microDAGR_DLGtoDSP_fctr);		// forests
	ptsPerSquareForEdge = QUOTE(100 / cTab_microDAGR_DLGtoDSP_fctr);	// forest edges
	ptsPerSquareRoad = QUOTE(1.5 / cTab_microDAGR_DLGtoDSP_fctr);		// roads
	ptsPerSquareObj = QUOTE(4 / cTab_microDAGR_DLGtoDSP_fctr);		// other objects

	/*
	// replace CustomMark with wedding cake icon
	class CustomMark
	{
		icon = "\cTab\img\icon_wedding_cake_ca.paa";
		size = 18;
		importance = 1;
		coefMin = 1;
		coefMax = 1;
		color[] = {1,1,1,1};
		shadow = 1;
	};
	*/
};
class cTab_microDAGR_background: cTab_RscPicture
{
	idc = IDC_CTAB_BACKGROUND;
	text = ""; // will be set during onLoad event
	x = QUOTE(GUI_GRID_X);
	y = QUOTE(GUI_GRID_Y);
	w = QUOTE(GUI_GRID_W);
	h = QUOTE(GUI_GRID_H);
};
class cTab_microDAGR_cursor: cTab_RscPicture
{
	idc=3;
	text = "\a3\ui_f\data\IGUI\Cfg\WeaponCursors\cursoraimon_gs.paa";
		// "\a3\ui_f\data\map\Markers\Military\destroy_ca.paa";
		// "\a3\ui_f\data\IGUI\Cfg\WeaponCursors\cursoraimon_gs.paa"
		// "\a3\ui_f\data\map\MarkerBrushes\cross_ca.paa"
	x = pxToScreen_X(cTab_GUI_microDAGR_MAP_X + cTab_GUI_microDAGR_MAP_W / 2 - 128 / 33 * cTab_GUI_microDAGR_CURSOR / 2);
	y = pxToScreen_Y(cTab_GUI_microDAGR_MAP_Y + cTab_GUI_microDAGR_MAP_H / 2 - 128 / 33 * cTab_GUI_microDAGR_CURSOR / 2);
	w = pxToScreen_W(128 / 33 * cTab_GUI_microDAGR_CURSOR);
	h = pxToScreen_H(128 / 33 * cTab_GUI_microDAGR_CURSOR);
	colorText[] = COLOR_NEON_GREEN;
};
class cTab_microDAGR_btnF7: cTab_RscButton_microDAGR_LeftBtn
{
	idc=4;
	tooltip = "$STR_ctab_core_CenterMapHint";
};
class cTab_microDAGR_btnbrtpls: cTab_RscButton_microDAGR_RightUp
{
	idc = IDC_CTAB_BTNUP;
	action = "call cTab_fnc_txt_size_inc;";
	tooltip = "$STR_ctab_core_IncreaseFontHint";
};
class cTab_microDAGR_btnbrtmns: cTab_RscButton_microDAGR_RightDown
{
	idc = IDC_CTAB_BTNDWN;
	action = "call cTab_fnc_txt_size_dec;";
	tooltip = "$STR_ctab_core_DecreaseFontHint";
};
class cTab_microDAGR_btnfunction: cTab_RscButton_microDAGR_RightBtn
{
	idc = IDC_CTAB_BTNFN;
	tooltip = "$STR_ctab_core_TextOnOffHint";
};
class cTab_microDAGR_btnMapType: cTab_RscButton_microDAGR_LeftUp
{
	idc=5;
	tooltip = "$STR_ctab_core_MapTexturesHint";
};
class cTab_microDAGR_btnMapTools: cTab_RscButton_microDAGR_LeftDown
{
	idc=6;
	tooltip = "$STR_ctab_core_MapToolsHint";
};
class cTab_microDAGR_on_screen_battery: cTab_RscPicture
{
	idc=7;
	text = "\cTab\img\icon_battery_ca.paa";
	x = pxToScreen_X(cTab_GUI_microDAGR_OSD_LEFT_X);
	y = pxToScreen_Y(cTab_GUI_microDAGR_OSD_EDGE_T + (cTab_GUI_microDAGR_OSD_ELEMENT_STD_H - cTab_GUI_microDAGR_OSD_ICON_STD_SIZE) / 2);
	w = pxToScreen_W(cTab_GUI_microDAGR_OSD_ICON_STD_SIZE);
	h = pxToScreen_H(cTab_GUI_microDAGR_OSD_ICON_STD_SIZE);
	colorText[] = COLOR_WHITE;
};
class cTab_microDAGR_on_screen_time: cTab_RscText_microDAGR
{
	idc = IDC_CTAB_OSD_TIME;
	style = ST_CENTER;
	x = pxToScreen_X(cTab_GUI_microDAGR_OSD_CENTER_X);
	y = pxToScreen_Y(cTab_GUI_microDAGR_OSD_EDGE_T);
	w = pxToScreen_W(cTab_GUI_microDAGR_OSD_ELEMENT_STD_W*1.5);
};
class cTab_microDAGR_on_screen_signalStrength: cTab_microDAGR_on_screen_battery
{
	idc=8;
	text = "\cTab\img\icon_signalStrength_ca.paa";
	x = pxToScreen_X(cTab_GUI_microDAGR_OSD_RIGHT_X + cTab_GUI_microDAGR_OSD_ELEMENT_STD_W - cTab_GUI_microDAGR_OSD_ICON_STD_SIZE * 2);
	colorText[] = COLOR_WHITE;
};
class cTab_microDAGR_on_screen_satellite: cTab_microDAGR_on_screen_battery
{
	idc=9;
	text = "\a3\ui_f\data\map\Diary\signal_ca.paa";
	x = pxToScreen_X(cTab_GUI_microDAGR_OSD_RIGHT_X + cTab_GUI_microDAGR_OSD_ELEMENT_STD_W - cTab_GUI_microDAGR_OSD_ICON_STD_SIZE);
	colorText[] = COLOR_WHITE;
};
class cTab_microDAGR_on_screen_dirDegree: cTab_RscText_microDAGR
{
	idc = IDC_CTAB_OSD_DIR_DEGREE;
	style = ST_LEFT;
	y = pxToScreen_Y(cTab_GUI_microDAGR_OSD_EDGE_B);
	x = pxToScreen_X(cTab_GUI_microDAGR_OSD_LEFT_X);
};
class cTab_microDAGR_on_screen_grid: cTab_microDAGR_on_screen_dirDegree
{
	idc = IDC_CTAB_OSD_GRID;
	style = ST_CENTER;
	x = pxToScreen_X(cTab_GUI_microDAGR_OSD_CENTER_X);
	w = pxToScreen_W(cTab_GUI_microDAGR_OSD_ELEMENT_STD_W*1.5);
};
class cTab_microDAGR_on_screen_dirOctant: cTab_microDAGR_on_screen_dirDegree
{
	idc = IDC_CTAB_OSD_DIR_OCTANT;
	style = ST_RIGHT;
	x = pxToScreen_X(cTab_GUI_microDAGR_OSD_RIGHT_X);
};
class cTab_microDAGR_on_screen_hookGrid: cTab_RscText_microDAGR
{
	idc = IDC_CTAB_OSD_HOOK_GRID;
	style = ST_CENTER;
	x = pxToScreen_X(cTab_GUI_microDAGR_OSD_RIGHT_X);
	y = pxToScreen_Y(cTab_GUI_microDAGR_OSD_EDGE_B - cTab_GUI_microDAGR_OSD_MARGIN - cTab_GUI_microDAGR_OSD_ELEMENT_STD_H * 4);
	colorText[] = {1,1,1,0.75};
	colorBackground[] = {0,0,0,0.2};
	sizeEx = pxToScreen_H(48);
};
class cTab_microDAGR_on_screen_hookElevation: cTab_microDAGR_on_screen_hookGrid
{
	idc = IDC_CTAB_OSD_HOOK_ELEVATION;
	y = pxToScreen_Y(cTab_GUI_microDAGR_OSD_EDGE_B - cTab_GUI_microDAGR_OSD_MARGIN - cTab_GUI_microDAGR_OSD_ELEMENT_STD_H * 3);
};
class cTab_microDAGR_on_screen_hookDst: cTab_microDAGR_on_screen_hookGrid
{
	idc = IDC_CTAB_OSD_HOOK_DST;
	y = pxToScreen_Y(cTab_GUI_microDAGR_OSD_EDGE_B - cTab_GUI_microDAGR_OSD_MARGIN - cTab_GUI_microDAGR_OSD_ELEMENT_STD_H * 2);
};
class cTab_microDAGR_on_screen_hookDir: cTab_microDAGR_on_screen_hookGrid
{
	idc = IDC_CTAB_OSD_HOOK_DIR;
	y = pxToScreen_Y(cTab_GUI_microDAGR_OSD_EDGE_B - cTab_GUI_microDAGR_OSD_MARGIN - cTab_GUI_microDAGR_OSD_ELEMENT_STD_H);
};
class cTab_microDAGR_loadingtxt: cTab_RscText_microDAGR
{
	idc = IDC_CTAB_LOADINGTXT;
	style = ST_CENTER;
	text = "$STR_ctab_core_Loading";
	x = pxToScreen_X(cTab_GUI_microDAGR_SCREEN_CONTENT_X);
	y = pxToScreen_Y(cTab_GUI_microDAGR_SCREEN_CONTENT_Y);
	w = pxToScreen_W(cTab_GUI_microDAGR_SCREEN_CONTENT_W);
	h = pxToScreen_H(cTab_GUI_microDAGR_SCREEN_CONTENT_H);
	colorBackground[] = COLOR_TRANSPARENT;
};

// Define areas around the screen as interaction areas to allow screen movement
class cTab_microDAGR_movingHandle_T: cTab_RscText_microDAGR
{
	idc=10;
	moving = 1;
	colorBackground[] = COLOR_TRANSPARENT;
	x = pxToScreen_X(0);
	y = pxToScreen_Y(0);
	w = pxToScreen_W(GUI_GRID_PX_W);
	h = pxToScreen_H(cTab_GUI_microDAGR_MAP_Y);
};
class cTab_microDAGR_movingHandle_B: cTab_microDAGR_movingHandle_T
{
	idc=11;
	y = pxToScreen_Y(cTab_GUI_microDAGR_MAP_Y + cTab_GUI_microDAGR_MAP_H);
	h = pxToScreen_H(GUI_GRID_PX_H - (cTab_GUI_microDAGR_MAP_Y + cTab_GUI_microDAGR_MAP_H));
};
class cTab_microDAGR_movingHandle_L: cTab_microDAGR_movingHandle_T
{
	idc=12;
	y = pxToScreen_Y(cTab_GUI_microDAGR_MAP_Y);
	w = pxToScreen_W(cTab_GUI_microDAGR_MAP_X);
	h = pxToScreen_H(cTab_GUI_microDAGR_MAP_H);
};
class cTab_microDAGR_movingHandle_R: cTab_microDAGR_movingHandle_L
{
	idc=13;
	x = pxToScreen_X(cTab_GUI_microDAGR_MAP_X + cTab_GUI_microDAGR_MAP_W);
	w = pxToScreen_W(GUI_GRID_PX_W - (cTab_GUI_microDAGR_MAP_X + cTab_GUI_microDAGR_MAP_W));
};

// transparent control that gets placed on top of the GUI to adjust brightness
class cTab_microDAGR_brightness: cTab_RscText_microDAGR
{
	idc = IDC_CTAB_BRIGHTNESS;
	x = pxToScreen_X(cTab_GUI_microDAGR_MAP_X);
	y = pxToScreen_Y(cTab_GUI_microDAGR_MAP_Y);
	w = pxToScreen_W(cTab_GUI_microDAGR_MAP_W);
	h = pxToScreen_H(cTab_GUI_microDAGR_MAP_H);
	colorBackground[] = COLOR_TRANSPARENT;
};