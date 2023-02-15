using System.Collections.Generic;
using System;
using System.IO;

namespace cTabWebApp
{
    public static class CTabMarkers
    {
        private static string GetUnitSize(string iconB)
        {
            switch (iconB)
            {
                case "\\A3\\ui_f\\data\\map\\markers\\nato\\group_0.paa":
                    return "11";
                case "\\A3\\ui_f\\data\\map\\markers\\nato\\group_1.paa":
                    return "12";
                case "\\A3\\ui_f\\data\\map\\markers\\nato\\group_2.paa":
                    return "13";
                case "\\A3\\ui_f\\data\\map\\markers\\nato\\group_3.paa":
                    return "14";
            }
            return "00";
        }

        // https://spatialillusions.com/unitgenerator/
        private static Dictionary<string, string> icons = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            // BLUE
            { "\\cTab\\img\\b_mech_inf_wheeled.paa"                  , "10031000001211020051" },
            { "\\A3\\ui_f\\data\\map\\markers\\nato\\b_support.paa"  , "10031000001634000000" },
            { "\\A3\\ui_f\\data\\map\\markers\\nato\\b_motor_inf.paa", "10031000001211040000" },
            { "\\A3\\ui_f\\data\\map\\markers\\nato\\b_uav.paa"      , "10031000001219000000" },
            { "\\A3\\ui_f\\data\\map\\markers\\nato\\b_air.paa"      , "10031000001206000000" },
            { "\\A3\\ui_f\\data\\map\\markers\\nato\\b_plane.paa"    , "10031000001208000000" },
            { "\\A3\\ui_f\\data\\map\\markers\\nato\\b_mech_inf.paa" , "10031000001211020000" },
            { "\\A3\\ui_f\\data\\map\\markers\\nato\\b_art.paa"      , "10031000001303000000" },
            { "\\A3\\ui_f\\data\\map\\markers\\nato\\b_armor.paa"    , "10031000001205000000" },
            { "\\A3\\ui_f\\data\\map\\markers\\nato\\b_mortar.paa"   , "10031000001308000000" },
            { "\\A3\\ui_f\\data\\map\\markers\\nato\\b_hq.paa"       , "10031002000000000000" },
            { "\\A3\\ui_f\\data\\map\\markers\\military\\end_CA.paa" , "img:mil_end.png" }, // 10032500001508000000 in theory
            { "\\A3\\ui_f\\data\\map\\markers\\nato\\b_inf.paa"      , "10031000001211000000" },
            { "\\A3\\ui_f\\data\\map\\markers\\military\\dot_CA.paa", "img:mil_dot_blue.png" },
            { "\\cTab\\img\\m_circle.paa",                            "img:mil_circle_blue.png" },
            { "\\cTab\\img\\tic.paa",                                 "img:tic.png" },

            // GREEN
            { "\\A3\\ui_f\\data\\map\\markers\\military\\join_CA.paa"   , "img:mil_join.png" },
            { "\\A3\\ui_f\\data\\map\\markers\\military\\circle_CA.paa" , "img:mil_circle.png" }, // 10032500003205000000 in theory
            { "\\A3\\ui_f\\data\\map\\mapcontrol\\Hospital_CA.paa"      , "10032000001122020000" },
            { "\\A3\\ui_f\\data\\map\\markers\\military\\warning_CA.paa", "img:mil_warning.png" },

            // RED 
            { "\\A3\\ui_f\\data\\map\\markers\\nato\\o_inf.paa"      , "10061000001211000000" },
            { "\\A3\\ui_f\\data\\map\\markers\\nato\\o_mech_inf.paa" , "10061000001211020000" },
            { "\\A3\\ui_f\\data\\map\\markers\\nato\\o_motor_inf.paa", "10061000001211040000" },
            { "\\A3\\ui_f\\data\\map\\markers\\nato\\o_armor.paa"    , "10061000001205000000" },
            { "\\A3\\ui_f\\data\\map\\markers\\nato\\o_air.paa"      , "10061000001206000000" },
            { "\\A3\\ui_f\\data\\map\\markers\\nato\\o_plane.paa"    , "10061000001208000000" },
            { "\\A3\\ui_f\\data\\map\\markers\\nato\\o_unknown.paa"  , "10061000000000000000" },
            { "\\cTab\\img\\o_inf_rifle.paa"                         , "10061500001100000000" },
            { "\\cTab\\img\\o_inf_mg.paa"                            , "10062700001103010000" },
            { "\\cTab\\img\\o_inf_at.paa"                            , "10062700001103160000" },
            { "\\cTab\\img\\o_inf_mmg.paa"                           , "10062700001103030000" },
            { "\\cTab\\img\\o_inf_mat.paa"                           , "10062700001103070000" },
            { "\\cTab\\img\\o_inf_mmortar.paa"                       , "10062700001103140000" },
            { "\\cTab\\img\\o_inf_aa.paa"                            , "10061500001111000000" }
        };

        public static string GetMilSymbol(string iconA, string iconB)
        {
            if (iconA.StartsWith("\\cTab\\img\\mil\\", StringComparison.OrdinalIgnoreCase))
            {
                return Path.GetFileNameWithoutExtension(iconA).PadRight(20, '0');
            }
            var size = GetUnitSize(iconB);
            string symbol;
            if (icons.TryGetValue(iconA, out symbol))
            {
                if (size == "00" || symbol.StartsWith("img:", StringComparison.Ordinal))
                {
                    return symbol;
                }
                return $"{symbol.Substring(0, 8)}{size}{symbol.Substring(10)}";
            }
            return $"10031000{size}0000000000";
        }
    }
}
