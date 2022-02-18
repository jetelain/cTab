#include "script_component.hpp"

params ['_code','_uri'];

INFO_1('Connected to %1', _uri);

player createDiarySubject ["cTab", LLSTRING(modName)];  

private _contentItems = [];
if ( isMultiplayer ) then {
	_contentItems pushBack format[LLSTRING(diaryEntryMP), _uri, GVAR(key)];
	_contentItems pushBack '<br />';
	_contentItems pushBack LLSTRING(diaryEntryScanQrCodeMP);
} else {
	_contentItems pushBack LLSTRING(diaryEntryScanQrCodeSP);
};
if ( (safezoneW / safezoneH) < 1 ) then {
	_contentItems pushBack format['<br /><br /><font face="QRFONT" size="12">%1</font><br /><br />', _code joinString '<br />' ];
} else {
	_contentItems pushBack format['<br /><br /><font face="QRFONT" size="14">%1</font><br /><br />', _code joinString '<br />' ];
};
_contentItems pushBack LLSTRING(diaryEntryNote);

private _content = [LLSTRING(diaryTitle), _contentItems joinString ''];

if ( isNil QGVAR(diaryRecord)) then {
	GVAR(diaryRecord) = player createDiaryRecord ["cTab", _content];
} else {
	player setDiaryRecordText [["cTab", GVAR(diaryRecord)], _content];
};

systemChat (format [LLSTRING(connected), _uri]);
systemChat LLSTRING(connectedQrCode);

// getResolution => [screenW, screenH, viewPortW, viewPortH, aspectRatio, uiScale, fovTop, fovLeft, tripleHead]
// [5120,1440,1056,792,3.55556,0.55,0.75,2.66667,true]

// [safeZoneW,safeZoneH]
// [1.61616,1.81818]

// (safezoneW / safezoneH)
// 0.888889