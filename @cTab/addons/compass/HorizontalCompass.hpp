#define BAR_WIDTH   safezoneW
#define BAR_HEIGHT (0.055 * safezoneH)

class RscControlsGroupNoScrollbars;
class RscMapControlEmpty;
class RscPicture;

class GVARMAIN(HorizontalCompass) : RscControlsGroupNoScrollbars
{
    onLoad=QUOTE(_this call FUNC(initCompass));
    onUnload=QUOTE(_this call FUNC(disposeCompass));
    w = QUOTE(BAR_WIDTH);
    h = QUOTE(BAR_HEIGHT);
    x = QUOTE(safezoneX);
    y = QUOTE(0.845 * safezoneH + safezoneY);

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
            w = QUOTE(BAR_WIDTH);
            h = QUOTE(BAR_HEIGHT);
            class Controls
            {
                class Bar1 : RscControlsGroupNoScrollbars
                {
                    idc=9001;
                    w = QUOTE(BAR_WIDTH*2);
                    h = QUOTE(BAR_HEIGHT);
                    class Controls
                    {
                        class Indicator : RscPicture
                        {
                            idc=9101;
                            w = QUOTE(BAR_WIDTH*2);
                            h = QUOTE(BAR_HEIGHT);
                            text=QPATHTOF(data\ns_ca.paa); // North > East > South
                        };
                    };
                };
                class Bar2 : Bar1
                {
                    idc=9002;
                    x = QUOTE(BAR_WIDTH*2);
                    class Controls : Controls
                    {
                        class Indicator : Indicator
                        {
                            text=QPATHTOF(data\sn_ca.paa); // South > West > North
                        };
                    };
                };
                class Center : RscPicture
                {
                    idc=-1;
                    x = QUOTE((BAR_WIDTH-BAR_HEIGHT)/2);
                    w = QUOTE(BAR_HEIGHT);
                    h = QUOTE(BAR_HEIGHT);
                    text=QPATHTOF(data\center2_ca.paa); // ^
                };
            };
        };
    };
};
