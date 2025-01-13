class CfgVehicles {
    class Man;
    class CAManBase: Man {
        class ACE_SelfActions {
            class cTab_Interact {
                displayName = "$STR_ctab_core_aceMenu";
                condition = QUOTE(true);
                exceptions[] = {"isNotInside", "isNotSitting"};
                statement = "";
                insertChildren = QUOTE(_this call FUNC(aceSelfActions));
                priority = 0.1;
            };
        };
    };
};