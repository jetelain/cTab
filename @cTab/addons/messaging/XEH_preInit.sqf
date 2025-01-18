#include "script_component.hpp"
ADDON = false;
#include "XEH_PREP.hpp"
ADDON = true;

GVAR(templates) = [];
GVAR(isBuiltinTemplates) = true;

if ( LLSTRING(medevac) == "EVASAN") then {
	GVAR(templates) pushBack ["builtin#1",1,"EVASAN","EVASAN","",[["EVASAN","",[]],["Line 1","POSITION UTM",[["","Position UTM",5]]],["Line 2","INDICATIF / FREQUENCE",[["","Indicatif",3],["","Fréquence",4]]],["Line 3","NOMBRE ET PRIORITE DES BLESSES",[["A","P1 (urgent)",1],["B","P2 (urgence chir.)",1],["C","P2 (prioritaire)",1]]],["Line 4","EQUIPEMENTS NECESSAIRES",[["A","néant",6],["B","hélico",6],["C","désincar.",6],["D","O2",6],["E","Autres",0]]],["Line 5","NOMBRE ET TYPE DE BLESSES",[["L","couché",1],["A","ambulatoire",1],["E","accompagnant (obligatoire si enfant)",1]]],["Line 6","SECURITE ZONE",[["N","Sure",6],["P","ennemis possibles",6],["E","ennemis présent",6],["X","escorte armée",6]]],["Line 7","MARQUAGE ZPH",[["A","cyalum, PN2A",6],["B","fusée éclair.",6],["C","fumigène",6],["D","néant",6],["E","autre moyen",0]]],["Line 8","NATIONALITES DES BLESSES",[["A","Militaire FR/OTAN",1],["D","Civils",1],["E","Prisonnier",1],["F","Enfants",1]]],["Line 9","DESCRIPTIF DE LA ZONE ZPH",[["","si différent de Line 1",0]]]]];
} else {
	GVAR(templates) pushBack ["builtin#2",1,"MEDEVAC","MEDEVAC","",[["MEDEVAC","",[]],["Line 1","LOCATION",[["","Grid of pickup zone",5]]],["Line 2","CALL SIGN & FREQ",[["","Call sign",3],["","Frequency",4]]],["Line 3","NUMBER OF PATIENTS/PRECEDENCE",[["A","URGENT Hospital under 90 min",1],["B","PRIORITY Hospital under 4 hours",1],["C","ROUTINE Hospital within 24 hours",1]]],["Line 4","SPECIAL EQUIPMENT REQUIRED",[["A","None",6],["B","Hoist (Winch)",6],["C","Extrication",6],["D","Ventilator",6],["E","Others",0]]],["Line 5","NUMBER TO BE CARRIED LYING/SITTING",[["L","Litter (Stretcher)",1],["A","Ambulatory (Walking)",1],["E","Escorts (e.g. for child patient)",1]]],["Line 6","SECURITY AT PICKUP ZONE (PZ)",[["N","No enemy",6],["P","Possible enemy",6],["E","Enemy in area",6],["X","Hot PZ - Armed escort required",6]]],["Line 7","PICKUP ZONE (PZ) MARKING METHOD",[["A","Panels",6],["B","Pyro",6],["C","Smoke",6],["D","None",6],["E","Other",0]]],["Line 8","NATIONALITY/STATUS",[["A","Military",1],["D","Civilian",1],["E","PW / Detainee",1],["F","Child",1]]],["Line 9","PICKUP ZONE (PZ) TERRAIN/OBSTACLES",[["","Terrain / obstacles",0]]]]];
};
