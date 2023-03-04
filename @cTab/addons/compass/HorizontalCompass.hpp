#define BAR_WIDTH   safezoneW
#define BAR_HEIGHT (0.055 * safezoneH)

class RscControlsGroupNoScrollbars;
class RscMapControlEmpty;
class RscPicture;
class RscText;

class GVARMAIN(HorizontalCompass) : RscControlsGroupNoScrollbars
{

    onLoad=QUOTE(_this call FUNC(initCompass));
    onUnload=QUOTE(_this call FUNC(disposeCompass));
    w = BAR_WIDTH;
    h = BAR_HEIGHT;
    x = safezoneX;
    y = 0.845 * safezoneH + safezoneY;

    class Controls
    {
        class Map : RscMapControlEmpty
        {
            idc=-1;
            w=0;
            h=0;
            onDraw=QUOTE([ctrlparentcontrolsgroup (_this # 0)] call FUNC(updateCompass));
            scaleMax=0.001;
            scaleMin=0.001;
        };
        class Compass : RscControlsGroupNoScrollbars
        {
            idc=9000;
            w = BAR_WIDTH;
            h = BAR_HEIGHT;
            class Controls
            {
                class Bar1 : RscControlsGroupNoScrollbars
                {
                    idc=9001;
                    w = BAR_WIDTH*2;
                    h = BAR_HEIGHT;
                    class Controls
                    {
                        class Indicator : RscPicture
                        {
                            idc=9101;
                            w = BAR_WIDTH*2;
                            h = BAR_HEIGHT;
                            text=QPATHTOF(data\ns_ca.paa); // North > East > South
                        };
                        class B00 : RscText
                        {
                            idc=9200;
                            w = BAR_WIDTH/18;
                            h = BAR_HEIGHT/4;
                            y = BAR_HEIGHT*3/4;
                            colorBackground[] = { 1, 0, 0, 0.8 };
                        };
                        class B01 : B00 { idc=9201; x = BAR_WIDTH*01/18; };
                        class B02 : B00 { idc=9202; x = BAR_WIDTH*02/18; };
                        class B03 : B00 { idc=9203; x = BAR_WIDTH*03/18; };
                        class B04 : B00 { idc=9204; x = BAR_WIDTH*04/18; };
                        class B05 : B00 { idc=9205; x = BAR_WIDTH*05/18; };
                        class B06 : B00 { idc=9206; x = BAR_WIDTH*06/18; };
                        class B07 : B00 { idc=9207; x = BAR_WIDTH*07/18; };
                        class B08 : B00 { idc=9208; x = BAR_WIDTH*08/18; };
                        class B09 : B00 { idc=9209; x = BAR_WIDTH*09/18; };
                        class B10 : B00 { idc=9210; x = BAR_WIDTH*10/18; };
                        class B11 : B00 { idc=9211; x = BAR_WIDTH*11/18; };
                        class B12 : B00 { idc=9212; x = BAR_WIDTH*12/18; };
                        class B13 : B00 { idc=9213; x = BAR_WIDTH*13/18; };
                        class B14 : B00 { idc=9214; x = BAR_WIDTH*14/18; };
                        class B15 : B00 { idc=9215; x = BAR_WIDTH*15/18; };
                        class B16 : B00 { idc=9216; x = BAR_WIDTH*16/18; };
                        class B17 : B00 { idc=9217; x = BAR_WIDTH*17/18; };
                        class B18 : B00 { idc=9218; x = BAR_WIDTH*18/18; };
                        class B19 : B00 { idc=9219; x = BAR_WIDTH*19/18; };
                        class B20 : B00 { idc=9220; x = BAR_WIDTH*20/18; };
                        class B21 : B00 { idc=9221; x = BAR_WIDTH*21/18; };
                        class B22 : B00 { idc=9222; x = BAR_WIDTH*22/18; };
                        class B23 : B00 { idc=9223; x = BAR_WIDTH*23/18; };
                        class B24 : B00 { idc=9224; x = BAR_WIDTH*24/18; };
                        class B25 : B00 { idc=9225; x = BAR_WIDTH*25/18; };
                        class B26 : B00 { idc=9226; x = BAR_WIDTH*26/18; };
                        class B27 : B00 { idc=9227; x = BAR_WIDTH*27/18; };
                        class B28 : B00 { idc=9228; x = BAR_WIDTH*28/18; };
                        class B29 : B00 { idc=9229; x = BAR_WIDTH*29/18; };
                        class B30 : B00 { idc=9230; x = BAR_WIDTH*30/18; };
                        class B31 : B00 { idc=9231; x = BAR_WIDTH*31/18; };
                        class B32 : B00 { idc=9232; x = BAR_WIDTH*32/18; };
                        class B33 : B00 { idc=9233; x = BAR_WIDTH*33/18; };
                        class B34 : B00 { idc=9234; x = BAR_WIDTH*34/18; };
                        class B35 : B00 { idc=9235; x = BAR_WIDTH*35/18; };
                    };
                };
                class Bar2 : Bar1
                {
                    idc=9002;
                    x = BAR_WIDTH*2;
                    class Controls : Controls
                    {
                        class Indicator : Indicator
                        {
                            text=QPATHTOF(data\sn_ca.paa); // South > West > North
                        };
                        class B00 : B00 {idc=9236;};
                        class B01 : B01 {idc=9237;};
                        class B02 : B02 {idc=9238;};
                        class B03 : B03 {idc=9239;};
                        class B04 : B04 {idc=9240;};
                        class B05 : B05 {idc=9241;};
                        class B06 : B06 {idc=9242;};
                        class B07 : B07 {idc=9243;};
                        class B08 : B08 {idc=9244;};
                        class B09 : B09 {idc=9245;};
                        class B10 : B10 {idc=9246;};
                        class B11 : B11 {idc=9247;};
                        class B12 : B12 {idc=9248;};
                        class B13 : B13 {idc=9249;};
                        class B14 : B14 {idc=9250;};
                        class B15 : B15 {idc=9251;};
                        class B16 : B16 {idc=9252;};
                        class B17 : B17 {idc=9253;};
                        class B18 : B18 {idc=9254;};
                        class B19 : B19 {idc=9255;};
                        class B20 : B20 {idc=9256;};
                        class B21 : B21 {idc=9257;};
                        class B22 : B22 {idc=9258;};
                        class B23 : B23 {idc=9259;};
                        class B24 : B24 {idc=9260;};
                        class B25 : B25 {idc=9261;};
                        class B26 : B26 {idc=9262;};
                        class B27 : B27 {idc=9263;};
                        class B28 : B28 {idc=9264;};
                        class B29 : B29 {idc=9265;};
                        class B30 : B30 {idc=9266;};
                        class B31 : B31 {idc=9267;};
                        class B32 : B32 {idc=9268;};
                        class B33 : B33 {idc=9269;};
                        class B34 : B34 {idc=9270;};
                        class B35 : B35 {idc=9271;};
                    };
                };
                class Center : RscPicture
                {
                    idc=-1;
                    x = (BAR_WIDTH-BAR_HEIGHT)/2;
                    w = BAR_HEIGHT;
                    h = BAR_HEIGHT;
                    text=QPATHTOF(data\center2_ca.paa); // ^
                };
            };
        };
    };
};