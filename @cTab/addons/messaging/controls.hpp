class RscEdit;
class RscText;
class RscControlsGroupNoScrollbars;
class RscListBox;
class cTab_RscButton;

class GVAR(templateDialog)
{
	idd = IDD_TEMPLATE_DIALOG;
	objects[] = {};
};

class GVAR(templateFooter) : RscControlsGroupNoScrollbars
{
	h = QUOTE(GRID_H * 70);
	w = 1;
	x = 0;
	y = 0;

	class controls {

		class message : RscText
		{
			text=CSTRING(GeneratedMessage);
			x = QUOTE(0.5 + (GRID_W * 2));
			y = 0;
			w = QUOTE(0.5 - (GRID_W*4));
			h = QUOTE(GRID_H * 8);
			color[] = {0.5,0.5,0.5,1};
		};

		class textPreview : RscEdit
		{
			idc = IDC_TEXTPREVIEW;
			style = 0x10; // ST_MULTI
			canModify = 0;
			x = QUOTE(0.5 + (GRID_W * 2));
			y = QUOTE(GRID_H * 8);
			w = QUOTE(0.5 - (GRID_W*4) - SCROLLBAR_WIDTH);
			h = QUOTE(GRID_H * 60);
		};

		class recipientLabel : RscText {
			text=CSTRING(Recipient);
			x = QUOTE((GRID_W*2));
			y = QUOTE(0);
			w = QUOTE(0.5 - (GRID_W*4));
			h = QUOTE(GRID_H * 8);
		};

		class recipientSelect: RscListBox
		{
			idc = IDC_RECIPIENTS;
			style = 0x20; // LB_MULTI
			x = QUOTE((GRID_W*2));
			y = QUOTE(GRID_H * 8);
			w = QUOTE(0.5 - (GRID_W*4));
			h = QUOTE(GRID_H * 30);
		};

		class attachementLabel : RscText {
			text=CSTRING(Attachements);
			x = QUOTE((GRID_W*2));
			y = QUOTE(GRID_H * 38);
			w = QUOTE(0.5 - (GRID_W*4));
			h = QUOTE(GRID_H * 8);
		};

		class attachementSelect: RscListBox
		{
			idc = IDC_ATTACHEMENTS;
			x = QUOTE((GRID_W*2));
			y = QUOTE(GRID_H * 46);
			w = QUOTE(0.5 - (GRID_W*4));
			h = QUOTE(GRID_H * 12);
			canModify = 0;
		};

		class sendButton: cTab_RscButton
		{
			idc = IDC_BUTTON_SEND;
			text = CSTRING(Send);
			x = QUOTE(0.25 + (GRID_W*2));
			y = QUOTE(GRID_H * 60);
			w = QUOTE(0.25 - (GRID_W*4));
			h = QUOTE(GRID_H * 8);
			onButtonClick=QUOTE(_this call FUNC(sendTemplatedMessage));
		};

		class cancelButton : cTab_RscButton
		{
			idc = IDC_BUTTON_CANCEL;
			text = CSTRING(Cancel);
			x = QUOTE((GRID_W*2));
			y = QUOTE(GRID_H * 60);
			w = QUOTE(0.25 - (GRID_W*4));
			h = QUOTE(GRID_H * 8);
			onButtonClick=QUOTE([_this # 0] call FUNC(closeTemplateUI));
		};
	};


};
