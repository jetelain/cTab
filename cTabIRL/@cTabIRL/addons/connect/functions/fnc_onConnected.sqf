#include "script_component.hpp"

params ['_code','_uri'];

INFO_1('Connected to %1', _uri);

player createDiarySubject ["cTab", LLSTRING(modName)];  

private _contentItems = [];

_contentItems pushBack format[LLSTRING(diaryEntryMultiplayer), _uri, GVAR(key)];
_contentItems pushBack '<br />';
_contentItems pushBack LLSTRING(diaryEntryScanQrCode);
_contentItems pushBack format['<br /><br /><font face="QRFONT" size="14">%1</font><br /><br />', _code joinString '<br />' ];
_contentItems pushBack LLSTRING(diaryEntryNote);

private _content = [LLSTRING(diaryTitle), _contentItems joinString ''];

if ( isNil QGVAR(diaryRecord)) then {
	GVAR(diaryRecord) = player createDiaryRecord ["cTab", _content];
} else {
	player setDiaryRecordText [["cTab", GVAR(diaryRecord)], _content];
};

systemChat (format [LLSTRING(connected), _uri]);
systemChat LLSTRING(connectedQrCode);