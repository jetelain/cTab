class CfgVehicles {
    class Man;
    class CAManBase: Man {
        class ACE_SelfActions {
            class cTab_Interact {
                displayName = "Blue Force Tracking";
                condition = QUOTE(true);
                exceptions[] = {"isNotInside", "isNotSitting"};
                statement = "";
                insertChildren = QUOTE(_this call FUNC(aceSelfActions));
                priority = 0.1;
            };
        };
    };
};