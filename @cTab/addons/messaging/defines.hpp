
// Must be kept in sync with https://github.com/jetelain/Arma3TacMap/blob/main/Arma3TacMapWebApp/Entities/MessageTemplateType.cs
#define MSG_TYPE_GENERIC 0
#define MSG_TYPE_MEDEVAC 1
#define MSG_TYPE_ARTILLERY 2
#define MSG_TYPE_AIRSUPPORT 3

// Must be kept in sync with https://github.com/jetelain/Arma3TacMap/blob/main/Arma3TacMapWebApp/Entities/MessageFieldType.cs
#define MSG_FIELD_TYPE_TEXT 0
#define MSG_FIELD_TYPE_NUMBER 1
#define MSG_FIELD_TYPE_DATETIME 2
#define MSG_FIELD_TYPE_CALLSIGN 3
#define MSG_FIELD_TYPE_FREQUENCY 4
#define MSG_FIELD_TYPE_MARKER 5
#define MSG_FIELD_TYPE_CHECKBOX 6
#define MSG_FIELD_TYPE_GRID 7
#define MSG_FIELD_TYPE_MULTILINE_TEXT 8

#define MSG_ATTACHMENT_MARKER 0
#define MSG_ATTACHMENT_GRID 1

#define pixelScale  0.25
#define GRID_W (pixelW * pixelGridNoUIScale * pixelScale)
#define GRID_H (pixelH * pixelGridNoUIScale * pixelScale)

#define FIELD_IDC(_l,_f) (9900000+_l*1000+_f*10)

#define IDD_TEMPLATE_DIALOG 990560
#define IDC_TEXTPREVIEW     990561
#define IDC_RECIPIENTS   	990562
#define IDC_BUTTON_SEND     990563
#define IDC_BUTTON_CANCEL   990564
#define IDC_ATTACHEMENTS	990565

#define IDC_TEMPLATEBLOCK	990566



#define WIDTH_PER_CHAR 2

#define SCROLLBAR_WIDTH 0.021